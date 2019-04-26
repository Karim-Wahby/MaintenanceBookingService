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
        
        public int? Day { get; set; } = null;

        public int? Month { get; set; } = null;

        public int? Year { get; set; } = null;

        public int? Minutes { get; set; } = null;

        public int? Hour { get; set; } = null;

        public string DayOrNight { get; set; } = null;

        public bool FailedToRecognizeProvidedDate { get; set; } = false;

        public bool FailedToRecognizeProvidedTime { get; set; } = false;

        public bool IsDateSet
        {
            get
            {
                return this.Day.HasValue && this.Month.HasValue && this.Year.HasValue;
            }
        }

        public bool IsTimeSet
        {
            get
            {
                return this.Hour.HasValue && this.Minutes.HasValue && !string.IsNullOrWhiteSpace(this.DayOrNight);
            }
        }

        public bool IsServiceDeliveryTimeSet
        {
            get
            {
                return this.IsDateSet && this.IsTimeSet;
            }
        }
    }
}
