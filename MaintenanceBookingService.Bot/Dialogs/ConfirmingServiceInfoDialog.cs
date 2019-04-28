using MaintenanceBookingService.Definitions;

namespace MaintenanceBookingService.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Definitions;
    using MaintenanceBookingService.Dialogs.Interfaces;
    using MaintenanceBookingService.Dialogs.Utilities;
    using MaintenanceBookingService.Models;
    using Microsoft.Bot.Builder;
    using Newtonsoft.Json;

    public class ConfirmingServiceInfoDialog : IStatelessDialog
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public ConfirmingServiceInfoDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        public override async Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var userInput = ConversationUtils.GetUserReply(turnContext);
            if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.ApprovalOptionValues))
            {
                if (await PostTheUserRequest())
                {
                    await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                        Constants.Confirmation.RequestSupmittedMessage,
                        this.userProfile,
                        turnContext,
                        cancellationToken
                        );
                }
                else
                {
                    await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                        Constants.Confirmation.FailedToSupmitRequestMessage,
                        this.userProfile,
                        turnContext,
                        cancellationToken
                        );
                }
            }
            else
            {
                this.conversationData.SetWaitingForUserInputFlag(false);
                if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.RequiredServicenAdjustmentOptionValues))
                {
                    this.conversationData.ServiceBookingForm.RequestedService = null;
                }
                else if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.ServiceDescriptionAdjustmentOptionValues))
                {
                    this.conversationData.ServiceBookingForm.RequiredServiceDescription = null;
                }
                else if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.AdressAdjustmentOptionValues))
                {
                    this.conversationData.ServiceBookingForm.DeliveryLocation = null;
                }
                else if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.YearAdjustmentOptionValues))
                {
                    this.conversationData.ServiceBookingForm.Year = null;
                }
                else if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.MonthAdjustmentOptionValues))
                {
                    this.conversationData.ServiceBookingForm.Month = null;
                }
                else if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.DayAdjustmentOptionValues))
                {
                    this.conversationData.ServiceBookingForm.Day = null;
                }
                else if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.HourAdjustmentOptionValues))
                {
                    this.conversationData.ServiceBookingForm.Hour = null;
                }
                else if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.MinuteAdjustmentOptionValues))
                {
                    this.conversationData.ServiceBookingForm.Minutes = null;
                }
                else if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.PartOfDayAdjustmentOptionValues))
                {
                    this.conversationData.ServiceBookingForm.DayOrNight = null;
                }
                else
                {
                    this.conversationData.SetWaitingForUserInputFlag();
                    await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                        Constants.General.InvalidValueProvided,
                        this.userProfile,
                        turnContext,
                        cancellationToken
                        );
                }
            }
        }

        private async Task<bool> PostTheUserRequest()
        {
            var userFilledFormValues = this.conversationData.ServiceBookingForm;
            var serviceBookingRequest = new BookingRequest(
                userFilledFormValues.RequestedService.Value,
                userFilledFormValues.RequiredServiceDescription,
                userFilledFormValues.DeliveryLocation,
                new DateTime(
                    userFilledFormValues.Year.Value,
                    userFilledFormValues.Month.Value,
                    userFilledFormValues.Day.Value,
                    userFilledFormValues.Hour.Value,
                    userFilledFormValues.Minutes.Value,
                    0),
                this.userProfile.Name,
                this.userProfile.Id,
                this.userProfile.ChannelId,
                this.conversationData.BotId);

            var stringContent = new StringContent(JsonConvert.SerializeObject(serviceBookingRequest), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("http://localhost:2614/api/MaintenanceServicesRequests/AddRequest/", stringContent);
            return await response.Content.ReadAsAsync<bool>();
        }

        public override async Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var formRepresentation = new string[]
                {
                    this.conversationData.ServiceBookingForm.ToLocalizedStrings(this.userProfile.PreferredLanguage.Value)
                };

            var formValuesStringRepresentation = new MessageOption()
            {
                English = formRepresentation,
                Arabic = formRepresentation
            };

            await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                        Constants.Confirmation.ConfirmationMessage,
                        this.userProfile,
                        turnContext,
                        cancellationToken,
                        formattingValues: formValuesStringRepresentation);

            conversationData.SetWaitingForUserInputFlag(true);
        }
    }
}