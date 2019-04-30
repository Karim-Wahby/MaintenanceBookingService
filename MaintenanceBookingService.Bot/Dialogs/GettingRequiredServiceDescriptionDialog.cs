namespace MaintenanceBookingService.Bot.Dialogs
{
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Bot.Dialogs.Interfaces;
    using MaintenanceBookingService.Bot.Dialogs.Utilities;
    using MaintenanceBookingService.Bot.Models;
    using Microsoft.Bot.Builder;

    public class GettingRequiredServiceDescriptionDialog : IStatelessDialog
    {
        public GettingRequiredServiceDescriptionDialog(ConversationData conversationData, UserData userProfile) 
            : base(conversationData, userProfile)
        {
        }

        public override Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            ConversationData.ServiceBookingForm.RequiredServiceDescription = ConversationUtils.GetUserReply(turnContext);
            ConversationData.SetWaitingForUserInputFlag(false);
            return Task.FromResult(0);
        }

        public override async Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                           Constants.ServiceFieldsMessages.ServiceDescribtionMessage,
                           UserProfile,
                           turnContext,
                           cancellationToken);

            ConversationData.SetWaitingForUserInputFlag();
        }
    }
}