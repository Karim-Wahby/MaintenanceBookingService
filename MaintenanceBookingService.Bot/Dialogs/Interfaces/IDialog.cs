namespace MaintenanceBookingService.Dialogs.Interfaces
{
    using Microsoft.Bot.Builder;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IDialog
    {
        Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken);

        Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken);
    }
}
