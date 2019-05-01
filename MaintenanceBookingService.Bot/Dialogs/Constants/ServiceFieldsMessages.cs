namespace MaintenanceBookingService.Bot.Dialogs.Constants
{
    using MaintenanceBookingService.Bot.Dialogs.Definitions;
    using MaintenanceBookingService.Bot.Models;
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

        public static Message ServiceDeliveryYearMessage = new Message()
        {
            English = $"Please enter the Year you wish to be delivered the serive in",
            Arabic = $"اكتب سنه تسلم الخدمه بالارقم رجاء",
        };

        public static Message ServiceDeliveryMonthMessage = new Message()
        {
            English = $"Please enter the Month you wish to be delivered the serive in",
            Arabic = $"اكتب شهر تسلم الخدمه بالارقم رجاء",
        };

        public static Message ServiceDeliveryDayMessage = new Message()
        {
            English = $"Please enter the day you wish to be delivered the serive in",
            Arabic = $"اكتب يوم تسلم الخدمه بالارقم رجاء",
        };

        public static Message ServiceDeliveryHourMessage = new Message()
        {
            English = $"Please enter the hour you wish to be delivered the serive in",
            Arabic = $"اكتب ساعه تسلم الخدمه بالارقم رجاء",
        };

        public static string[] MinuteDelivaryTimes = new string[] { "15", "20", "30", "40", "45", "5", "10", "25", "35", "50", "55" };
        public static Message ServiceDeliveryMinuteMessage = new Message()
        {
            English = $"Please enter the minute you wish to be delivered the serive in",
            Arabic = $"اكتب دقيقه تسلم الخدمه بالارقم رجاء",
            Options = new MessageOption
            {
                English = MinuteDelivaryTimes,
                Arabic = MinuteDelivaryTimes
            }
        };

        public static Message ServiceDeliveryPartOfDayMessage = new Message()
        {
            English = $"Please Select the part of day (PM/AM) to deliver the serive",
            Arabic = $"اختر جزى من اليوم (صباحا/مساء) لتسلم الخدمه",
            Options = new MessageOption()
            {
                English = new string[] { "PM", "AM" },
                Arabic = new string[] { "صباحا", "مساء" }
            }
        };

        public static Message DateInThePastErrorMessage = new Message()
        {
            English = $"Please enter a Date That is In the Future as the Date to deliver the serive",
            Arabic = $"اكتب تاريخ فى المستقبل  لتسلم الخدمه بالارقم رجاء",
        };

        public static Message TimeInThePastErrorMessage = new Message()
        {
            English = $"Please enter a Time That is In the Future as the Date to deliver the serive, and It Needs to be at least 1 hour from now",
            Arabic = $"اكتب ميعاد فى المستقبل  لتسلم الخدمه بالارقم رجاء, و ايضا الميعاد لابد ان يكون بعد ساعه على الاقل",
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

        public static HashSet<string> PMOptionValues = new HashSet<string>() { "am", "صباحا" };
        public static HashSet<string> AMOptionValues = new HashSet<string>() { "pm", "مساء" };
        public static HashSet<string> TomorrowOptionValues = new HashSet<string>() { "غدا", "tomorrow" };
        public static HashSet<string> TodayOptionValues = new HashSet<string>() { "today" , "اليوم" };
        public static HashSet<string> AsSoonAsPossibleOptionValues = new HashSet<string>() { "as soon as possible", "فى اسرع وقت" };
    }
}