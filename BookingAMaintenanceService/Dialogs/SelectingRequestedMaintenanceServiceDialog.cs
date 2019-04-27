namespace BookingAMaintenanceService.Dialogs
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using BookingAMaintenanceService.Dialogs.Interfaces;
    using BookingAMaintenanceService.Dialogs.Utilities;
    using BookingAMaintenanceService.Models;
    using MaintenanceBookingService.Definitions;
    using Microsoft.Bot.Builder;

    public class SelectingRequestedMaintenanceServiceDialog : IStatelessDialog
    {
        public SelectingRequestedMaintenanceServiceDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        public override async Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var userInput = ConversationUtils.GetUserReply(turnContext);
            if (DialogUtils.IsUserInputInOptions(userInput, Constants.RequestedMaintenanceServiceSelection.CarpentryPossibleValues))
            {
                conversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.Carpentry;
            }
            else if (DialogUtils.IsUserInputInOptions(userInput, Constants.RequestedMaintenanceServiceSelection.ElectricalMaintenancePossibleValues))
            {
                conversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.ElectricalMaintenance;
            }
            else if (DialogUtils.IsUserInputInOptions(userInput, Constants.RequestedMaintenanceServiceSelection.PlumbingPossibleValues))
            {
                conversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.PlumbingServices;
            }
            else if (DialogUtils.IsUserInputInOptions(userInput, Constants.RequestedMaintenanceServiceSelection.AirConditioningPossibleValues))
            {
                conversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.AirConditioningMaintenance;
            }
            else if (DialogUtils.IsUserInputInOptions(userInput, Constants.RequestedMaintenanceServiceSelection.PaintingPossibleValues))
            {
                conversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.PaintingServices;
            }
            else if (DialogUtils.IsUserInputInOptions(userInput, Constants.RequestedMaintenanceServiceSelection.CleaningPossibleValues))
            {
                conversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.Cleaning;
            }
            else
            {
                await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                    Constants.General.InvalidValueProvided,
                    userProfile,
                    turnContext,
                    cancellationToken
                    );
            }

            if (conversationData.ServiceBookingForm.RequestedService.HasValue)
            {
                conversationData.SetWaitingForUserInputFlag(false);
            }
        }

        public override async Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                        Constants.RequestedMaintenanceServiceSelection.AskingForRequiredService,
                        userProfile,
                        turnContext,
                        cancellationToken);

            conversationData.SetWaitingForUserInputFlag();
        }
    }
}