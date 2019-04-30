
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
                        this.UserProfile,
                        turnContext,
                        cancellationToken,
                        formattingValues: new MessageOption()
                            {
                                English = requestIdAsOption,
                                Arabic  = requestIdAsOption
                            }
                        );

                    this.ConversationData.NewUserMaintenanceServiceId = newMaintenanceServiceRequestId;
                    this.ConversationData.IsExpectingFeedBackFromUser = true;
                    await new GettingUserFeedBackDialog(this.ConversationData, this.UserProfile).StartAsync(turnContext, cancellationToken);

                    ClearServiceBookingForm();
                    ResetUserIntentFromDialog();
                    this.ConversationData.NewUserMaintenanceServiceId = newMaintenanceServiceRequestId;
                    this.ConversationData.SetWaitingForUserInputFlag(false);
                }
                else
                {
                    await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                        Constants.Confirmation.FailedToSupmitRequestMessage,
                        this.UserProfile,
                        turnContext,
                        cancellationToken
                        );
                }
            }
            else
            {
                this.ConversationData.SetWaitingForUserInputFlag(false);
                if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.RequiredServicenAdjustmentOptionValues))
                {
                    this.ConversationData.ServiceBookingForm.RequestedService = null;
                }
                else if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.ServiceDescriptionAdjustmentOptionValues))
                {
                    this.ConversationData.ServiceBookingForm.RequiredServiceDescription = null;
                }
                else if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.AdressAdjustmentOptionValues))
                {
                    this.ConversationData.ServiceBookingForm.DeliveryLocation = null;
                }
                else if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.YearAdjustmentOptionValues))
                {
                    this.ConversationData.ServiceBookingForm.Year = null;
                }
                else if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.MonthAdjustmentOptionValues))
                {
                    this.ConversationData.ServiceBookingForm.Month = null;
                }
                else if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.DayAdjustmentOptionValues))
                {
                    this.ConversationData.ServiceBookingForm.Day = null;
                }
                else if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.HourAdjustmentOptionValues))
                {
                    this.ConversationData.ServiceBookingForm.Hour = null;
                }
                else if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.MinuteAdjustmentOptionValues))
                {
                    this.ConversationData.ServiceBookingForm.Minutes = null;
                }
                else if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.PartOfDayAdjustmentOptionValues))
                {
                    this.ConversationData.ServiceBookingForm.DayOrNight = null;
                }
                else
                {
                    this.ConversationData.SetWaitingForUserInputFlag();
                    await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                        Constants.General.InvalidValueProvided,
                        this.UserProfile,
                        turnContext,
                        cancellationToken
                        );
                }
            }
        }

        private void ResetUserIntentFromDialog()
        {
            this.ConversationData.CurrentConversationIntent = null;
    }

        private void ClearServiceBookingForm()
        {
            this.ConversationData.ServiceBookingForm = new MaintenanceBookingServiceForm();
            this.ConversationData.IsExpectingFeedBackFromUser = false;
            this.ConversationData.NewUserMaintenanceServiceId = string.Empty;
        }

        private async Task<string> PostMaintenanceServiceRequestForTheUser()
        {
            var userFilledFormValues = this.ConversationData.ServiceBookingForm;

            var userRequestedDelivaryDate = new System.DateTime(
                    userFilledFormValues.Year.Value,
                    userFilledFormValues.Month.Value,
                    userFilledFormValues.Day.Value,
                    userFilledFormValues.Hour.Value,
                    userFilledFormValues.Minutes.Value,
                    0);

            var conversationChannelData = new ConversationChannelData(
                this.UserProfile.Id,
                this.UserProfile.Name,
                this.ConversationData.BotId,
                this.ConversationData.BotName,
                this.UserProfile.ChannelId,
                this.ConversationData.ConversationId,
                this.ConversationData.ServiceUrl);

            var serviceBookingRequest = new BookingRequest(
                userFilledFormValues.RequestedService.Value,
                userFilledFormValues.RequiredServiceDescription,
                userFilledFormValues.DeliveryLocation,
                userRequestedDelivaryDate,
                conversationChannelData,
                this.UserProfile.PreferredLanguage.Value);

            var stringContent = new StringContent(JsonConvert.SerializeObject(serviceBookingRequest), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("http://localhost:2614/api/MaintenanceServicesRequests/AddRequest/", stringContent);
            return await response.Content.ReadAsAsync<string>();
        }

        public override async Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var formRepresentation = new string[]
                {
                    this.ConversationData.ServiceBookingForm.ToLocalizedStrings(this.UserProfile.PreferredLanguage.Value)
                };

            var formValuesStringRepresentation = new MessageOption()
            {
                English = formRepresentation,
                Arabic = formRepresentation
            };

            await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                        Constants.Confirmation.ConfirmationMessage,
                        this.UserProfile,
                        turnContext,
                        cancellationToken,
                        formattingValues: formValuesStringRepresentation);

            ConversationData.SetWaitingForUserInputFlag(true);
        }
    }
}