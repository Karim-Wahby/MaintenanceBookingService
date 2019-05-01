namespace MaintenanceBookingService.Bot.Dialogs.DateTimeDialogs
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Bot.Dialogs.Interfaces;
    using MaintenanceBookingService.Bot.Models;
    using Microsoft.Bot.Builder;

    public class GetMonthDialog : IStatelessDialog
    {
        public GetMonthDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        public override async Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var userInput = Utilities.ConversationUtils.GetUserReply(turnContext);
            int userRequiredMonth;
            if (int.TryParse(userInput, out userRequiredMonth) && userRequiredMonth < 13 && userRequiredMonth > 0)
            {
                var currentDate = DateTime.Now;

                if (currentDate.Year < this.ConversationData.ServiceBookingForm.Year || currentDate.Month <= userRequiredMonth)
                {
                    this.ConversationData.ServiceBookingForm.Month = userRequiredMonth;
                    this.ConversationData.SetWaitingForUserInputFlag(false);
                }
                else
                {
                    await Utilities.ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                        Dialogs.Constants.ServiceFieldsMessages.DateInThePastErrorMessage,
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
            string[] dayMessageExtraOptions = GetMonthOptions();

            await Utilities.ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                Constants.ServiceFieldsMessages.ServiceDeliveryMonthMessage,
                this.UserProfile,
                turnContext,
                cancellationToken,
                new MessageOption()
                {
                    English = dayMessageExtraOptions,
                    Arabic = dayMessageExtraOptions
                });

            this.ConversationData.SetWaitingForUserInputFlag();
        }

        private string[] GetMonthOptions()
        {
            var todayDate = DateTime.Now;
            var MonthOptions = new List<string>(3) { todayDate.Month.ToString() };
            if (todayDate.Month + 1 < 13)
            {
                MonthOptions.Add((todayDate.Month + 1).ToString());
            }

            if (todayDate.Month + 2 < 13)
            {
                MonthOptions.Add((todayDate.Month + 2).ToString());
            }

            return MonthOptions.ToArray();
        }
    }
}