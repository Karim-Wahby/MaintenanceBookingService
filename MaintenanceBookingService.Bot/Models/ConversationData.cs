namespace MaintenanceBookingService.Bot.Models
{
    using Microsoft.Bot.Builder;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ConversationData
    {
        public ConversationData()
        {
        }

        public ConversationData(ConversationData other)
        {
            this.CurrentConversationIntent = other.CurrentConversationIntent;
            this.ServiceBookingForm = other.ServiceBookingForm;
            this.DialogsStatuses = other.DialogsStatuses;
            this.IsInProActiveDialog = other.IsInProActiveDialog;
            this.NewUserMaintenanceServiceId = other.NewUserMaintenanceServiceId;
            this.IsExpectingFeedBackFromUser = other.IsExpectingFeedBackFromUser;
            this.WaitingForUserInput = other.WaitingForUserInput;
            this.BotId = other.BotId;
            this.BotName = other.BotName;
            this.ServiceUrl = other.ServiceUrl;
            this.ConversationId = other.ConversationId;
        }

        public BotSupportedIntents? CurrentConversationIntent { get; set; } = null;

        public MaintenanceBookingServiceForm ServiceBookingForm { get; set; } = new MaintenanceBookingServiceForm();

        // Dialog name To Dialog State
        public Dictionary<string, string> DialogsStatuses { get; set; } = new Dictionary<string, string>();

        public bool IsInProActiveDialog { get; set; } = false;

        public string NewUserMaintenanceServiceId { get; set; } = string.Empty;

        public bool IsExpectingFeedBackFromUser { get; set; } = false;

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
