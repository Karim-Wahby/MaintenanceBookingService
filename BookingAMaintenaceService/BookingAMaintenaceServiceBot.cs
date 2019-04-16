// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookingAMaintenaceService.Managers;
using BookingAMaintenaceService.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace BookingAMaintenaceService
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
    public class BookingAMaintenaceServiceBot : IBot
    {
        private readonly ConversationStateDataAccessors conversationStateDataAccessor;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>                        
        public BookingAMaintenaceServiceBot(ConversationStateDataAccessors accessors)
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
                    await HandleConverstationupdates(turnContext, cancellationToken);
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

        private async Task HandleConverstationupdates(ITurnContext turnContext, CancellationToken cancellationToken)
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

            await turnContext.SendActivityAsync($"Hello World Turn {++conversationData.Counter}", cancellationToken: cancellationToken);

            await conversationStateDataAccessor.UpdateUserData(turnContext, userProfile);
            await conversationStateDataAccessor.UpdateConversationData(turnContext, conversationData);
        }
    }
}
