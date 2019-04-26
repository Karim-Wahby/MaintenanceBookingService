namespace BookingAMaintenanceService.Dialogs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using BookingAMaintenanceService.Dialogs.Interfaces;
    using BookingAMaintenanceService.Models;
    using Microsoft.Bot.Builder;

    public class ConfirmingServiceInfoDialog : IStatelessDialog
    {
        public ConfirmingServiceInfoDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        public override Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public override async Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await Utilities.ConversationUtils.SendMessage(
                        $"Your Request is being processed, we will keep you posted with the updates",
                        turnContext,
                        cancellationToken);

            conversationData.SetWaitingForUserInputFlag(true);
        }
    }
}