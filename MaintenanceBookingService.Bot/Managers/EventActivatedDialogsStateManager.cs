namespace MaintenanceBookingService.Bot.Managers
{
    using MaintenanceBookingService.Bot.Dialogs.Definitions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// this is an in-memory storage for the status of the 
    /// requests that await user feedback 
    /// this should be a database store,
    /// though to cut the cost in-memory storage would do.
    public static class EventActivatedDialogsStateManager
    {
        private static Dictionary<string, DialogStateData>
            bookingRequestStore;

        static EventActivatedDialogsStateManager()
        {
            bookingRequestStore =
                new Dictionary<string, DialogStateData>();
        }

        public static DialogStateData GetRequestStatus(string statusKey)
        {
            if (bookingRequestStore.ContainsKey(statusKey))
            {
                return bookingRequestStore[statusKey];
            }

            return null;
        }

        public static void AddRequestStatus(
            string statusKey,
            DialogStateData state)
        {
            if (bookingRequestStore.ContainsKey(statusKey))
            {
                bookingRequestStore[statusKey] = state;
            }
            else
            {
                bookingRequestStore.Add(statusKey, state);
            }
        }
    }
}
