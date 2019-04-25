// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace BookingAMaintenanceService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using BookingAMaintenanceService.Managers;
    using BookingAMaintenanceService.Models;
    using Microsoft.Recognizers.Text;
    using Microsoft.Recognizers.Text.DateTime;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Schema;

    /// <summary>
    /// Represents a bot that processes incoming activities.
    /// For each user interaction, an instance of this class is created and the OnTurnAsync method is called.
    /// This is a Transient lifetime service. Transient lifetime services are created
    /// each time they're requested. Objects that are expensive to construct, or have a lifetime
    /// beyond a single turn, should be carefully managed.
    /// For example, the <see cref="MemoryStorage"/> object and associated
    /// <see cref="IStatePropertyAccessor{T}"/> object are created with a singleton lifetime.
    /// </summary>
    /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1"/>
    public class BookingAMaintenanceServiceBot : IBot
    {
        private readonly ConversationStateDataAccessors conversationStateDataAccessor;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>                        
        public BookingAMaintenanceServiceBot(ConversationStateDataAccessors accessors)
        {
            conversationStateDataAccessor = accessors ?? throw new ArgumentNullException(nameof(accessors));
        }

        /// <summary>
        /// Every conversation turn calls this method.
        /// </summary>
        /// <param name="turnContext">A <see cref="ITurnContext"/> containing all the data needed
        /// for processing this conversation turn. </param>
        /// <param name="cancellationToken">(Optional) A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> that represents the work queued to execute.</returns>
        /// <seealso cref="BotStateSet"/>
        /// <seealso cref="ConversationState"/>
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Handle Message activity type, which is the main activity type for shown within a conversational interface
            // Message activities may contain text, speech, interactive cards, and binary or unknown attachments.
            // see https://aka.ms/about-bot-activity-message to learn more about the message and other activity types
            switch (turnContext.Activity.Type)
            {
                case ActivityTypes.Message:
                    await HandleIncommingMessages(turnContext, cancellationToken);
                    break;
                case ActivityTypes.DeleteUserData:
                    await handleDeleteUserDataRequest(turnContext, cancellationToken);
                    break;
                case ActivityTypes.ConversationUpdate:
                    await HandleConverstationUpdates(turnContext, cancellationToken);
                    break;
                default:
                    break;
            }
        }

        private async Task handleDeleteUserDataRequest(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await conversationStateDataAccessor.DeleteUserCachedData(turnContext);
            await SendMessage("User Data Deleted (Y)!", turnContext, cancellationToken);
        }

        private async Task HandleConverstationUpdates(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            if (turnContext.Activity.MembersAdded != null)
            {
                var theBotId = turnContext.Activity.Recipient.Id;
                var addedUsers = turnContext.Activity.MembersAdded
                    .Where(addedMember => addedMember.Id != theBotId);

                if (addedUsers.Any())
                {
                    await SayHelloToUsers(addedUsers, turnContext, cancellationToken);
                }

            }
        }

        private static async Task SendMessageWithOptions(string message, ITurnContext turnContext, CancellationToken cancellationToken, params string[] options)
        {
            var reply = turnContext.Activity.CreateReply(message);
            reply.Type = ActivityTypes.Message;
            reply.TextFormat = TextFormatTypes.Plain;
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = options
                .Select(option => new CardAction() { Title = option, Type = ActionTypes.ImBack, Value = option })
                .ToList()
            };

            await turnContext.SendActivityAsync(reply, cancellationToken: cancellationToken);
        }

        private static async Task SendMessage(string message, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync(message, cancellationToken: cancellationToken);

        }


        private static async Task SendMessageBasedOnUserPreferredLanguage(
            string arabicMessage, 
            string englishMessage, 
            UserData userProfile, 
            ITurnContext turnContext, 
            CancellationToken cancellationToken)
        {
            if (userProfile.PreferredLanguage.Value == SupportedLanguage.Arabic)
            {
                await SendMessage(arabicMessage, turnContext, cancellationToken);
            }
            else
            {
                await SendMessage(englishMessage, turnContext, cancellationToken);
            }
        }


        private static async Task SendMessageBasedOnUserPreferredLanguage(
            string arabicMessage,
            string[] arabicOptions,
            string englishMessage,
            string[] englishOptions,
            UserData userProfile,
            ITurnContext turnContext,
            CancellationToken cancellationToken)
        {
            if (userProfile.PreferredLanguage.Value == SupportedLanguage.Arabic)
            {
                await SendMessageWithOptions(arabicMessage, turnContext, cancellationToken, arabicOptions);
            }
            else
            {
                await SendMessageWithOptions(englishMessage, turnContext, cancellationToken, englishOptions);
            }
        }

        public static void SetWaitingForUserInputFlag(ConversationData conversationData, bool value = true)
        {
            conversationData.WaitingForUserInput = value;
        }

        public static string GetUserReply(ITurnContext turnContext)
        {
            return turnContext.Activity.Text;
        }

        private async Task SayHelloToUsers(IEnumerable<ChannelAccount> addedUsers, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            if (addedUsers == null || !addedUsers.Any())
            {
                return;
            }

            foreach (ChannelAccount member in addedUsers)
            {
                await SendMessage($"Hi there - {member.Name}.{Environment.NewLine}Welcome To SALA7LEE.",
                    turnContext, cancellationToken);
            }
        }

        private async Task HandleIncommingMessages(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Get the state properties from the turn context.
            var userProfile = await conversationStateDataAccessor.GetUserData(turnContext);
            var conversationData = await conversationStateDataAccessor.GetConversationData(turnContext);

            do
            {
                // case ConversationPhases.UpdatingTheUserWithHisBookingRequestsStatus:
                //     break;
                // case ConversationPhases.RequestingUserFeedbackOfDeliveredService:
                //     break;
                if (!userProfile.PreferredLanguage.HasValue)
                {
                    // it's Time To ask the user for his preferred language
                    await SelectingUserPreferredLanguage(turnContext, cancellationToken, conversationData, userProfile);
                }
                else if (!conversationData.CurrentConversationIntent.HasValue)
                {
                    // it's time to know what is the user intent 
                    await SelectingUserIntentFromConversation(turnContext, cancellationToken, conversationData, userProfile);
                }
                else if (conversationData.CurrentConversationIntent == BotSupportedIntents.BookingAMaintenanceService)
                {
                    // the user want to book one of our services
                    await BookingAMaintenanceServiceConversation(turnContext, cancellationToken, conversationData, userProfile);
                }
                else if (conversationData.CurrentConversationIntent == BotSupportedIntents.GettingUpdatesAboutCurrentRequests)
                {
                    await SendMessage("getting updates about current requests dialog", turnContext, cancellationToken);
                    userProfile = new UserData();
                    conversationData = new ConversationData();
                }
                else
                {
                    await SendMessage("Next Step !!!", turnContext, cancellationToken);
                    userProfile = new UserData();
                    conversationData = new ConversationData();
                }
            } while (!conversationData.WaitingForUserInput) ;
            
            await conversationStateDataAccessor.UpdateUserData(turnContext, userProfile);
            await conversationStateDataAccessor.UpdateConversationData(turnContext, conversationData);
        }

        private async Task BookingAMaintenanceServiceConversation(ITurnContext turnContext, CancellationToken cancellationToken, ConversationData conversationData, UserData userProfile)
        {
            if (conversationData.ServiceBookingForm == null)
            {
                conversationData.ServiceBookingForm = new BookingAMaintenanceServiceForm();
            }

            if (conversationData.WaitingForUserInput)
            {
                var userInput = GetUserReply(turnContext)?.Trim();
                if (string.IsNullOrWhiteSpace(userInput))
                {
                    if (userProfile.PreferredLanguage == SupportedLanguage.Arabic)
                    {
                        await SendMessage($"معذره, لم نتلق ردا, برجاء كتابه شئ ما", turnContext, cancellationToken);
                    }
                    else
                    {
                        await SendMessage($"Sorry, didn't recive any response, Please write someting as a response", turnContext, cancellationToken);
                    }

                    return;
                }

                if (conversationData.ServiceBookingForm.RequestedService == null)
                {
                    switch (userInput)
                    {
                        case "1":
                            conversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.Carpentry;
                            break;
                        case "2":
                            conversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.ElectricalMaintenance;
                            break;
                        case "3":
                            conversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.PlumbingServices;
                            break;
                        case "4":
                            conversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.AirConditioningMaintenance;
                            break;
                        case "5":
                            conversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.PaintingServices;
                            break;
                        case "6":
                            conversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.Cleaning;
                            break;
                    }

                    if (conversationData.ServiceBookingForm.RequestedService.HasValue)
                    {
                        SetWaitingForUserInputFlag(conversationData, false);
                        return;
                    }

                    if (userProfile.PreferredLanguage == SupportedLanguage.Arabic)
                    {
                        await SendMessage($" بالعربى {Environment.NewLine}" +
                            $"The value that you Entired couldn't be recognized,{Environment.NewLine}" +
                            "please select one of the provided options (1, 2, ...)",
                            turnContext,
                            cancellationToken);
                    }
                    else
                    {
                        userInput = userInput.ToLower();
                        if (userInput.Contains("carpent"))
                        {
                            conversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.Carpentry;
                        }
                        else if (userInput.Contains("electrical"))
                        {
                            conversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.ElectricalMaintenance;
                        }
                        else if (userInput.Contains("plumbing"))
                        {
                            conversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.PlumbingServices;
                        }
                        else if (userInput.Contains("conditioning") || userInput.Contains("air"))
                        {
                            conversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.AirConditioningMaintenance;
                        }
                        else if (userInput.Contains("paint"))
                        {
                            conversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.PaintingServices;
                        }
                        else if (userInput.Contains("Carpent"))
                        {
                            conversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.Carpentry;
                        }
                        else if (userInput.Contains("cleaning"))
                        {
                            conversationData.ServiceBookingForm.RequestedService = SupportedMaintenanceServices.Cleaning;
                        }
                        else
                        {
                            await SendMessage($"The value that you Entired couldn't be recognized,{Environment.NewLine}" +
                                "please select one of the provided options (1, 2, ...)",
                                turnContext,
                                cancellationToken);
                        }
                    }

                    if (conversationData.ServiceBookingForm.RequestedService.HasValue)
                    {
                        SetWaitingForUserInputFlag(conversationData, false);
                        return;
                    }
                }
                else if (conversationData.ServiceBookingForm.RequiredServiceDescription == null)
                {
                    conversationData.ServiceBookingForm.RequiredServiceDescription = userInput;
                    SetWaitingForUserInputFlag(conversationData, false);
                }
                else if (conversationData.ServiceBookingForm.DeliveryLocation == null)
                {
                    if (!string.IsNullOrWhiteSpace(userInput))
                    {
                        conversationData.ServiceBookingForm.DeliveryLocation = userInput;
                        SetWaitingForUserInputFlag(conversationData, false);
                    }
                    else
                    {
                        if (userProfile.PreferredLanguage == SupportedLanguage.Arabic)
                        {
                            await SendMessage($"فضلا, اكتب عنوان لتسليم الخدمه المطلوبه", turnContext, cancellationToken);
                        }
                        else
                        {
                            await SendMessage($"Please Enter a Valid address", turnContext, cancellationToken);
                        }
                    }
                }
                else if (!conversationData.ServiceBookingForm.RequiredSerivceTime.HasValue)
                {
                    switch (userInput)
                    {
                        case "I need the service now / ASAP":
                        case "اريد الخدمه الان / فى اقرب وقت ممكن":
                            conversationData.ServiceBookingForm.SerivceIsRequiredASAP = true;
                            conversationData.ServiceBookingForm.RequiredSerivceTime = DateTime.Now.AddHours(1);
                            break;
                    }

                    if (conversationData.ServiceBookingForm.RequiredSerivceTime.HasValue)
                    {
                        SetWaitingForUserInputFlag(conversationData, false);
                        return;
                    }

                    if (conversationData.ServiceBookingForm.FailedToRecognizeProvidedDateTime)
                    {
                        if (!conversationData.ServiceBookingForm.Year.HasValue)
                        {
                            int year = -1;
                            if (int.TryParse(userInput, out year) && year >= DateTime.Now.Year)
                            {
                                conversationData.ServiceBookingForm.Year = year;
                                SetWaitingForUserInputFlag(conversationData, false);
                            }
                            else if (year == -1)
                            {
                                if (userProfile.PreferredLanguage == SupportedLanguage.Arabic)
                                {
                                    await SendMessage($"معذره, لم نتلق رقماو برجاء ادخال القيمه بالارقام اللانجليزيه", turnContext, cancellationToken);
                                }
                                else
                                {
                                    await SendMessage($"Sorry, didn't receive any number.", turnContext, cancellationToken);
                                }
                            }
                            else
                            {
                                if (userProfile.PreferredLanguage == SupportedLanguage.Arabic)
                                {
                                    await SendMessage($"معذره, لا يمكنك الحجز فى سنه ماضيه", turnContext, cancellationToken);
                                }
                                else
                                {
                                    await SendMessage($"Sorry, you can't book in the past, please provide us with an upcomming year.", turnContext, cancellationToken);
                                }
                            }
                        }
                        else if (!conversationData.ServiceBookingForm.Month.HasValue)
                        {
                            int month = -1;
                            if (int.TryParse(userInput, out month) && 
                                month > 0 && month < 13 && 
                                (conversationData.ServiceBookingForm.Year > DateTime.Now.Year || month >= DateTime.Now.Month))
                            {
                                conversationData.ServiceBookingForm.Month = month;
                                SetWaitingForUserInputFlag(conversationData, false);
                            }
                            else if (month == -1)
                            {
                                if (userProfile.PreferredLanguage == SupportedLanguage.Arabic)
                                {
                                    await SendMessage($"معذره, لم نتلق رقماو برجاء ادخال القيمه بالارقام اللانجليزيه", turnContext, cancellationToken);
                                }
                                else
                                {
                                    await SendMessage($"Sorry, didn't receive any number.", turnContext, cancellationToken);
                                }
                            }
                            else
                            {
                                if (userProfile.PreferredLanguage == SupportedLanguage.Arabic)
                                {
                                    await SendMessage($"معذره, لا يمكنك الحجز فى شهر ماضيه", turnContext, cancellationToken);
                                }
                                else
                                {
                                    await SendMessage($"Sorry, you can't book in the past, please provide us with an upcomming/current Month.", turnContext, cancellationToken);
                                }
                            }
                        }
                        else if (!conversationData.ServiceBookingForm.Day.HasValue)
                        {
                            int day = -1;
                            if (int.TryParse(userInput, out day) &&
                                day > 0 &&
                                day <= DateTime.DaysInMonth(conversationData.ServiceBookingForm.Year.Value, conversationData.ServiceBookingForm.Month.Value) &&
                                (conversationData.ServiceBookingForm.Year > DateTime.Now.Year ||
                                    conversationData.ServiceBookingForm.Month > DateTime.Now.Month ||
                                    day >= DateTime.Now.Day))
                            {
                                conversationData.ServiceBookingForm.Day = day;
                                SetWaitingForUserInputFlag(conversationData, false);
                            }
                            else if (day == -1)
                            {
                                if (userProfile.PreferredLanguage == SupportedLanguage.Arabic)
                                {
                                    await SendMessage($"معذره, لم نتلق رقماو برجاء ادخال القيمه بالارقام اللانجليزيه", turnContext, cancellationToken);
                                }
                                else
                                {
                                    await SendMessage($"Sorry, didn't receive any number.", turnContext, cancellationToken);
                                }
                            }
                            else if (day < 1 || day > DateTime.DaysInMonth(conversationData.ServiceBookingForm.Year.Value, conversationData.ServiceBookingForm.Month.Value))
                            {
                                if (userProfile.PreferredLanguage == SupportedLanguage.Arabic)
                                {
                                    await SendMessage($"معذره, لا يمكنك الحجز فى يوم غير موجود بالشهر المطلوب", turnContext, cancellationToken);
                                }
                                else
                                {
                                    await SendMessage($"Sorry, you can't book in a day that doesn't exist in the requested month for the requested year.", turnContext, cancellationToken);
                                }
                            }
                            else
                            {
                                if (userProfile.PreferredLanguage == SupportedLanguage.Arabic)
                                {
                                    await SendMessage($"معذره, لا يمكنك الحجز فى يوم ماضيه", turnContext, cancellationToken);
                                }
                                else
                                {
                                    await SendMessage($"Sorry, you can't book in the past, please provide us with an upcomming/current day.", turnContext, cancellationToken);
                                }
                            }
                        }
                        else if (string.IsNullOrWhiteSpace(conversationData.ServiceBookingForm.DayOrNight))
                        {
                            switch (userInput)
                            {
                                case "AM":
                                case "am":
                                case "صباحا":
                                    conversationData.ServiceBookingForm.DayOrNight = "AM";
                                    break;
                                case "PM":
                                case "pm":
                                case "مساء":
                                    conversationData.ServiceBookingForm.DayOrNight = "PM";
                                    break;
                            }

                            if (!string.IsNullOrWhiteSpace(conversationData.ServiceBookingForm.DayOrNight))
                            {
                                SetWaitingForUserInputFlag(conversationData, false);
                            }
                            else
                            {
                                if (userProfile.PreferredLanguage == SupportedLanguage.Arabic)
                                {
                                    await SendMessage($"رجاء اختر واحده من الاختيارت المعروضه", turnContext, cancellationToken);
                                }
                                else
                                {
                                    await SendMessage($"Sorry, please select one of the provided options.", turnContext, cancellationToken);
                                }
                            }
                        }            
                        else if (!conversationData.ServiceBookingForm.Hour.HasValue)
                        {
                            int hour = -1;
                            if (int.TryParse(userInput, out hour) &&
                                hour > 0 &&
                                hour < 13 &&
                                hour >= DateTime.Now.Hour + 1)
                            {
                                conversationData.ServiceBookingForm.Hour = hour;
                                SetWaitingForUserInputFlag(conversationData, false);
                            }
                            else if (hour == -1)
                            {
                                if (userProfile.PreferredLanguage == SupportedLanguage.Arabic)
                                {
                                    await SendMessage($"معذره, لم نتلق رقماو برجاء ادخال القيمه بالارقام اللانجليزيه", turnContext, cancellationToken);
                                }
                                else
                                {
                                    await SendMessage($"Sorry, didn't receive any number.", turnContext, cancellationToken);
                                }
                            }
                            else if (hour == DateTime.Now.Hour)
                            {
                                if (userProfile.PreferredLanguage == SupportedLanguage.Arabic)
                                {
                                    await SendMessage($"معذره, لا يمكنك الحجز قبل ميعاد تسلم الخدمه باقل من ساعه", turnContext, cancellationToken);
                                }
                                else
                                {
                                    await SendMessage($"Sorry, you can't book before the delivery time with less than an hour", turnContext, cancellationToken);
                                }
                            }
                            else if (hour < 1 || hour > 12)
                            {
                                if (userProfile.PreferredLanguage == SupportedLanguage.Arabic)
                                {
                                    await SendMessage($"معذره, برجاء ادخال رقم صحيح", turnContext, cancellationToken);
                                }
                                else
                                {
                                    await SendMessage($"Sorry, please enter a valid number.", turnContext, cancellationToken);
                                }
                            }
                            else
                            {
                                if (userProfile.PreferredLanguage == SupportedLanguage.Arabic)
                                {
                                    await SendMessage($"معذره, لا يمكنك الحجز فى وقت ماضيه", turnContext, cancellationToken);
                                }
                                else
                                {
                                    await SendMessage($"Sorry, you can't book in the past, please provide us with an upcomming/current day.", turnContext, cancellationToken);
                                }
                            }
                        }
                        else if (!conversationData.ServiceBookingForm.Minutes.HasValue)
                        {
                            int minutes = -1;
                            if (int.TryParse(userInput, out minutes) &&
                                minutes > 0 &&
                                minutes < 60)
                            {
                                conversationData.ServiceBookingForm.Minutes = minutes;
                                conversationData.ServiceBookingForm.RequiredSerivceTime = 
                                    new DateTime(
                                        conversationData.ServiceBookingForm.Year.Value,
                                        conversationData.ServiceBookingForm.Month.Value,
                                        conversationData.ServiceBookingForm.Day.Value,
                                        conversationData.ServiceBookingForm.Hour.Value,
                                        conversationData.ServiceBookingForm.Minutes.Value,
                                        second: 0);
                                SetWaitingForUserInputFlag(conversationData, false);
                            }
                            else if (minutes == -1)
                            {
                                if (userProfile.PreferredLanguage == SupportedLanguage.Arabic)
                                {
                                    await SendMessage($"معذره, لم نتلق رقماو برجاء ادخال القيمه بالارقام اللانجليزيه", turnContext, cancellationToken);
                                }
                                else
                                {
                                    await SendMessage($"Sorry, didn't receive any number.", turnContext, cancellationToken);
                                }
                            }
                            else
                            {
                                if (userProfile.PreferredLanguage == SupportedLanguage.Arabic)
                                {
                                    await SendMessage($"معذره, برجاء ادخال رقم صحيح", turnContext, cancellationToken);
                                }
                                else
                                {
                                    await SendMessage($"Sorry, please enter a valid number.", turnContext, cancellationToken);
                                }
                            }
                        }
                    }
                    else
                    {
                        var earliest = DateTime.Now.AddHours(1.0);
                        if (DateTime.TryParse(userInput, out var time))
                        {
                            var culture = Culture.English;
                            // Datetime recognizer This model will find any Date even if its write in coloquial language 
                            // E.g "I'll go back 8pm today" will return "2017-10-04 20:00:00"
                            var result = DateTimeRecognizer.RecognizeDateTime(userInput, culture);

                            if (result.Any(entity => entity.TypeName == "datetime"))
                            {
                                var userTime = DateTime.Parse(result.First(entity => entity.TypeName == "datetime").Text);
                                if (userTime >= earliest)
                                {
                                    conversationData.ServiceBookingForm.RequiredSerivceTime = userTime;
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
            else
            {
                if (!conversationData.ServiceBookingForm.RequestedService.HasValue)
                {
                    await SendMessageBasedOnUserPreferredLanguage(
                        $"فصلا اختر خدمه الصيانه المطلوبه:-{Environment.NewLine}" +
                            $"1- نجاره و هذا يضم الكوالين و المفاتيح / اعمال خشبيه{Environment.NewLine}" +
                            $"2- كهرباء, و هذا يتضمن اعمال الثلاجات و الغسالات و خلافه{Environment.NewLine}" +
                            $"3- خدمات سباكه{Environment.NewLine}" +
                            $"4- صيانه مكيف الهواء{Environment.NewLine}" +
                            $"5- خدمات نقاشه{Environment.NewLine}" +
                            $"6- تنظيف",
                        $"Please Select the maintenance service you need :-{Environment.NewLine}" +
                            $"1- Carpentry (including lock and key services){Environment.NewLine}" +
                            $"2- Electrical maintenance (including refrigeration, alarms and controls, and lighting services){Environment.NewLine}" +
                            $"3- Plumbing services{Environment.NewLine}" +
                            $"4- Air conditioning maintenance{Environment.NewLine}" +
                            $"5- Painting services{Environment.NewLine}" +
                            $"6- Cleaning",
                        userProfile,
                        turnContext,
                        cancellationToken);
                }
                else if (conversationData.ServiceBookingForm.RequiredServiceDescription == null)
                {
                    await SendMessageBasedOnUserPreferredLanguage(
                        $"فضلا, قم بوصف الخدمه المطلوبه",
                        $"Please Describe the requested service",
                        userProfile,
                        turnContext,
                        cancellationToken);
                }
                else if (conversationData.ServiceBookingForm.DeliveryLocation == null)
                {
                    await SendMessageBasedOnUserPreferredLanguage(
                        $"اكتب العنوان لتوصيل الخدمه",
                        $"Please enter the address to deliver the serive to",
                        userProfile,
                        turnContext,
                        cancellationToken);
                }
                else if (!conversationData.ServiceBookingForm.RequiredSerivceTime.HasValue)
                {
                    if (conversationData.ServiceBookingForm.FailedToRecognizeProvidedDateTime)
                    {
                        if (userProfile.PreferredLanguage == SupportedLanguage.Arabic)
                        {
                            if (!conversationData.ServiceBookingForm.Year.HasValue)
                            {
                                await SendMessageWithOptions($"اكتب سنه تسلم الخدمه",
                                        turnContext,
                                        cancellationToken,
                                        "اريد الخدمه الان / فى اقرب وقت ممكن",
                                        DateTime.Now.Year.ToString());
                            }
                            else if (!conversationData.ServiceBookingForm.Month.HasValue)
                            {
                                await SendMessageWithOptions($"اكتب شهر تسلم الخدمه",
                                        turnContext,
                                        cancellationToken,
                                        DateTime.Now.Month.ToString());
                            }
                            else if (!conversationData.ServiceBookingForm.Day.HasValue)
                            {
                                await SendMessageWithOptions($"اكتب يوم تسلم الخدمه",
                                        turnContext,
                                        cancellationToken,
                                        DateTime.Now.Day.ToString());
                            }
                            else if (string.IsNullOrWhiteSpace(conversationData.ServiceBookingForm.DayOrNight))
                            {
                                await SendMessageWithOptions($"اختر جزى من اليوم لتسلم الخدمه",
                                        turnContext,
                                        cancellationToken,
                                        "صباحا",
                                        "مساء");
                            }
                            else if (!conversationData.ServiceBookingForm.Hour.HasValue)
                            {
                                await SendMessage($"اختر الساعه من اليوم لتسلم الخدمه",
                                        turnContext,
                                        cancellationToken);
                            }
                            else if (!conversationData.ServiceBookingForm.Minutes.HasValue)
                            {
                                await SendMessageWithOptions($"اختر الدقيقه من اليوم لتسلم الخدمه",
                                        turnContext,
                                        cancellationToken,
                                        "15",
                                        "30",
                                        "45");
                            }
                        }
                        else
                        {
                            if (!conversationData.ServiceBookingForm.Year.HasValue)
                            {
                                await SendMessageWithOptions($"Please enter the Year to deliver the serive",
                                        turnContext,
                                        cancellationToken,
                                        "I need the service now / ASAP",
                                        DateTime.Now.Year.ToString());
                            }
                            else if (!conversationData.ServiceBookingForm.Month.HasValue)
                            {
                                await SendMessageWithOptions($"Please enter the Month to deliver the serive",
                                        turnContext,
                                        cancellationToken,
                                        DateTime.Now.Month.ToString());
                            }
                            else if (!conversationData.ServiceBookingForm.Day.HasValue)
                            {
                                await SendMessageWithOptions($"Please enter the Day to deliver the serive",
                                        turnContext,
                                        cancellationToken,
                                        DateTime.Now.Day.ToString());
                            }
                            else if (string.IsNullOrWhiteSpace(conversationData.ServiceBookingForm.DayOrNight))
                            {
                                await SendMessageWithOptions($"Please Select the part of day to deliver the serive",
                                        turnContext,
                                        cancellationToken,
                                        "AM",
                                        "PM");
                            }
                            else if (!conversationData.ServiceBookingForm.Hour.HasValue)
                            {
                                await SendMessage($"Please enter the hour of the day to deliver the serive",
                                        turnContext,
                                        cancellationToken);
                            }
                            else if (!conversationData.ServiceBookingForm.Minutes.HasValue)
                            {
                                await SendMessageWithOptions($"Please enter the Minute of the day to deliver the serive",
                                        turnContext,
                                        cancellationToken,
                                        "15",
                                        "30",
                                        "45");
                            }
                        }
                    }
                    else
                    {
                        if (userProfile.PreferredLanguage == SupportedLanguage.Arabic)
                        {
                            await SendMessageWithOptions($"اكتب تاريخ تسلم الخدمه(يوم/شهر/سنه) بالارقم رجاء",
                                        turnContext,
                                        cancellationToken,
                                        "اريد الخدمه الان / فى اقرب وقت ممكن",
                                        DateTime.Now.Year.ToString());
                        }
                        else
                        {
                            await SendMessageWithOptions($"Please enter the Date to deliver the serive (Day/Month/Year)",
                                       turnContext,
                                       cancellationToken,
                                       DateTime.Now.Month.ToString());
                        }
                    }
                }
                else
                {
                    await SendMessage(
                        $"Your Request is being processed, we will keep you posted with the updates",
                        turnContext, 
                        cancellationToken);
                }

                SetWaitingForUserInputFlag(conversationData);
            }
        }

        private async Task SelectingUserIntentFromConversation(ITurnContext turnContext, CancellationToken cancellationToken, ConversationData conversationData, UserData userProfile)
        {
            if (conversationData.WaitingForUserInput)
            {
                var userInput = GetUserReply(turnContext)?.Trim();
                switch (userInput)
                {
                    case "1":
                        conversationData.CurrentConversationIntent = BotSupportedIntents.BookingAMaintenanceService;
                        break;
                    case "2":
                        conversationData.CurrentConversationIntent = BotSupportedIntents.GettingUpdatesAboutCurrentRequests;
                        break;
                    default:
                        userInput = userInput.ToLower();
                        if (userInput.Contains("status") || userInput.Contains("check"))
                        {
                            conversationData.CurrentConversationIntent = BotSupportedIntents.GettingUpdatesAboutCurrentRequests;
                        }
                        else if (userInput.Contains("book") || userInput.Contains("new"))
                        {
                            conversationData.CurrentConversationIntent = BotSupportedIntents.BookingAMaintenanceService;
                        }
                        break;
                }

                if (conversationData.CurrentConversationIntent.HasValue)
                {
                    SetWaitingForUserInputFlag(conversationData, false);
                    return;
                }
                else
                {
                    await SendMessageBasedOnUserPreferredLanguage(
                        $" بالعربى {Environment.NewLine}" +
                            $"The value that you Entired couldn't be recognized,{Environment.NewLine}" +
                            "please select one of the provided options (1, 2, ...)",
                        $"The value that you Entired couldn't be recognized,{Environment.NewLine}" +
                            "please select one of the provided options (1, 2, ...)",
                        userProfile,
                        turnContext,
                        cancellationToken
                        );
                }
            }
            else
            {
                await SendMessageBasedOnUserPreferredLanguage(
                    $"كيف استطيع مساعدتك اليوم:{Environment.NewLine}" +
                        $"1- حجز خدمه صيانه{Environment.NewLine}" +
                        $"2- تفقد الحاله الحاليه لخدماتك{Environment.NewLine}",
                    $"How Can I Help You Today:{Environment.NewLine}" +
                        $"1- Book a new Maintenance Service {Environment.NewLine}" +
                        $"2- Check your Requests Status{Environment.NewLine}",
                    userProfile,
                    turnContext,
                    cancellationToken
                    );

                SetWaitingForUserInputFlag(conversationData);
            }
        }

        private async Task SelectingUserPreferredLanguage(ITurnContext turnContext, CancellationToken cancellationToken, ConversationData conversationData, UserData userProfile)
        {
            if (conversationData.WaitingForUserInput)
            {
                var userInput = GetUserReply(turnContext)?.ToLower().Trim();
                switch (userInput)
                {
                    case "english":
                    case "eng":
                    case "1":
                        userProfile.PreferredLanguage = SupportedLanguage.English;
                        break;
                    case "ar":
                    case "arabic":
                    case "عربى":
                    case "2":
                        userProfile.PreferredLanguage = SupportedLanguage.Arabic;
                        break;
                    default:
                        await SendMessage($"The value that you Entired couldn't be recognized,{Environment.NewLine}" +
                            "please select one of the provided options (1, 2, ...)",
                            turnContext,
                            cancellationToken);
                        break;
                }

                if (userProfile.PreferredLanguage.HasValue)
                {
                    SetWaitingForUserInputFlag(conversationData, false);
                }
            }
            else
            {
                await SendMessageWithOptions(
                $"Please Select Your prefered Language:-{Environment.NewLine}" +
                $"أختر اللغه التى تفضلها رجاء{Environment.NewLine}" +
                $"1- English{Environment.NewLine}" +
                $"2- عربى",
                turnContext,
                cancellationToken,
                "عربى",
                "English");

                SetWaitingForUserInputFlag(conversationData);
            }
        }
    }
}
