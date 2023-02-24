
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class FriendShipService : IFriendShipService
    {
        public const string SUCCESS = "success";
        private readonly MobileBasedCashFlowGameContext _context;

        public FriendShipService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetFriendList(string userId)
        {
            try
            {
                var friendShip = await (from friend in _context.Friendships
                                        join user in _context.UserAccounts on friend.RequesterId equals user.UserId
                                        where friend.RequesterId == userId || friend.AddresseeId == userId
                                        select new
                                        {
                                            friend.RequesterId,
                                            friend.AddresseeId,
                                            friend.CreateAt,
                                        }).ToListAsync();
                return friendShip;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Task<object?> SearchFriend(string userId, string friendName)
        {
            throw new NotImplementedException();
        }

        public Task<string> AddFriend(string RequesterId, string AddresseeId)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteFriend(string RequesterId, string AddresseeId)
        {
            throw new NotImplementedException();
        }


    }
}
