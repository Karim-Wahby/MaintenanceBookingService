﻿namespace MaintenanceBookingService.Dialogs
{
    using System.Threading;
    using System.Threading.Tasks;
    using MaintenanceBookingService.Dialogs.Interfaces;
    using MaintenanceBookingService.Models;
    using Microsoft.Bot.Builder;

    public class GetHourDialog : IStatelessDialog
    {
        public GetHourDialog(ConversationData conversationData, UserData userProfile)
            : base(conversationData, userProfile)
        {
        }

        public override Task HandleIncomingUserResponseAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public override Task StartAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}