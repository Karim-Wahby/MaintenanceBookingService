namespace MaintenanceBookingService.Bot.Dialogs.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Bot.Models;
    using Microsoft.Bot.Builder;

    public abstract class IStatelessDialog : IDialog
    {
        protected ConversationData conversationData { get; set; }
        protected UserData userProfile;

        public IStatelessDialog(ConversationData conversationData, UserData userProfile)
        {
            this.conversationData = conversationData;
            this.userProfile = userProfile;
        }

        public abstract Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken);

        public abstract Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken);
    }
}
