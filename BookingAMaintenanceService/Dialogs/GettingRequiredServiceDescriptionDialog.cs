namespace BookingAMaintenanceService.Dialogs
{
    using System.Threading;
    using System.Threading.Tasks;
    using BookingAMaintenanceService.Dialogs.Interfaces;
    using BookingAMaintenanceService.Dialogs.Utilities;
    using BookingAMaintenanceService.Models;
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