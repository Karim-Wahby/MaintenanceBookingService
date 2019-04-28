namespace MaintenanceBookingService.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ConversationChannelData
    {
        public string UserName { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string ChannelId { get; set; } = string.Empty;

        public string BotId { get; set; } = string.Empty;

        public string BotName { get; set; } = string.Empty;

        public string ServiceUrl { get; set; } = string.Empty;

        public string ConversationId { get; set; } = string.Empty;

        public ConversationChannelData()
        {
        }

        public ConversationChannelData(
            string userId,
            string userName,
            string botId,
            string botName,
            string channelId,
            string conversationId,
            string serviceUrl)
        {
            this.BotId = botId;
            this.BotName = botName;
            this.UserId = userId;
            this.UserName = userName;
            this.ChannelId = channelId;
            this.ConversationId = conversationId;
            this.ServiceUrl = serviceUrl;
        }
    }
}
