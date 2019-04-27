using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaintenanceBookingService.Models
{
    public class UserData
    {
        public string Name { get; set; } = string.Empty;
        public SupportedLanguage? PreferredLanguage { get; set; } = null;

        // public string HomeAddress { get; set; } = string.Empty;
        // public string PhoneNumber { get; set; } = string.Empty;
    }
}
