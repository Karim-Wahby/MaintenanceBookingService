namespace MaintenanceBookingService.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Definitions;
    using MaintenanceBookingService.Dialogs.Interfaces;
    using MaintenanceBookingService.Dialogs.Utilities;
    using MaintenanceBookingService.Models;
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
            var userInput = ConversationUtils.GetUserReply(turnContext);
            if (DialogUtils.IsUserInputInOptions(userInput, Constants.LanguageSelection.EnglishLanguagePossibleSelectionValues))
            {
                userProfile.PreferredLanguage = SupportedLanguage.English;
            }
            else if (DialogUtils.IsUserInputInOptions(userInput, Constants.LanguageSelection.ArabicLanguagePossibleSelectionValues))
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
