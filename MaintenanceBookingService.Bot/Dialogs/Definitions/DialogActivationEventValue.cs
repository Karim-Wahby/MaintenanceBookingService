namespace MaintenanceBookingService.Bot.Dialogs.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DialogActivationEventValue
    {
        public string StateKey { get; set; }

        public string DialogName { get; set; }

        public DialogActivationEventValue(string stateKey, string dialogName)
        {
            this.DialogName = dialogName;
            this.StateKey = stateKey;
        }
    }
}
