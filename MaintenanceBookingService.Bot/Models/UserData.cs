using MaintenanceBookingService.Definitions;
using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaintenanceBookingService.Bot.Models
{
    public class UserData
    {
        public UserData()
        {
        }

        public UserData(UserData other)
        {
            this.Name = other.Name;
            this.Id = other.Id;
            this.ChannelId = other.ChannelId;
            this.PreferredLanguage = other.PreferredLanguage;
        }

        public string Name { get; set; } = string.Empty;

        public string Id { get; set; } = string.Empty;

        public string ChannelId { get; set; } = string.Empty;

        public SupportedLanguage? PreferredLanguage { get; set; } = null;

        // public string HomeAddress { get; set; } = string.Empty;
        // public string PhoneNumber { get; set; } = string.Empty;

        public void InitializeConversationDataFromDialogContext(ITurnContext turnContext)
        {
            this.Name = turnContext.Activity.From.Name;
            this.Id = turnContext.Activity.From.Id;
            this.ChannelId = turnContext.Activity.ChannelId;
        }
    }
}
