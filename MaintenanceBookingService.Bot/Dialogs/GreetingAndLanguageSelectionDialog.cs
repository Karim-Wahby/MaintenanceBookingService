namespace MaintenanceBookingService.Bot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Definitions;
    using MaintenanceBookingService.Bot.Dialogs.Interfaces;
    using MaintenanceBookingService.Bot.Dialogs.Utilities;
    using MaintenanceBookingService.Bot.Models;
    using Microsoft.Bot.Builder;

    public class GreetingAndLanguageSelectionDialog : IStatelessDialog
    {
        public GreetingAndLanguageSelectionDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        public override async Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await ConversationUtils.SendMessage(
                string.Format(Constants.General.Greetings.CombineLanguageValues(), this.UserProfile.Name),
                turnContext,
                cancellationToken
                );

            await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                Constants.LanguageSelection.AskingForpreferredLanguage,
                this.UserProfile,
                turnContext,
                cancellationToken);

            ConversationData.SetWaitingForUserInputFlag();
        }

        public override async Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var userInput = ConversationUtils.GetUserReply(turnContext);
            if (DialogUtils.IsUserInputInOptions(userInput, Constants.LanguageSelection.EnglishLanguagePossibleSelectionValues))
            {
                UserProfile.PreferredLanguage = SupportedLanguage.English;
            }
            else if (DialogUtils.IsUserInputInOptions(userInput, Constants.LanguageSelection.ArabicLanguagePossibleSelectionValues))
            {
                UserProfile.PreferredLanguage = SupportedLanguage.Arabic;
            }
            else
            {
                await ConversationUtils.SendMessage(Constants.General.InvalidValueProvided.CombineLanguageValues(),
                        turnContext,
                        cancellationToken);
            }

            if (UserProfile.PreferredLanguage.HasValue)
            {
                this.ConversationData.SetWaitingForUserInputFlag(false);
            }
        }
    }
}
