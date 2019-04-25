namespace BookingAMaintenanceService.Dialogs.Interfaces
{
    using BookingAMaintenanceService.Models;
    using Microsoft.Bot.Builder;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class IStatefulDialog<DialogStateEnum> : IDialog
        where DialogStateEnum : System.Enum 
    {
        private DialogStateEnum currentDialogState;
        private ConversationData conversationData;
        private UserData userProfile;

        public IStatefulDialog(DialogStateEnum currentDialogState, ConversationData conversationData, UserData userProfile)
        {
            this.currentDialogState = currentDialogState;
            this.conversationData = conversationData;
            this.userProfile = userProfile;
        }

        public abstract Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken);

        public abstract Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken);
    }
}
