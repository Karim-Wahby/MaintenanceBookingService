namespace MaintenanceBookingService.Models
{
    using MaintenanceBookingService.Definitions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MaintenanceBookingServiceForm
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

        public string ToLocalizedStrings(SupportedLanguage preferredLanguage)
        {
            switch (preferredLanguage)
            {
                case SupportedLanguage.English:
                    return $"Requested Service: {this.RequestedServiceToLocalizedString(preferredLanguage)}{Environment.NewLine}" +
                    $"Service Description: {this.RequiredServiceDescription}{Environment.NewLine}" +
                    $"Address: {this.DeliveryLocation}{Environment.NewLine}" +
                    $"Day: {this.Day.Value.ToString()}{Environment.NewLine}" +
                    $"Month: {this.Month.Value.ToString()}{Environment.NewLine}" +
                    $"Year: {this.Year.Value.ToString()}{Environment.NewLine}" +
                    $"Hour: {this.Hour.Value.ToString()}{Environment.NewLine}" +
                    $"Minute: {this.Minutes.Value.ToString()}{Environment.NewLine}" +
                    $"PM/AM: {this.DayOrNight}";
                case SupportedLanguage.Arabic:
                    var dayOrNight = this.DayOrNight == "PM" ? "مساء" : "صباحا";
                    return $"الخدمه المطلوبه: {this.RequestedServiceToLocalizedString(preferredLanguage)}{Environment.NewLine}" +
                    $"وصف الخدمه: {this.RequiredServiceDescription}{Environment.NewLine}" +
                    $"عنوان توصيل الخدمه: {this.DeliveryLocation}{Environment.NewLine}" +
                    $"يوم استلام الخدمه: {this.Day.Value.ToString()}{Environment.NewLine}" +
                    $"شهر استلام الخدمه: {this.Month.Value.ToString()}{Environment.NewLine}" +
                    $"سنه استلام الخدمه: {this.Year.Value.ToString()}{Environment.NewLine}" +
                    $"ساعه استلام الخدمه: {this.Hour.Value.ToString()}{Environment.NewLine}" +
                    $"دقيقه استلام الخدمه: {this.Minutes.Value.ToString()}{Environment.NewLine}" +
                    $"صباحا/مساء: {dayOrNight}";
            }

            throw new ArgumentException("Unknown language Requested", nameof(preferredLanguage));
        }

        private string RequestedServiceToLocalizedString(SupportedLanguage preferredLanguage)
        {
            if (preferredLanguage == SupportedLanguage.Arabic)
            {
                switch (this.RequestedService.Value)
                {
                    case SupportedMaintenanceServices.Carpentry:
                        return "اعمال خشبيه";
                    case SupportedMaintenanceServices.ElectricalMaintenance:
                        return "كهرباء";
                    case SupportedMaintenanceServices.PlumbingServices:
                        return "سباكه";
                    case SupportedMaintenanceServices.AirConditioningMaintenance:
                        return "صيانه مكيف الهواء";
                    case SupportedMaintenanceServices.Cleaning:
                        return "خدمات نقاشه";
                    case SupportedMaintenanceServices.PaintingServices:
                        return "تنظيف";
                }
            }
            else
            {
                switch (this.RequestedService.Value)
                {
                    case SupportedMaintenanceServices.Carpentry:
                        return "Carpentry";
                    case SupportedMaintenanceServices.ElectricalMaintenance:
                        return "Electrical maintenance";
                    case SupportedMaintenanceServices.PlumbingServices:
                        return "Plumbing";
                    case SupportedMaintenanceServices.AirConditioningMaintenance:
                        return "Air conditioning";
                    case SupportedMaintenanceServices.Cleaning:
                        return "Painting";
                    case SupportedMaintenanceServices.PaintingServices:
                        return "Cleaning";
                    default:
                        break;
                }
            }

            throw new ArgumentException("Unknown Requested Service value", nameof(RequestedService));
        }
    }
}
