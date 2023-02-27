using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MobileBasedCashFlowAPI.IMongoServices;

namespace MobileBasedCashFlowAPI.MongoServices
{
    public class EventCardService : IEventCardService
    {
        private readonly IMongoCollection<EventCard> _eventCard;
        public EventCardService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _eventCard = database.GetCollection<EventCard>("Event_card");
        }

        public async Task<List<EventCard>> GetAsync()
        {
            return await _eventCard.Find(_ => true).ToListAsync();
        }

        public async Task<EventCard?> GetAsync(string id)
        {
            return await _eventCard.Find(evt => evt._id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(EventCard eventCard)
        {
            await _eventCard.InsertOneAsync(eventCard);
        }

        public async Task UpdateAsync(string id, EventCard updatedEventCard)
        {
            await _eventCard.ReplaceOneAsync(x => x._id == id, updatedEventCard);
        }

        public async Task RemoveAsync(string id)
        {
            await _eventCard.DeleteOneAsync(x => x._id == id);
        }
    }
}
