namespace BookingAMaintenanceService.Managers
{
    using BookingAMaintenanceService.Models;
    using Microsoft.Bot.Builder;
    using System;
    using System.Threading.Tasks;

    public class ConversationStateDataAccessors
    {
        public const string ConversationDataName = "ConversationData";

        public const string UserDataName = "UserData";

        public ConversationStateDataAccessors(
            ConversationState conversationState, 
            UserState userState, 
            IStatePropertyAccessor<ConversationData> conversationDataAccessor,
            IStatePropertyAccessor<UserData> userDataAccessor)
        {
            this.ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
            this.ConversationDataAccessor = conversationDataAccessor ?? throw new ArgumentNullException(nameof(conversationDataAccessor));
            this.UserState = userState ?? throw new ArgumentNullException(nameof(userState));
            this.UserDataAccessor = userDataAccessor ?? throw new ArgumentNullException(nameof(userDataAccessor));
        }

        public IStatePropertyAccessor<UserData> UserDataAccessor { get; set; }

        public IStatePropertyAccessor<ConversationData> ConversationDataAccessor { get; set; }

        public ConversationState ConversationState { get; }

        public UserState UserState { get; }
        
        public async Task DeleteUserCachedData(ITurnContext turnContext)
        {
            await this.UpdateUserData(turnContext, null);
            await this.UpdateConversationData(turnContext, null);
        }
        
        public async Task<ConversationData> GetConversationData(ITurnContext turnContext)
        {
            return await this.ConversationDataAccessor.GetAsync(turnContext, () => new ConversationData());
        }

        public async Task<UserData> GetUserData(ITurnContext turnContext)
        {
            return await this.UserDataAccessor.GetAsync(turnContext, () => new UserData());
        }

        public async Task UpdateUserData(ITurnContext turnContext, UserData userProfile)
        {
            // Update user state and save changes.
            await this.UserDataAccessor.SetAsync(turnContext, userProfile);
            await this.UserState.SaveChangesAsync(turnContext);
        }

        public async Task UpdateConversationData(ITurnContext turnContext, ConversationData conversationData)
        {
            // Update conversation state and save changes.
            await this.ConversationDataAccessor.SetAsync(turnContext, conversationData);
            await this.ConversationState.SaveChangesAsync(turnContext);
        }
    }
}
