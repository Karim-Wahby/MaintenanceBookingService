namespace MaintenanceBookingService.Bot.Dialogs.DateTimeDialogs
{
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Bot.Dialogs.Interfaces;
    using MaintenanceBookingService.Bot.Models;
    using Microsoft.Bot.Builder;

    public class GetPartOfDayDialog : IStatelessDialog
    {
        public GetPartOfDayDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        public override Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public override Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}