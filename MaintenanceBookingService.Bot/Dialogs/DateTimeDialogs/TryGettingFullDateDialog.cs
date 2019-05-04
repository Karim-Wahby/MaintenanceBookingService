namespace MaintenanceBookingService.Bot.Dialogs.DateTimeDialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Bot.Dialogs.Interfaces;
    using MaintenanceBookingService.Bot.Models;
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
            if (Utilities.DialogUtils.IsUserInputInOptions(userInput, Dialogs.Constants.ServiceFieldsMessages.TodayOptionValues))
            {
                userRequestedDate = DateTime.Now;
            }
            else if (Utilities.DialogUtils.IsUserInputInOptions(userInput, Dialogs.Constants.ServiceFieldsMessages.TomorrowOptionValues))
            {
                userRequestedDate = DateTime.Now.AddDays(1);
            }
            else if (!Utilities.DialogUtils.TryGetDateFromUserInput(userInput, out userRequestedDate))
            {
                ConversationData.ServiceBookingForm.FailedToRecognizeProvidedDate = true;
            }

            if (userRequestedDate.HasValue)
            {
                if (userRequestedDate < DateTime.Now.Date)
                {
                    await Utilities.ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                        Dialogs.Constants.ServiceFieldsMessages.DateInThePastErrorMessage,
                        UserProfile,
                        turnContext,
                        cancellationToken);
                }
                else
                {
                    ConversationData.ServiceBookingForm.FailedToRecognizeProvidedDate = true;
                    ConversationData.SetWaitingForUserInputFlag(false);
                    ConversationData.ServiceBookingForm.Day = userRequestedDate.Value.Day;
                    ConversationData.ServiceBookingForm.Month = userRequestedDate.Value.Month;
                    ConversationData.ServiceBookingForm.Year = userRequestedDate.Value.Year;
                }
            }
            else
            {
                await Utilities.ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                        Dialogs.Constants.General.InvalidValueProvided,
                        UserProfile,
                        turnContext,
                        cancellationToken);

                ConversationData.SetWaitingForUserInputFlag(false);
            }
        }
        
        public override async Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await Utilities.ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                    Dialogs.Constants.ServiceFieldsMessages.ServiceDeliveryDateMessage,
                    UserProfile,
                    turnContext,
                    cancellationToken);

            ConversationData.SetWaitingForUserInputFlag();
        }
    }
}