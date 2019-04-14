namespace BookingAMaintenaceService.Models
{
    public enum ConversationStates
    {
        GreetingTheUser,
        SelectingUserPreferredLanguage,
        SelectingUserIntent,
        BookingAMaintenanceService,
        UpdatingTheUserWithHisBookingRequestsStatus,
        RequestingUserFeedbackOfDeliveredService,
    }
}
