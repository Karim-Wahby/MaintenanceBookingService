namespace MaintenanceBookingService.Bot.Dialogs.DateTimeDialogs
{
    using MaintenanceBookingService.Bot.Dialogs.Interfaces;
    using MaintenanceBookingService.Bot.Models;
    using Microsoft.Bot.Builder;
    using Microsoft.Recognizers.Text;
    using Microsoft.Recognizers.Text.DateTime;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class TryGettingFullTimeDialog : IStatelessDialog
    {
        public TryGettingFullTimeDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        public override async Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var userInput = Utilities.ConversationUtils.GetUserReply(turnContext);
            TimeSpan? userRequestedTime = null;
            if (Utilities.DialogUtils.IsUserInputInOptions(userInput, Dialogs.Constants.ServiceFieldsMessages.AsSoonAsPossibleOptionValues))
            {
                var todayDateAndTime = DateTime.Now;
                if (ConversationData.ServiceBookingForm.Year > todayDateAndTime.Year ||
                    ConversationData.ServiceBookingForm.Month > todayDateAndTime.Month ||
                    ConversationData.ServiceBookingForm.Day > todayDateAndTime.Day)
                {
                    userRequestedTime = new TimeSpan(8, 0, 0);
                }
                else
                {
                    userRequestedTime = todayDateAndTime.AddHours(1).TimeOfDay;
                    if (userRequestedTime.Value.Hours < todayDateAndTime.Hour)
                    {
                        userRequestedTime = new TimeSpan(23, 59, 59);
                    }
                }
            }
            else if (!Utilities.DialogUtils.TryGetTimeFromUserInput(userInput, out userRequestedTime))
            {
                ConversationData.ServiceBookingForm.FailedToRecognizeProvidedDate = true;
            }

            if (userRequestedTime.HasValue)
            {
                ConversationData.SetWaitingForUserInputFlag(false);
                ConversationData.ServiceBookingForm.Hour = userRequestedTime.Value.Hours % 12;
                ConversationData.ServiceBookingForm.Minutes = userRequestedTime.Value.Minutes;
                ConversationData.ServiceBookingForm.DayOrNight = userRequestedTime.Value.Hours > 12 ? "PM" : "AM";
            }
        }

        public override async Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await Utilities.ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                    Dialogs.Constants.ServiceFieldsMessages.ServiceDeliveryTimeMessage,
                    UserProfile,
                    turnContext,
                    cancellationToken);

            ConversationData.SetWaitingForUserInputFlag();
        }
    }
}