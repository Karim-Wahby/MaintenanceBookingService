namespace MaintenanceBookingService.Bot.Dialogs.DateTimeDialogs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Bot.Dialogs.Interfaces;
    using MaintenanceBookingService.Bot.Models;
    using Microsoft.Bot.Builder;

    public class GetMinuteDialog : IStatelessDialog
    {
        public GetMinuteDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        public override async Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var userInput = Utilities.ConversationUtils.GetUserReply(turnContext);
            var currentDate = DateTime.Now;
            int userRequiredMinute;
            if (int.TryParse(userInput, out userRequiredMinute) &&
                userRequiredMinute > 0 &&
                userRequiredMinute < 60)
            {
                if (currentDate.Year < this.ConversationData.ServiceBookingForm.Year ||
                    currentDate.Month < this.ConversationData.ServiceBookingForm.Month ||
                    currentDate.Day < this.ConversationData.ServiceBookingForm.Day ||
                    currentDate.Hour + 1 < this.ConversationData.ServiceBookingForm.Hour ||
                    currentDate.Minute <= userRequiredMinute)
                {
                    this.ConversationData.ServiceBookingForm.Minutes = userRequiredMinute;
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
                Constants.ServiceFieldsMessages.ServiceDeliveryMinuteMessage,
                this.UserProfile,
                turnContext,
                cancellationToken);
        }
    }
}