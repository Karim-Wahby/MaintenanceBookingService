namespace MaintenanceBookingService.Bot.Dialogs
{
    using MaintenanceBookingService.Bot.Dialogs.Interfaces;
    using MaintenanceBookingService.Bot.Dialogs.Utilities;
    using MaintenanceBookingService.Bot.Models;
    using Microsoft.Bot.Builder;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class SelectingUserIntentDialog : IStatelessDialog
    {
        public SelectingUserIntentDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        public override async Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await Utilities.ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                    Constants.UserIntentSelection.IntentSelection,
                    UserProfile,
                    turnContext,
                    cancellationToken);
            ConversationData.SetWaitingForUserInputFlag();
        }

        public override async Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            if (ConversationData.WaitingForUserInput)
            {
                var userInput = ConversationUtils.GetUserReply(turnContext);
                if (DialogUtils.IsUserInputInOptions(userInput, Constants.UserIntentSelection.ReservationPossibleSelectionValues))
                {
                    ConversationData.CurrentConversationIntent = BotSupportedIntents.MaintenanceBookingService;
                }
                else if (DialogUtils.IsUserInputInOptions(userInput, Constants.UserIntentSelection.CheckingStatusPossibleSelectionValues))
                {
                    ConversationData.CurrentConversationIntent = BotSupportedIntents.GettingUpdatesAboutCurrentRequests;
                }
                else
                {
                    await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                        Constants.General.InvalidValueProvided,
                        this.UserProfile,
                        turnContext,
                        cancellationToken);
                }

                if (ConversationData.CurrentConversationIntent.HasValue)
                {
                    ConversationData.SetWaitingForUserInputFlag(false);
                }
            }
        }
    }
}
