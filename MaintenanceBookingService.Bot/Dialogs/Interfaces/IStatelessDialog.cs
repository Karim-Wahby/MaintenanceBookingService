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
        public IStatelessDialog(ConversationData conversationData, UserData userProfile)
        {
            this.ConversationData = conversationData;
            this.UserProfile = userProfile;
        }

        public override abstract Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken);

        public override abstract Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken);
    }
}
