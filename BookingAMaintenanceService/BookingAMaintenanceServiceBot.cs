// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookingAMaintenanceService.Managers;
using BookingAMaintenanceService.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace BookingAMaintenanceService
{
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
    public class BookingAMaintenanceServiceBot : IBot
    {
        private readonly ConversationStateDataAccessors conversationStateDataAccessor;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>                        
        public BookingAMaintenanceServiceBot(ConversationStateDataAccessors accessors)
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
                    await HandleIncommingMessages(turnContext, cancellationToken);
                    break;
                case ActivityTypes.DeleteUserData:
                    await handleDeleteUserDataRequest(turnContext, cancellationToken);
                    break;
                case ActivityTypes.ConversationUpdate:
                    await HandleConverstationUpdates(turnContext, cancellationToken);
                    break;
                default:
                    break;
            }
        }

        private async Task handleDeleteUserDataRequest(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await conversationStateDataAccessor.DeleteUserCachedData(turnContext);
            await SendMessage("User Data Deleted (Y)!", turnContext, cancellationToken);
        }

        private async Task HandleConverstationUpdates(ITurnContext turnContext, CancellationToken cancellationToken)
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

        private static async Task SendMessage(string message, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync(message, cancellationToken: cancellationToken);

        }

        public static void SetWaitingForUserInputFlag(ConversationData conversationData, bool value = true)
        {
            conversationData.WaitingForUserInput = value;
        }

        public static string GetUserReply(ITurnContext turnContext)
        {
            return turnContext.Activity.Text;
        }

        private async Task SayHelloToUsers(IEnumerable<ChannelAccount> addedUsers, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            if (addedUsers == null || !addedUsers.Any())
            {
                return;
            }

            foreach (ChannelAccount member in addedUsers)
            {
                await SendMessage($"Hi there - {member.Name}.{Environment.NewLine}Welcome To SALA7LEE.",
                    turnContext, cancellationToken);
            }
        }

        private async Task HandleIncommingMessages(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Get the state properties from the turn context.
            var userProfile = await conversationStateDataAccessor.GetUserData(turnContext);
            var conversationData = await conversationStateDataAccessor.GetConversationData(turnContext);

            do
            {
                // case ConversationPhases.UpdatingTheUserWithHisBookingRequestsStatus:
                //     break;
                // case ConversationPhases.RequestingUserFeedbackOfDeliveredService:
                //     break;
                if (!userProfile.PreferredLanguage.HasValue)
                {
                    // it's Time To ask the user for his preferred language
                    await SelectingUserPreferredLanguage(turnContext, cancellationToken, conversationData, userProfile);
                }
                else if (!conversationData.CurrentConversationIntent.HasValue)
                {
                    // it's time to know what is the user intent 
                    await SelectingUserIntentFromConversation(turnContext, cancellationToken, conversationData, userProfile);
                }
                else if (conversationData.CurrentConversationIntent == BotSupportedIntents.BookingAMaintenanceService)
                {
                    // the user want to book one of our services
                    await BookingAMaintenanceServiceConversation(turnContext, cancellationToken, conversationData, userProfile);
                }
                else if (conversationData.CurrentConversationIntent == BotSupportedIntents.GettingUpdatesAboutCurrentRequests)
                {
                    await SendMessage("getting updates about current requests dialog", turnContext, cancellationToken);
                    userProfile = new UserData();
                    conversationData = new ConversationData();
                }
                else
                {
                    await SendMessage("Next Step !!!", turnContext, cancellationToken);
                    userProfile = new UserData();
                    conversationData = new ConversationData();
                }
            } while (!conversationData.WaitingForUserInput) ;
            
            await conversationStateDataAccessor.UpdateUserData(turnContext, userProfile);
            await conversationStateDataAccessor.UpdateConversationData(turnContext, conversationData);
        }

        private async Task BookingAMaintenanceServiceConversation(ITurnContext turnContext, CancellationToken cancellationToken, ConversationData conversationData, UserData userProfile)
        {
            if (conversationData.WaitingForUserInput)
            {
                // if (conversationData.)
            }
            else
            {
            }
            // await SendMessage("booking a maintenance service dialog", turnContext, cancellationToken);
        }

        private async Task SelectingUserIntentFromConversation(ITurnContext turnContext, CancellationToken cancellationToken, ConversationData conversationData, UserData userProfile)
        {
            if (conversationData.WaitingForUserInput)
            {
                var userInput = GetUserReply(turnContext).Trim();
                switch (userInput)
                {
                    case "1":
                        conversationData.CurrentConversationIntent = BotSupportedIntents.BookingAMaintenanceService;
                        break;
                    case "2":
                        conversationData.CurrentConversationIntent = BotSupportedIntents.GettingUpdatesAboutCurrentRequests;
                        break;
                }

                if (conversationData.CurrentConversationIntent.HasValue)
                {
                    SetWaitingForUserInputFlag(conversationData, false);
                    return;
                }

                if (userProfile.PreferredLanguage.Value == SupportedLanguage.Arabic)
                {
                    userInput = userInput.ToLower();
                    await SendMessage($" بالعربى {Environment.NewLine}" +
                        $"The value that you Entired couldn't be recognized,{Environment.NewLine}" +
                        "please select one of the provided options (1, 2, ...)",
                        turnContext,
                        cancellationToken);
                }
                else
                {
                    if (userInput.Contains("status") || userInput.Contains("check"))
                    {
                        conversationData.CurrentConversationIntent = BotSupportedIntents.GettingUpdatesAboutCurrentRequests;
                    }
                    else if (userInput.Contains("book") || userInput.Contains("new"))
                    {
                        conversationData.CurrentConversationIntent = BotSupportedIntents.BookingAMaintenanceService;
                    }
                    else
                    {
                        userInput = userInput.ToLower();
                        await SendMessage($"The value that you Entired couldn't be recognized,{Environment.NewLine}" +
                            "please select one of the provided options (1, 2, ...)",
                            turnContext,
                            cancellationToken);
                    }
                }


                if (conversationData.CurrentConversationIntent.HasValue)
                {
                    SetWaitingForUserInputFlag(conversationData, false);
                    return;
                }
            }
            else
            {
                if (userProfile.PreferredLanguage.Value == SupportedLanguage.Arabic)
                {
                    await SendMessage($"عربى SelectingUserIntentFromConversation", turnContext, cancellationToken);
                }
                else
                {
                    await SendMessage($"How Can I Help You Today:{Environment.NewLine}" +
                        $"1- Book a new Maintenance Service {Environment.NewLine}" +
                        $"2- Check your Requests Status{Environment.NewLine}",
                        turnContext,
                        cancellationToken);
                }

                SetWaitingForUserInputFlag(conversationData);
            }
        }

        private async Task SelectingUserPreferredLanguage(ITurnContext turnContext, CancellationToken cancellationToken, ConversationData conversationData, UserData userProfile)
        {
            if (conversationData.WaitingForUserInput)
            {
                var userInput = GetUserReply(turnContext).ToLower().Trim();
                switch (userInput)
                {
                    case "english":
                    case "eng":
                    case "1":
                        userProfile.PreferredLanguage = SupportedLanguage.English;
                        break;
                    case "ar":
                    case "arabic":
                    case "عربى":
                    case "2":
                        userProfile.PreferredLanguage = SupportedLanguage.Arabic;
                        break;
                    default:
                        await SendMessage($"The value that you Entired couldn't be recognized,{Environment.NewLine}" +
                            "please select one of the provided options (1, 2, ...)",
                            turnContext,
                            cancellationToken);
                        break;
                }

                if (userProfile.PreferredLanguage.HasValue)
                {
                    SetWaitingForUserInputFlag(conversationData, false);
                }
            }
            else
            {
                await SendMessage(
                $"Please Select Your prefered Language:-{Environment.NewLine}" +
                $"أختر اللغه التى تفضلها رجاء{Environment.NewLine}" +
                $"1- English{Environment.NewLine}" +
                $"2- عربى",
                turnContext,
                cancellationToken);

                SetWaitingForUserInputFlag(conversationData);
            }
        }
    }
}
