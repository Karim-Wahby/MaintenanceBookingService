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

    public class GetDayDialog : IStatelessDialog
    {
        public GetDayDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        public override Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public override async Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            string[] dayMessageExtraOptions = GetDayOptions();
            
            await Utilities.ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                Constants.ServiceFieldsMessages.ServiceDeliveryDayMessage,
                this.userProfile,
                turnContext,
                cancellationToken,
                new MessageOption()
                {
                    English = dayMessageExtraOptions,
                    Arabic = dayMessageExtraOptions
                });
        }

        private string[] GetDayOptions()
        {
            var todayDate = DateTime.Now;
            var userFormValues = this.conversationData.ServiceBookingForm;
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