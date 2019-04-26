namespace BookingAMaintenanceService.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using BookingAMaintenanceService.Dialogs.Interfaces;
    using BookingAMaintenanceService.Models;
    using Microsoft.Bot.Builder;
    using Microsoft.Recognizers.Text;
    using Microsoft.Recognizers.Text.DateTime;

    public class TryGettingFullDateDialog : IStatelessDialog
    {
        public TryGettingFullDateDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        public override async Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var userInput = Utilities.ConversationUtils.GetUserReply(turnContext);
            DateTime? userRequestedDate = null;
            if (Utilities.DialogUtils.IsUserInputInOptions(userInput, Constants.ServiceFieldsMessages.TodayOptionValues))
            {
                userRequestedDate = DateTime.Now;
            }
            else if (Utilities.DialogUtils.IsUserInputInOptions(userInput, Constants.ServiceFieldsMessages.TomorrowOptionValues))
            {
                userRequestedDate = DateTime.Now.AddDays(1);
            }
            else if (!Utilities.DialogUtils.TryGetDateFromUserInput(userInput, out userRequestedDate))
            {
                conversationData.ServiceBookingForm.FailedToRecognizeProvidedDate = true;
            }

            if (userRequestedDate.HasValue)
            {
                if (userRequestedDate < DateTime.Now.Date)
                {
                    await Utilities.ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                        Constants.ServiceFieldsMessages.DateInThePastErrorMessage,
                        userProfile,
                        turnContext,
                        cancellationToken);
                }
                else
                {
                    conversationData.SetWaitingForUserInputFlag(false);
                    conversationData.ServiceBookingForm.Day = userRequestedDate.Value.Day;
                    conversationData.ServiceBookingForm.Month = userRequestedDate.Value.Month;
                    conversationData.ServiceBookingForm.Year = userRequestedDate.Value.Year;
                }
            }
            else
            {
                await Utilities.ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                        Constants.General.InvalidValueProvided,
                        userProfile,
                        turnContext,
                        cancellationToken);

                conversationData.SetWaitingForUserInputFlag(false);
            }
        }
        
        public override async Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await Utilities.ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                    Constants.ServiceFieldsMessages.ServiceDeliveryDateMessage,
                    userProfile,
                    turnContext,
                    cancellationToken);

            conversationData.SetWaitingForUserInputFlag();
        }
    }
}