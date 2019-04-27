namespace BookingAMaintenanceService.Dialogs.Constants
{
    using BookingAMaintenanceService.Dialogs.Definitions;
    using BookingAMaintenanceService.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class ServiceFieldsMessages
    {
        public static Message ServiceDescribtionMessage = new Message()
        {
            English = $"Please Describe the requested service",
            Arabic = $"فضلا, قم بوصف الخدمه المطلوبه",
        };

        public static Message ServiceDeliveryLocationMessage = new Message()
        {
            English = $"Please enter the address to deliver the serive to",
            Arabic = $"اكتب العنوان لتوصيل الخدمه",
        };

        public static Message ServiceDeliveryDateMessage = new Message()
        {
            English = $"Please enter the Date to deliver the serive (Day/Month/Year)",
            Arabic = $"اكتب تاريخ تسلم الخدمه(يوم/شهر/سنه) بالارقم رجاء",
            Options = new MessageOption()
            {
                Arabic = new string[] { "اليوم", "غدا" },
                English = new string[] { "Today", "Tomorrow" }
            }
        };

        public static Message DateInThePastErrorMessage = new Message()
        {
            English = $"Please enter a Date That is In the Future as the Date to deliver the serive (Day/Month/Year)",
            Arabic = $"اكتب تاريخ فى المستقبل  لتسلم الخدمه(يوم/شهر/سنه) بالارقم رجاء",
        };

        public static Message ServiceDeliveryTimeMessage = new Message()
        {
            English = $"Please enter the time to deliver the serive (Hour:Minute PM/AM)",
            Arabic = $"اكتب تاريخ تسلم الخدمه(Hour:Minute PM/AM) بالارقم رجاء",
            Options = new MessageOption()
            {
                Arabic = new string[] { "فى اسرع وقت" },
                English = new string[] { "As Soon As Possible" }
            }
        };

        public static HashSet<string> TomorrowOptionValues = new HashSet<string>() { "غدا", "tomorrow" };
        public static HashSet<string> TodayOptionValues = new HashSet<string>() { "today" , "اليوم" };
        public static HashSet<string> AsSoonAsPossibleOptionValues = new HashSet<string>() { "as soon as possible", "فى اسرع وقت" };

        public static Message ServiceDeliveryDayMessage = new Message()
        {
            English =$"Please enter the Day to deliver the serive",
            Arabic = $"اكتب يوم تسلم الخدمه",
        };

    }
}
