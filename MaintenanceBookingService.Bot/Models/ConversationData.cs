namespace MaintenanceBookingService.Bot.Models
{
    using Microsoft.Bot.Builder;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ConversationData
    {
        public BotSupportedIntents? CurrentConversationIntent { get; set; } = null;

        public MaintenanceBookingServiceForm ServiceBookingForm { get; set; } = new MaintenanceBookingServiceForm();

        // Dialog name To Dialog State
        public Dictionary<string, string> DialogsStatuses { get; set; } = new Dictionary<string, string>();

        public string NewUserMaintenanceServiceId = string.Empty;

        public bool IsExpectingFeedBackFromUser = false;

        public bool WaitingForUserInput { get; set; } = false;

        public string BotId { get; set; } = string.Empty;

        public string BotName { get; set; } = string.Empty;

        public string ServiceUrl { get; set; } = string.Empty;

        public string ConversationId { get; set; } = string.Empty;

        public static void SetWaitingForUserInputFlag(ConversationData conversationData, bool value = true)
        {
            conversationData.WaitingForUserInput = value;
        }

        public void SetWaitingForUserInputFlag(bool value = true)
        {
            this.WaitingForUserInput = value;
        }

        public void InitializeConversationDataFromDialogContext(ITurnContext turnContext)
        {
            this.BotId = turnContext.Activity.Recipient.Id;
            this.BotName = turnContext.Activity.Recipient.Name;
            this.ServiceUrl = turnContext.Activity.ServiceUrl;
            this.ConversationId = turnContext.Activity.Conversation.Id;
        }
    }
}
