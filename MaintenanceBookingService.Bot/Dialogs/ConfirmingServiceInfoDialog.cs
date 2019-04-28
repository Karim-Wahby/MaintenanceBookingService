
namespace MaintenanceBookingService.Bot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Definitions;
    using MaintenanceBookingService.Bot.Dialogs.Interfaces;
    using MaintenanceBookingService.Bot.Dialogs.Utilities;
    using MaintenanceBookingService.Bot.Models;
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
                var newMaintenanceServiceRequestId = await PostMaintenanceServiceRequestForTheUser();
                if (!string.IsNullOrWhiteSpace(newMaintenanceServiceRequestId))
                {
                    var requestIdAsOption = new string[] { newMaintenanceServiceRequestId };
                    await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                        Constants.Confirmation.RequestSupmittedMessage,
                        this.userProfile,
                        turnContext,
                        cancellationToken,
                        formattingValues: new MessageOption()
                            {
                                English = requestIdAsOption,
                                Arabic  = requestIdAsOption
                            }
                        );

                    ClearServiceBookingForm();
                    ResetUserIntentFromDialog();
                    this.conversationData.NewUserMaintenanceServiceId = newMaintenanceServiceRequestId;
                    this.conversationData.IsExpectingFeedBackFromUser = true;
                    this.conversationData.SetWaitingForUserInputFlag(false);
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

        private void ResetUserIntentFromDialog()
        {
            this.conversationData.CurrentConversationIntent = null;
    }

        private void ClearServiceBookingForm()
        {
            this.conversationData.ServiceBookingForm = new MaintenanceBookingServiceForm();
        }

        private async Task<string> PostMaintenanceServiceRequestForTheUser()
        {
            var userFilledFormValues = this.conversationData.ServiceBookingForm;

            var userRequestedDelivaryDate = new System.DateTime(
                    userFilledFormValues.Year.Value,
                    userFilledFormValues.Month.Value,
                    userFilledFormValues.Day.Value,
                    userFilledFormValues.Hour.Value,
                    userFilledFormValues.Minutes.Value,
                    0);

            var conversationChannelData = new ConversationChannelData(
                this.userProfile.Id,
                this.userProfile.Name,
                this.conversationData.BotId,
                this.conversationData.BotName,
                this.userProfile.ChannelId,
                this.conversationData.ConversationId,
                this.conversationData.ServiceUrl);

            var serviceBookingRequest = new BookingRequest(
                userFilledFormValues.RequestedService.Value,
                userFilledFormValues.RequiredServiceDescription,
                userFilledFormValues.DeliveryLocation,
                userRequestedDelivaryDate,
                conversationChannelData,
                this.userProfile.PreferredLanguage.Value);

            var stringContent = new StringContent(JsonConvert.SerializeObject(serviceBookingRequest), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("http://localhost:2614/api/MaintenanceServicesRequests/AddRequest/", stringContent);
            return await response.Content.ReadAsAsync<string>();
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