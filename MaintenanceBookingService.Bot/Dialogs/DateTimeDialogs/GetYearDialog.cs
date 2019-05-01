namespace MaintenanceBookingService.Bot.Dialogs.DateTimeDialogs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Bot.Dialogs.Interfaces;
    using MaintenanceBookingService.Bot.Models;
    using Microsoft.Bot.Builder;

    public class GetYearDialog : IStatelessDialog
    {
        public GetYearDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        public override async Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var userInput = Utilities.ConversationUtils.GetUserReply(turnContext);
            int userRequiredYear;
            if (int.TryParse(userInput, out userRequiredYear))
            {
                if (DateTime.Now.Year <= userRequiredYear)
                {
                    this.ConversationData.ServiceBookingForm.Year = userRequiredYear;
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
            string[] dayMessageExtraOptions = GetYearOptions();

            await Utilities.ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                Constants.ServiceFieldsMessages.ServiceDeliveryYearMessage,
                this.UserProfile,
                turnContext,
                cancellationToken,
                new MessageOption()
                {
                    English = dayMessageExtraOptions,
                    Arabic = dayMessageExtraOptions
                });
        }

        private string[] GetYearOptions()
        {
            var todayDate = DateTime.Now;
            return new string[] { todayDate.Year.ToString(), (todayDate.Year + 1).ToString(), (todayDate.Year + 2).ToString() };
        }
    }
}