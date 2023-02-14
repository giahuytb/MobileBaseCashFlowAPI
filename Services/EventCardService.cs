using MongoDB.Driver;
using MobileBaseCashFlowGameAPI.IServices;
using Microsoft.EntityFrameworkCore;
using MobieBasedCashFlowAPI.MongoModels;
using MobieBasedCashFlowAPI.Settings;

namespace MobileBaseCashFlowGameAPI.Services
{
    public class EventCardService : IEventCardService
    {
        private readonly IMongoCollection<EventCardMg> _eventCard;
        public EventCardService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _eventCard = database.GetCollection<EventCardMg>("Event_card");
        }

        public async Task<List<EventCardMg>> GetAsync()
        {
            return await _eventCard.Find(_ => true).ToListAsync();
        }
            
        public async Task<EventCardMg?> GetAsync(string id)
        {
            return await _eventCard.Find(evt => evt._id == id).FirstOrDefaultAsync();
        }
            
        public async Task CreateAsync(EventCardMg eventCard)
        {
            await _eventCard.InsertOneAsync(eventCard);
        }
            
        public async Task UpdateAsync(string id, EventCardMg updatedEventCard)
        {
            await _eventCard.ReplaceOneAsync(x => x._id == id, updatedEventCard);
        }


        public async Task RemoveAsync(string id) 
        {
            await _eventCard.DeleteOneAsync(x => x._id == id);
        }
    }
}
