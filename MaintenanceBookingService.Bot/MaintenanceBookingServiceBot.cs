// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace MaintenanceBookingService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Managers;
    using MaintenanceBookingService.Models;
    using Microsoft.Recognizers.Text;
    using Microsoft.Recognizers.Text.DateTime;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Schema;
    using MaintenanceBookingService.Dialogs.Interfaces;
    using MaintenanceBookingService.Dialogs;
    using MaintenanceBookingService.Dialogs.Utilities;

    /// <summary>
    /// Represents a bot that processes incoming activities.
    /// For each user interaction, an instance of this class is created and the OnTurnAsync method is called.
    /// This is a Transient lifetime service. Transient lifetime services are created
    /// each time they're requested. Objects that are expensive to construct, or have a lifetime
    /// beyond a single turn, should be carefully managed.
    /// For example, the <see cref="MemoryStorage"/> object and associated
    /// <see cref="IStatePropertyAccessor{T}"/> object are created with a singleton lifetime.
    /// </summary>
    /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1"/>
    public class MaintenanceBookingServiceBot : IBot
    {
        private readonly ConversationStateDataAccessors conversationStateDataAccessor;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>                        
        public MaintenanceBookingServiceBot(ConversationStateDataAccessors accessors)
        {
            conversationStateDataAccessor = accessors ?? throw new ArgumentNullException(nameof(accessors));
        }

        /// <summary>
        /// Every conversation turn calls this method.
        /// </summary>
        /// <param name="turnContext">A <see cref="ITurnContext"/> containing all the data needed
        /// for processing this conversation turn. </param>
        /// <param name="cancellationToken">(Optional) A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> that represents the work queued to execute.</returns>
        /// <seealso cref="BotStateSet"/>
        /// <seealso cref="ConversationState"/>
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Handle Message activity type, which is the main activity type for shown within a conversational interface
            // Message activities may contain text, speech, interactive cards, and binary or unknown attachments.
            // see https://aka.ms/about-bot-activity-message to learn more about the message and other activity types
            switch (turnContext.Activity.Type)
            {
                case ActivityTypes.Message:
                    await HandleIncommingMessage(turnContext, cancellationToken);
                    break;
                case ActivityTypes.DeleteUserData:
                    await HandleDeleteUserDataRequest(turnContext, cancellationToken);
                    break;
                case ActivityTypes.ConversationUpdate:
                    await HandleConverstationUpdate(turnContext, cancellationToken);
                    break;
                default:
                    break;
            }
        }

        private async Task HandleDeleteUserDataRequest(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await conversationStateDataAccessor.DeleteUserCachedData(turnContext);
            await Dialogs.Utilities.ConversationUtils.SendMessage("User Data Deleted (Y)!", turnContext, cancellationToken);
        }

        private async Task HandleConverstationUpdate(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            if (turnContext.Activity.MembersAdded != null)
            {
                var theBotId = turnContext.Activity.Recipient.Id;
                var addedUsers = turnContext.Activity.MembersAdded
                    .Where(addedMember => addedMember.Id != theBotId);

                if (addedUsers.Any())
                {
                    await SayHelloToUsers(addedUsers, turnContext, cancellationToken);
                }
            }
        }

        private async Task HandleIncommingMessage(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            var userProfile = await conversationStateDataAccessor.GetUserData(turnContext);
            var conversationData = await conversationStateDataAccessor.GetConversationData(turnContext);

            do
            {
                var suitableDialog = GetSuitableDialog(userProfile, conversationData);

                if (!conversationData.WaitingForUserInput)
                {
                    await suitableDialog.StartAsync(turnContext, cancellationToken);
                }
                else
                {
                    if (DialogUtils.ValidateUserInputIsNotEmpty(ConversationUtils.GetUserReply(turnContext)))
                    {
                        await suitableDialog.HandleIncomingUserResponseAsync(turnContext, cancellationToken);
                    }
                    else
                    {
                        await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                            Dialogs.Constants.General.EmptyValueProvided,
                            userProfile,
                            turnContext,
                            cancellationToken
                            );
                    }
                }

            } while (!conversationData.WaitingForUserInput) ;
            
            await conversationStateDataAccessor.UpdateUserData(turnContext, userProfile);
            await conversationStateDataAccessor.UpdateConversationData(turnContext, conversationData);
        }

        // Dialog Factory
        private IDialog GetSuitableDialog(UserData userProfile, ConversationData conversationData = null)
        {
            if (userProfile == null)
            {
                throw new ArgumentNullException(nameof(userProfile));
            }

            if (conversationData == null)
            {
                throw new ArgumentNullException(nameof(conversationData));
            }

            if (!userProfile.PreferredLanguage.HasValue)
            {
                return new GreetingAndLanguageSelectionDialog(conversationData, userProfile);
            }
            else if (!conversationData.CurrentConversationIntent.HasValue)
            {
                return new SelectingUserIntentDialog(conversationData, userProfile);
            }
            else if (conversationData.CurrentConversationIntent == BotSupportedIntents.MaintenanceBookingService)
            {
                return new MaintenanceBookingServiceDialog(conversationData, userProfile);
            }
            else if (conversationData.CurrentConversationIntent == BotSupportedIntents.GettingUpdatesAboutCurrentRequests)
            {
                // the user want to know what his current request status
            }

            // fallback dialog
            return new GreetingAndLanguageSelectionDialog(conversationData, userProfile);
        }

        private async Task SayHelloToUsers(IEnumerable<ChannelAccount> addedUsers, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            if (addedUsers == null || !addedUsers.Any())
            {
                return;
            }

            // TODO:- handel more than one user added to the conversation
            // currently only one user is supported (the last user in case of more than one user
            foreach (ChannelAccount member in addedUsers)
            {
                var userProfile = await conversationStateDataAccessor.GetUserData(turnContext);
                var conversationData = await conversationStateDataAccessor.GetConversationData(turnContext);
                conversationData.InitializeConversationDataFromDialogContext(turnContext);
                userProfile.InitializeConversationDataFromDialogContext(turnContext);

                await GetSuitableDialog(userProfile, conversationData).StartAsync(turnContext, cancellationToken);
                await conversationStateDataAccessor.UpdateUserData(turnContext, userProfile);
                await conversationStateDataAccessor.UpdateConversationData(turnContext, conversationData);
            }
        }
    }
}
