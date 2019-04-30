namespace MaintenanceBookingService.Bot.Dialogs
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Bot.Dialogs.Interfaces;
    using MaintenanceBookingService.Bot.Dialogs.Utilities;
    using MaintenanceBookingService.Bot.Models;
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
                ConversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.Carpentry;
            }
            else if (DialogUtils.IsUserInputInOptions(userInput, Constants.RequestedMaintenanceServiceSelection.ElectricalMaintenancePossibleValues))
            {
                ConversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.ElectricalMaintenance;
            }
            else if (DialogUtils.IsUserInputInOptions(userInput, Constants.RequestedMaintenanceServiceSelection.PlumbingPossibleValues))
            {
                ConversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.PlumbingServices;
            }
            else if (DialogUtils.IsUserInputInOptions(userInput, Constants.RequestedMaintenanceServiceSelection.AirConditioningPossibleValues))
            {
                ConversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.AirConditioningMaintenance;
            }
            else if (DialogUtils.IsUserInputInOptions(userInput, Constants.RequestedMaintenanceServiceSelection.PaintingPossibleValues))
            {
                ConversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.PaintingServices;
            }
            else if (DialogUtils.IsUserInputInOptions(userInput, Constants.RequestedMaintenanceServiceSelection.CleaningPossibleValues))
            {
                ConversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.Cleaning;
            }
            else
            {
                await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                    Constants.General.InvalidValueProvided,
                    UserProfile,
                    turnContext,
                    cancellationToken
                    );
            }

            if (ConversationData.ServiceBookingForm.RequestedService.HasValue)
            {
                ConversationData.SetWaitingForUserInputFlag(false);
            }
        }

        public override async Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                        Constants.RequestedMaintenanceServiceSelection.AskingForRequiredService,
                        UserProfile,
                        turnContext,
                        cancellationToken);

            ConversationData.SetWaitingForUserInputFlag();
        }
    }
}