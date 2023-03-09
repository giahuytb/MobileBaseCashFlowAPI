
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IFriendShipService
    {
        public Task<IEnumerable> GetAllFriendShip();
        public Task<IEnumerable> GetAllFriendShipStatus();
        public Task<object?> GetFriendList(string userId, string statusCode);
        public Task<object?> SearchFriend(string userId, string friendName);
        public Task<string> AddFriend(string requesterId, string addresseeId);
        public Task<string> UpdateFriendShipStatus(string requesterId, string addresseeId, string statusCode);
    }
}
