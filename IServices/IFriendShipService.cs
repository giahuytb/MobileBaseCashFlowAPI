
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IFriendShipService
    {
        public Task<IEnumerable> GetFriendList(string userId);
        public Task<object?> SearchFriend(string userId, string FriendName);
        public Task<string> AddFriend(string RequesterId, string AddresseeId);
        public Task<string> DeleteFriend(string RequesterId, string AddresseeId);
    }
}
