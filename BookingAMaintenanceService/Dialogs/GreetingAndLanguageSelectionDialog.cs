namespace BookingAMaintenanceService.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using BookingAMaintenanceService.Dialogs.Interfaces;
    using BookingAMaintenanceService.Dialogs.Utilities;
    using BookingAMaintenanceService.Models;
    using Microsoft.Bot.Builder;

    public class GreetingAndLanguageSelectionDialog : IStatelessDialog
    {
        public GreetingAndLanguageSelectionDialog(ConversationData conversationData, UserData userProfile) : base(conversationData, userProfile)
        {
        }

        public override async Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await ConversationUtils.SendMessage(
                string.Format(Constants.General.Greetings.CombineLanguageValues(), this.userProfile.Name),
                turnContext,
                cancellationToken
                );

            await ConversationUtils.SendMessageBasedOnUserPreferredLanguage(
                Constants.LanguageSelection.AskingForpreferredLanguage,
                this.userProfile,
                turnContext,
                cancellationToken);

            conversationData.SetWaitingForUserInputFlag();
        }

        public override async Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var userInput = ConversationUtils.GetUserReply(turnContext)?.ToLower().Trim();
            if (Constants.LanguageSelection.EnglishLanguagePossibleSelectionValues.Contains(userInput))
            {
                userProfile.PreferredLanguage = SupportedLanguage.English;
            }
            else if (Constants.LanguageSelection.ArabicLanguagePossibleSelectionValues.Contains(userInput))
            {
                userProfile.PreferredLanguage = SupportedLanguage.Arabic;
            }
            else
            {
                await ConversationUtils.SendMessage(Constants.General.InvalidValueProvided.CombineLanguageValues(),
                        turnContext,
                        cancellationToken);
            }

            if (userProfile.PreferredLanguage.HasValue)
            {
                this.conversationData.SetWaitingForUserInputFlag(false);
            }
        }
    }
}
