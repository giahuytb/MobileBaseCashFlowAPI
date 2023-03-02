using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IFriendShipStatusService
    {
        public Task<IEnumerable> GetAll();
        public Task<object?> GetFriendList(string userId);
        public Task<string> AddFriend(string requesterId, string addresseeId);
        public Task<string> DeleteFriend(string requesterId, string addresseeId);
    }
}
