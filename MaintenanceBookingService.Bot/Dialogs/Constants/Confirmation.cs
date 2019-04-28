namespace MaintenanceBookingService.Dialogs.Constants
{
    using MaintenanceBookingService.Dialogs.Definitions;
    using MaintenanceBookingService.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class Confirmation
    {
        public static Message ConfirmationMessage = new Message()
        {
            English = $"Please Confirm your Provided Information:- {Environment.NewLine}{{0}}",
            Arabic = $"فضلا قم بالتاكد من القيم الخاصه بطلبكم:- {Environment.NewLine}{{0}}",
            Options = new MessageOption()
            {
                English = new string[] 
                {
                    "Looks Good!",
                    "Adjust Requested Service",
                    "Adjust Service Description",
                    "Adjust Address",
                    "Adjust Day",
                    "Adjust Month",
                    "Adjust Year",
                    "Adjust Hour",
                    "Adjust Minute",
                    "Adjust PM/AM"
                },
                Arabic = new string[]
                {
                    "يبدو جيدا!",
                    "تعديل الخدمه المطلوبه",
                    "تعديل وصف الخدمه",
                    "تعديل عنوان توصيل الخدمه",
                    "تعديل يوم استلام الخدمه",
                    "تعديل شهر استلام الخدمه",
                    "تعديل سنه استلام الخدمه",
                    "تعديل ساعه استلام الخدمه",
                    "تعديل دقيقه استلام الخدمه",
                    "تعديل صباحا/مساء"
                }
            }
        };

        public static Message RequestSupmittedMessage = new Message()
        {
            English = $"Your Service Request has been submitted, we will contact you soon",
            Arabic = $"لقد تم تسلم تسلم طلبكم بنجاح, سنقوم بالتواصل معكم قريبا",
        };

        public static Message FailedToSupmitRequestMessage = new Message()
        {
            English = $"Sorry, We failed to submit Your Service Request, There Seems to be a problem, please try again later",
            Arabic = $"نتاسف, يبدو ان هناك خطا ما حدث اثناء تقديم طلبكم رجاء حاول مره اخرى",
        };

        public static HashSet<string> ApprovalOptionValues = new HashSet<string>() { "looks good!", "يبدو جيدا!" };
        public static HashSet<string> RequiredServicenAdjustmentOptionValues = new HashSet<string>() { "تعديل الخدمه المطلوبه", "adjust requested service" };
        public static HashSet<string> ServiceDescriptionAdjustmentOptionValues = new HashSet<string>() { "تعديل وصف الخدمه", "adjust service description" };
        public static HashSet<string> AdressAdjustmentOptionValues = new HashSet<string>() { "تعديل وصف الخدمه", "adjust address" };
        public static HashSet<string> DayAdjustmentOptionValues = new HashSet<string>() { "تعديل يوم استلام الخدمه", "Adjust Day" };
        public static HashSet<string> MonthAdjustmentOptionValues = new HashSet<string>() {"تعديل شهر استلام الخدمه" , "Adjust Month" };
        public static HashSet<string> YearAdjustmentOptionValues = new HashSet<string>() {"تعديل سنه استلام الخدمه", "Adjust Year" };
        public static HashSet<string> HourAdjustmentOptionValues = new HashSet<string>() {"تعديل ساعه استلام الخدمه", "Adjust Hour" };
        public static HashSet<string> MinuteAdjustmentOptionValues = new HashSet<string>() {"تعديل دقيقه استلام الخدمه", "Adjust Minute" };
        public static HashSet<string> PartOfDayAdjustmentOptionValues = new HashSet<string>() {"تعديل نهارا/ليلا" , "Adjust PM/AM" };
    }                                                                                                      
}                                                                                                          
                                                                                                           