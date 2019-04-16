using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAMaintenaceService.Models
{
    public class ConversationData
    {
        public ConversationPhases ConversationState { get; set; } = ConversationPhases.GreetingTheUser;
        public int Counter { get; set; } = 0;
    }
}
