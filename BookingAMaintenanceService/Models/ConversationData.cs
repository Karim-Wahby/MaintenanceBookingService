using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAMaintenanceService.Models
{
    public class ConversationData
    {
        public BotSupportedIntents? CurrentConversationIntent { get; set; } = null;

        public BookingAMaintenanceServiceForm ServiceBookingForm { get; set; } = null;

        public bool WaitingForUserInput { get; set; } = false;

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
