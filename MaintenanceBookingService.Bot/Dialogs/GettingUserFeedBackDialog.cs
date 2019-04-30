namespace MaintenanceBookingService.Bot.Dialogs
{
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Bot.Dialogs.Definitions;
    using MaintenanceBookingService.Bot.Dialogs.Interfaces;
    using MaintenanceBookingService.Bot.Dialogs.Utilities;
    using MaintenanceBookingService.Bot.Managers;
    using MaintenanceBookingService.Bot.Models;
    using Microsoft.Bot.Builder;

    public class GettingUserFeedBackDialog : IEventActivatedDialog
    {
        protected GettingUserFeedBackDialog(DialogStateData storedDialogState)
            : base(storedDialogState)
        {
        }

        public GettingUserFeedBackDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        public GettingUserFeedBackDialog(string key, ConversationData oldConversationData, UserData oldUserProfile)
            : base(key, oldConversationData, oldUserProfile)
        {
        }

        public override async Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await RevertToNormalStateAsync(turnContext, cancellationToken);
        }

        public override Task<string> KeySelectionFunction(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            return Task.FromResult<string>(this.ConversationData.NewUserMaintenanceServiceId);
        }

        public override async Task ProActiveMessageToUseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var messageFormattingValues = new string[]
                {
                    this.ConversationData.NewUserMaintenanceServiceId,
                    this.ConversationData.ServiceBookingForm.RequiredServiceDescription
                };

            await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                           Constants.RequestStatusUpdate.ServiceRequestDeliveredMessage,
                           UserProfile,
                           turnContext,
                           cancellationToken,
                           formattingValues: new MessageOption()
                           {
                               Arabic = messageFormattingValues,
                               English = messageFormattingValues
                           });

            ConversationData.SetWaitingForUserInputFlag();
        }

        public override Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            return base.StartAsync(turnContext, cancellationToken);
        }
    }
}