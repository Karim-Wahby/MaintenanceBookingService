namespace BookingAMaintenanceService.Dialogs.Constants
{
    using BookingAMaintenanceService.Dialogs.Definitions;
    using BookingAMaintenanceService.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class General
    {
        public static Message InvalidValueProvided = new Message()
        {
            English = $"The value that you Entired couldn't be recognized,{Environment.NewLine}please select one of the provided options or enter a valid value",
            Arabic = $"القيمه التى ادخلتها غير صحيحه{Environment.NewLine}فضلا اختر واحده من القيم المعطاه او ادخل قيمه صحيحه"
        };

        public static Message EmptyValueProvided = new Message()
        {
            English = $"Sorry, didn't recive any response, Please write someting as a response",
            Arabic = $"معذره, لم نتلق ردا, برجاء كتابه شئ ما"
        };

        public static Message Greetings = new Message()
        {
            English = $"Hi there - {{0}}.{Environment.NewLine}Welcome To SALA7LEE.",
            Arabic = $"مرحبا بك {{0}}.{Environment.NewLine}اهلا بك مع صلحلى."
        };
    }
}
