namespace BookingAMaintenanceService.Dialogs.Interfaces
{
    using BookingAMaintenanceService.Models;
    using Microsoft.Bot.Builder;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class IStatefulDialog<DialogStateEnum> : IDialog
        where DialogStateEnum : System.Enum 
    {
        protected bool currentDialogStatusHasValue = false;
        protected DialogStateEnum currentDialogStatus;
        protected ConversationData conversationData;
        protected UserData userProfile;

        public IStatefulDialog(ConversationData conversationData, UserData userProfile)
        {
            this.currentDialogStatusHasValue = conversationData.DialogsStatuses.ContainsKey(this.GetType().Name);
            this.currentDialogStatus = this.currentDialogStatusHasValue ? ((DialogStateEnum)Enum.Parse(typeof(DialogStateEnum), conversationData.DialogsStatuses[this.GetType().Name])) : default(DialogStateEnum);
            this.conversationData = conversationData;
            this.userProfile = userProfile;
        }

        public async Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await GetSuitableSubDialogBasedOnStatus().HandleIncomingUserResponseAsync(turnContext, cancellationToken);
        }

        public async Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            DetermineWhatShouldtheCurrentDialogStatusBe();
            await GetSuitableSubDialogBasedOnStatus().StartAsync(turnContext, cancellationToken);
        }

        protected abstract void DetermineWhatShouldtheCurrentDialogStatusBe();

        protected abstract IDialog GetSuitableSubDialogBasedOnStatus();

        public void SetCurrentDialogStatus(DialogStateEnum newState)
        {
            this.currentDialogStatus = newState;
            this.currentDialogStatusHasValue = true;
            if (this.conversationData.DialogsStatuses.ContainsKey(this.GetType().Name))
            {
                this.conversationData.DialogsStatuses[this.GetType().Name] = newState.ToString();
            }
            else
            {
                this.conversationData.DialogsStatuses.Add(this.GetType().Name, newState.ToString());
            }
        }
    }
}
