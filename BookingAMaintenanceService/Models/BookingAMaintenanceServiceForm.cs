using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAMaintenanceService.Models
{
    public class BookingAMaintenanceServiceForm
    {
        public SupportedMaintenanceServices? RequestedService { get; set; } = null;

        public string RequiredServiceDescription { get; set; } = null;

        public string DeliveryLocation { get; set; } = null;

        public DateTime? RequiredSerivceTime { get; set; } = null;

        public bool FailedToRecognizeProvidedDateTime { get; set; } = false;

        public bool SerivceIsRequiredASAP { get; set; } = false;

        public int? Day { get; set; } = null;

        public int? Month { get; set; } = null;

        public int? Year { get; set; } = null;

        public int? Minutes { get; set; } = null;

        public int? Hour { get; set; } = null;

        public string DayOrNight { get; set; } = null;
    }
}
