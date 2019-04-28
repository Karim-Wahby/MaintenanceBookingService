namespace MaintenanceBookingService.Bot.Dialogs
{
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Bot.Dialogs.Interfaces;
    using MaintenanceBookingService.Bot.Dialogs.Utilities;
    using MaintenanceBookingService.Bot.Models;
    using Microsoft.Bot.Builder;

    public class GettingUserFeedBackDialog : IStatelessDialog
    {
        public GettingUserFeedBackDialog(ConversationData conversationData, UserData userProfile) 
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