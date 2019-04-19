using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAMaintenaceService.Models
{
    public class ConversationData
    {
        // public ConversationPhases LastConversationState { get; set; } = ConversationPhases.GreetingTheUser;
        public BotSupportedIntents? CurrentConversationIntent { get; set; } = null;

        public bool WaitingForUserInput { get; set; } = false;

        public int Counter { get; set; } = 0;
    }
}
