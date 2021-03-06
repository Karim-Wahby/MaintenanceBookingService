﻿namespace MaintenanceBookingService.Bot.Dialogs.DateTimeDialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Bot.Dialogs.Interfaces;
    using MaintenanceBookingService.Bot.Models;
    using Microsoft.Bot.Builder;

    public class GetDayDialog : IStatelessDialog
    {
        public GetDayDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        public override async Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var userInput = Utilities.ConversationUtils.GetUserReply(turnContext);
            int userRequiredDay;
            if (int.TryParse(userInput, out userRequiredDay) &&
                userRequiredDay <= DateTime.DaysInMonth(this.ConversationData.ServiceBookingForm.Year.Value, this.ConversationData.ServiceBookingForm.Month.Value) &&
                userRequiredDay > 0)
            {
                var currentDate = DateTime.Now;

                if (currentDate.Year < this.ConversationData.ServiceBookingForm.Year ||
                    currentDate.Month < this.ConversationData.ServiceBookingForm.Month ||
                    currentDate.Day <= userRequiredDay)
                {
                    this.ConversationData.ServiceBookingForm.Day = userRequiredDay;
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
            string[] dayMessageExtraOptions = GetDayOptions();
            
            await Utilities.ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                Constants.ServiceFieldsMessages.ServiceDeliveryDayMessage,
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

        private string[] GetDayOptions()
        {
            var todayDate = DateTime.Now;
            var userFormValues = this.ConversationData.ServiceBookingForm;
            var daysInMonth = DateTime.DaysInMonth(userFormValues.Year.Value, userFormValues.Month.Value);
            if (todayDate.Year < userFormValues.Year.Value || todayDate.Month < userFormValues.Month.Value)
            {
                return Enumerable.Range(1, daysInMonth).Select(dayNumber => dayNumber.ToString()).ToArray();
            }
            else
            {
                return Enumerable.Range(1, daysInMonth).Where(day => day >= todayDate.Day)
                    .Select(dayNumber => dayNumber.ToString()).ToArray();
            }
        }
    }
}