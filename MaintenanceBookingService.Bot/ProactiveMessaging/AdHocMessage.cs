namespace MaintenanceBookingService.Bot.ProactiveMessaging
{
    using MaintenanceBookingService.Definitions;
    using MaintenanceBookingService.Bot.Models;
    using Microsoft.Bot.Connector;
    using Microsoft.Bot.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class AdHocMessage
    {
        public static async Task SendMessageAsync(ConversationChannelData userCommunicationChannelInfo, SupportedLanguage userPreferedLangue, Models.Message messageToSend, Models.MessageOption messgeOptions = null, bool startNewConversation = false)
        {
            var userAccount = new ChannelAccount(userCommunicationChannelInfo.UserId, userCommunicationChannelInfo.UserName);
            var botAccount = new ChannelAccount(userCommunicationChannelInfo.BotId, userCommunicationChannelInfo.BotName);
            var connector = new ConnectorClient(new Uri(userCommunicationChannelInfo.ServiceUrl));

            // Create a new message.
            IMessageActivity proactiveMessageActivity = Activity.CreateMessageActivity();
            if (!string.IsNullOrEmpty(userCommunicationChannelInfo.ConversationId) 
                && !string.IsNullOrEmpty(userCommunicationChannelInfo.ChannelId)
                && !startNewConversation)
            {
                // If conversation ID and channel ID was stored previously, use it.
                proactiveMessageActivity.ChannelId = userCommunicationChannelInfo.ChannelId;
            }
            else
            {
                // Conversation ID was not stored previously, so create a conversation. 
                // Note: If the user has an existing conversation in a channel, this will likely create a new conversation window.
                userCommunicationChannelInfo.ConversationId = (await connector.Conversations.CreateDirectConversationAsync(botAccount, userAccount)).Id;
            }

            // Set the address-related properties in the message and send the message.
            proactiveMessageActivity.From = botAccount;
            proactiveMessageActivity.Recipient = userAccount;
            proactiveMessageActivity.Conversation = new ConversationAccount(id: userCommunicationChannelInfo.ConversationId);
            InitializeMessage(proactiveMessageActivity, userPreferedLangue, messageToSend, messgeOptions);
            

            await connector.Conversations.SendToConversationAsync((Activity)proactiveMessageActivity);
        }

        private static void InitializeMessage(
            IMessageActivity proactiveMessageActivity, 
            SupportedLanguage userPreferedLangue,
            Message messageToSend, 
            MessageOption messgeFormattingOptions = null, 
            MessageOption ExtraMessageOptions = null)
        {
            proactiveMessageActivity.Text = GetMessageString(userPreferedLangue, messageToSend, messgeFormattingOptions);
            var messageOptions = GetMessageOptions(userPreferedLangue, messageToSend, ExtraMessageOptions);

            if (messageOptions != null && messageOptions.Any())
            {
                proactiveMessageActivity.SuggestedActions = new SuggestedActions()
                {
                    Actions = messageOptions
                    .Select(option => new CardAction() { Title = option, Type = ActionTypes.ImBack, Value = option })
                    .ToList()
                };
            }
        }

        private static IEnumerable<string> GetMessageOptions(SupportedLanguage userPreferedLangue, Message messageToSend, MessageOption extraMessageOptions)
        {
            MessageOption combinedOptions = MessageOption.CombineOptions(messageToSend?.Options, extraMessageOptions);
            switch (userPreferedLangue)
            {
                case SupportedLanguage.English:
                    return combinedOptions?.English;
                case SupportedLanguage.Arabic:
                    return combinedOptions?.Arabic;
                default:
                    return combinedOptions?.English;
            }
        }

        private static string GetMessageString(SupportedLanguage userPreferedLangue, Message messageToSend, MessageOption messgeOptions)
        {
            switch (userPreferedLangue)
            {
                case SupportedLanguage.English:
                    return GetMessageString(messageToSend.English, messgeOptions?.English);
                case SupportedLanguage.Arabic:
                    return GetMessageString(messageToSend.Arabic, messgeOptions?.Arabic);
                default:
                    return GetMessageString(messageToSend.English, messgeOptions?.English);
            }
        }

        private static string GetMessageString(string message, string[] messgeOptions)
        {
            if (messgeOptions != null && messgeOptions.Any())
            {
                return string.Format(message, messgeOptions);
            }
            else
            {
                return message;
            }
        }
    }
}
