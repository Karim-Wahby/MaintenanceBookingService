namespace MaintenanceBookingService.Bot.Dialogs.DateTimeDialogs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Bot.Dialogs.Interfaces;
    using MaintenanceBookingService.Bot.Models;
    using Microsoft.Bot.Builder;

    public class GetHourDialog : IStatelessDialog
    {
        public GetHourDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        public override async Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var userInput = Utilities.ConversationUtils.GetUserReply(turnContext);
            var currentDate = DateTime.Now;
            int userRequiredHour;
            if (int.TryParse(userInput, out userRequiredHour) &&
                userRequiredHour > 0 &&
                userRequiredHour < 24)
            {
                if (currentDate.Year < this.ConversationData.ServiceBookingForm.Year ||
                    currentDate.Month < this.ConversationData.ServiceBookingForm.Month ||
                    currentDate.Day < this.ConversationData.ServiceBookingForm.Day ||
                    currentDate.Hour < userRequiredHour)
                {

                    this.ConversationData.ServiceBookingForm.Hour = userRequiredHour % 12;
                    if (userRequiredHour > 12)
                    {
                        this.ConversationData.ServiceBookingForm.DayOrNight = "PM";
                    }

                    this.ConversationData.SetWaitingForUserInputFlag(false);
                }
                else
                {
                    await Utilities.ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                        Dialogs.Constants.ServiceFieldsMessages.TimeInThePastErrorMessage,
                        UserProfile,
                        turnContext,
                        cancellationToken);
                }
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
                Constants.ServiceFieldsMessages.ServiceDeliveryHourMessage,
                this.UserProfile,
                turnContext,
                cancellationToken);

            this.ConversationData.SetWaitingForUserInputFlag();
        }
    }
}