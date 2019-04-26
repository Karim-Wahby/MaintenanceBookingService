namespace BookingAMaintenanceService.Dialogs
{
    using BookingAMaintenanceService.Dialogs.Interfaces;
    using BookingAMaintenanceService.Models;
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
            if (Utilities.DialogUtils.IsUserInputInOptions(userInput, Constants.ServiceFieldsMessages.AsSoonAsPossibleOptionValues))
            {
                userRequestedTime = DateTime.Now.AddHours(1).TimeOfDay;
            }
            else if (!Utilities.DialogUtils.TryGetTimeFromUserInput(userInput, out userRequestedTime))
            {
                conversationData.ServiceBookingForm.FailedToRecognizeProvidedDate = true;
            }

            if (userRequestedTime.HasValue)
            {
                conversationData.SetWaitingForUserInputFlag(false);
                conversationData.ServiceBookingForm.Hour = userRequestedTime.Value.Hours;
                conversationData.ServiceBookingForm.Minutes = userRequestedTime.Value.Minutes;
                conversationData.ServiceBookingForm.DayOrNight = userRequestedTime.Value.Hours > 12 ? "PM" : "AM";
            }
        }

        public override async Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await Utilities.ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                    Constants.ServiceFieldsMessages.ServiceDeliveryTimeMessage,
                    userProfile,
                    turnContext,
                    cancellationToken);

            conversationData.SetWaitingForUserInputFlag();
        }
    }
}