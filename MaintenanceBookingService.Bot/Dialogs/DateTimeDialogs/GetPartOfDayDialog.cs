namespace MaintenanceBookingService.Bot.Dialogs.DateTimeDialogs
{
    using System;
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

        public override async Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var userInput = Utilities.ConversationUtils.GetUserReply(turnContext);
            if (Utilities.DialogUtils.IsUserInputInOptions(userInput, Constants.ServiceFieldsMessages.PMOptionValues))
            {
                this.ConversationData.ServiceBookingForm.DayOrNight = "PM";
                this.ConversationData.SetWaitingForUserInputFlag(false);
            }
            else if (Utilities.DialogUtils.IsUserInputInOptions(userInput, Constants.ServiceFieldsMessages.AMOptionValues))
            {
                this.ConversationData.ServiceBookingForm.DayOrNight = "AM";
                this.ConversationData.SetWaitingForUserInputFlag(false);
            }
            else
            {
                await Utilities.ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                    Dialogs.Constants.General.InvalidValueProvided,
                    UserProfile,
                    turnContext,
                    cancellationToken);
            }
        }

        public override async Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await Utilities.ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                Constants.ServiceFieldsMessages.ServiceDeliveryPartOfDayMessage,
                this.UserProfile,
                turnContext,
                cancellationToken);

            this.ConversationData.SetWaitingForUserInputFlag();
        }
    }
}