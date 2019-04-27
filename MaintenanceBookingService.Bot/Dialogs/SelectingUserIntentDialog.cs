namespace MaintenanceBookingService.Dialogs
{
    using MaintenanceBookingService.Dialogs.Interfaces;
    using MaintenanceBookingService.Dialogs.Utilities;
    using MaintenanceBookingService.Models;
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
                    conversationData.CurrentConversationIntent = BotSupportedIntents.MaintenanceBookingService;
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
