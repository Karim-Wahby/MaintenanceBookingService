﻿namespace BookingAMaintenanceService.Dialogs.Utilities
{
    using BookingAMaintenanceService.Dialogs.Definitions;
    using BookingAMaintenanceService.Models;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public static class ConversationUtils
    {
        public static async Task SendMessageWithOptions(string message, ITurnContext turnContext, CancellationToken cancellationToken, params string[] options)
        {
            var reply = turnContext.Activity.CreateReply(message);
            reply.Type = ActivityTypes.Message;
            reply.TextFormat = TextFormatTypes.Plain;
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = options
                .Select(option => new CardAction() { Title = option, Type = ActionTypes.ImBack, Value = option })
                .ToList()
            };

            await turnContext.SendActivityAsync(reply, cancellationToken: cancellationToken);
        }

        public static async Task SendMessage(string message, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync(message, cancellationToken: cancellationToken);

        }

        public static async Task SendMessageBasedOnUserPreferredLanguage(
            Message message,
            UserData userProfile,
            ITurnContext turnContext,
            CancellationToken cancellationToken,
            MessageOption extraOptions = null)
        {
            var allOptions = MessageOption.CombineOptions(message.Options, extraOptions);
            if (message.IsLanguageIndependent || userProfile.PreferredLanguage == null)
            {
                var options = allOptions?.CombineLanguagesOptions();

                if (options != null && options.Count() > 0)
                {
                    await SendMessageWithOptions(message.English, turnContext, cancellationToken, options);
                }
                else
                {
                    await SendMessage(message.English, turnContext, cancellationToken);
                }
            }
            else if (userProfile.PreferredLanguage.Value == SupportedLanguage.Arabic)
            {
                if (allOptions?.Arabic != null && allOptions.Arabic.Count() > 0)
                {
                    await SendMessageWithOptions(message.Arabic, turnContext, cancellationToken, allOptions.Arabic);
                }
                else
                {
                    await SendMessage(message.Arabic, turnContext, cancellationToken);
                }
            }
            else if (userProfile.PreferredLanguage.Value == SupportedLanguage.English)
            {
                if (allOptions?.English != null && allOptions.English.Count() > 0)
                {
                    await SendMessageWithOptions(message.English, turnContext, cancellationToken, allOptions.English);
                }
                else
                {
                    await SendMessage(message.English, turnContext, cancellationToken);
                }
            }
            else
            {
                // TODO:- Telemtry for invalid state
            }
        }
        
        public static string GetUserReply(ITurnContext turnContext)
        {
            return turnContext.Activity.Text?.Trim().ToLower();
        }
    }
}
