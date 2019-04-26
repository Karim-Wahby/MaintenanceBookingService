namespace BookingAMaintenanceService.Dialogs
{
    using BookingAMaintenanceService.Dialogs.Interfaces;
    using BookingAMaintenanceService.Dialogs.Utilities;
    using BookingAMaintenanceService.Models;
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
                    userProfile,
                    turnContext,
                    cancellationToken);
            conversationData.SetWaitingForUserInputFlag();
        }

        public override async Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            if (conversationData.WaitingForUserInput)
            {
                var userInput = ConversationUtils.GetUserReply(turnContext);
                if (DialogUtils.IsUserInputInOptions(userInput, Constants.UserIntentSelection.ReservationPossibleSelectionValues))
                {
                    conversationData.CurrentConversationIntent = BotSupportedIntents.BookingAMaintenanceService;
                }
                else if (DialogUtils.IsUserInputInOptions(userInput, Constants.UserIntentSelection.CheckingStatusPossibleSelectionValues))
                {
                    conversationData.CurrentConversationIntent = BotSupportedIntents.GettingUpdatesAboutCurrentRequests;
                }
                else
                {
                    await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                        Constants.General.InvalidValueProvided,
                        this.userProfile,
                        turnContext,
                        cancellationToken);
                }

                if (conversationData.CurrentConversationIntent.HasValue)
                {
                    conversationData.SetWaitingForUserInputFlag(false);
                }
            }
        }
    }
}
