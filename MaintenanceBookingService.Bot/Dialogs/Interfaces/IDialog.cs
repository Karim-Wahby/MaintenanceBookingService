namespace MaintenanceBookingService.Bot.Dialogs.Interfaces
{
    using MaintenanceBookingService.Bot.Models;
    using Microsoft.Bot.Builder;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class IDialog
    {
        public ConversationData ConversationData { get; set; }

        public UserData UserProfile { get; set; }

        public abstract Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken);

        public abstract Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken);
    }
}
