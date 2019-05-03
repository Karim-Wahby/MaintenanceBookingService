namespace MaintenanceBookingService.Bot.Dialogs.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Bot.Dialogs.Definitions;
    using MaintenanceBookingService.Bot.Managers;
    using MaintenanceBookingService.Bot.Models;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Schema;

    public abstract class IEventActivatedDialog : IStatelessDialog
    {
        public static string DialogName { get; protected set; }

        static IEventActivatedDialog()
        {
            DialogName = MethodBase.GetCurrentMethod().DeclaringType.Name;
        }

        public IEventActivatedDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        protected IEventActivatedDialog(DialogStateData storedDialogState)
            : base(storedDialogState?.ConversationData, storedDialogState?.UserProfile)
        {
        }

        public IEventActivatedDialog(string key, ConversationData oldConversationData, UserData oldUserProfile)
            : this(EventActivatedDialogsStateManager.GetRequestStatus(KeyAugmentationFunction(key)))
        {
            EventActivatedDialogsStateManager.AddRequestStatus(
                KeyAugmentationFunction(key),
                new DialogStateData(oldUserProfile, oldConversationData, null, null));
        }

        public abstract Task ProActiveMessageToUseAsync(ITurnContext turnContext, CancellationToken cancellationToken);

        public abstract Task<string> KeySelectionFunction(ITurnContext turnContext, CancellationToken cancellationToken);

        public override async Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            EventActivatedDialogsStateManager.AddRequestStatus(
                KeyAugmentationFunction(await this.KeySelectionFunction(turnContext, cancellationToken)),
                new DialogStateData(
                    this.UserProfile,
                    this.ConversationData,
                    turnContext.Adapter,
                    turnContext.Activity.GetConversationReference()));
            var msg = turnContext.Activity.CreateReply();
            msg.Type = ActivityTypes.EndOfConversation;
            msg.AsEndOfConversationActivity().Code = EndOfConversationCodes.CompletedSuccessfully;

            await turnContext.SendActivityAsync(msg, cancellationToken);
        }

        protected async Task RevertToNormalStateAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var oldState = EventActivatedDialogsStateManager.GetRequestStatus(
                KeyAugmentationFunction(await KeySelectionFunction(turnContext, cancellationToken)));

            this.ConversationData = oldState.ConversationData;
            this.UserProfile = oldState.UserProfile;
        }

        public static string KeyAugmentationFunction(string key)
        {
            return DialogName + " " + key;
        }
    }
}
