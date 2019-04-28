﻿namespace MaintenanceBookingService.Models
{
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

        public bool WaitingForUserInput { get; set; } = false;

        public string BotId { get; set; } = string.Empty;

        public static void SetWaitingForUserInputFlag(ConversationData conversationData, bool value = true)
        {
            conversationData.WaitingForUserInput = value;
        }

        public void SetWaitingForUserInputFlag(bool value = true)
        {
            this.WaitingForUserInput = value;
        }
    }
}
