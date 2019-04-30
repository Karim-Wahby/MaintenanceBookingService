namespace MaintenanceBookingService.Bot.Dialogs.Definitions
{
    using MaintenanceBookingService.Bot.Models;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DialogStateData
    {
        public ConversationReference ConversationReference { get; set; }

        public ConversationData ConversationData { get; set; }

        public UserData UserProfile { get; set; }

        public BotAdapter BotAdapter { get; set; }

        public DialogStateData(
            UserData userProfile, 
            ConversationData conversationData,
            BotAdapter botAdapter,
            ConversationReference conversationReference)
        {
            this.BotAdapter = botAdapter;
            this.ConversationData = new ConversationData(conversationData);
            this.ConversationReference = conversationReference;
            this.UserProfile = new UserData(userProfile);
        }
    }
}
