using MobieBasedCashFlowAPI.MongoModels;

namespace MobileBaseCashFlowGameAPI.IServices
{
    public interface IEventCardService
    {
        public Task<List<EventCardMg>> GetAsync();
        public Task<EventCardMg?> GetAsync(string id);
        public Task CreateAsync(EventCardMg eventCard);
        public Task UpdateAsync(string id, EventCardMg eventCard);
        public Task RemoveAsync(string id);
    }
}
