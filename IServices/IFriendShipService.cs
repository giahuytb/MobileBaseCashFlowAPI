
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IFriendShipService
    {
        public Task<IEnumerable> GetAsync();
        public Task<IEnumerable> GetAsync(string requesterId, string addresseeId);
        public Task<string> AddFriendShip(string requesterId, string addresseeId);
    }
}
