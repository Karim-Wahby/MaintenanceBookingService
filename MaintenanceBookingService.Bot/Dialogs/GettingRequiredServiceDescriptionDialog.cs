namespace MaintenanceBookingService.Dialogs
{
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Dialogs.Interfaces;
    using MaintenanceBookingService.Dialogs.Utilities;
    using MaintenanceBookingService.Models;
    using Microsoft.Bot.Builder;

    public class GettingRequiredServiceDescriptionDialog : IStatelessDialog
    {
        public GettingRequiredServiceDescriptionDialog(ConversationData conversationData, UserData userProfile) 
            : base(conversationData, userProfile)
        {
        }

        public override Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            conversationData.ServiceBookingForm.RequiredServiceDescription = ConversationUtils.GetUserReply(turnContext);
            conversationData.SetWaitingForUserInputFlag(false);
            return Task.FromResult(0);
        }

        public override async Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                           Constants.ServiceFieldsMessages.ServiceDescribtionMessage,
                           userProfile,
                           turnContext,
                           cancellationToken);

            conversationData.SetWaitingForUserInputFlag();
        }
    }
}