namespace MaintenanceBookingService.Dialogs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Dialogs.Interfaces;
    using MaintenanceBookingService.Dialogs.Utilities;
    using MaintenanceBookingService.Models;
    using Microsoft.Bot.Builder;

    public class ConfirmingServiceInfoDialog : IStatelessDialog
    {
        public ConfirmingServiceInfoDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        public override async Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var userInput = ConversationUtils.GetUserReply(turnContext);
            if (DialogUtils.IsUserInputInOptions(userInput, Constants.Confirmation.ApprovalOptionValues))
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