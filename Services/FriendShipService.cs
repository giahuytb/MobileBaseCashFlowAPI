
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.DTO;
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

        public async Task<IEnumerable> GetAsync()
        {
            try
            {
                var friendShip = await (from friend in _context.Friendships
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

        public async Task<IEnumerable> GetAsync(string requesterId, string addresseeId)
        {
            try
            {
                var friendShip = await (from friend in _context.Friendships
                                        join requester in _context.UserAccounts on friend.RequesterId equals requester.UserId
                                        join addressee in _context.UserAccounts on friend.AddresseeId equals addressee.UserId
                                        where friend.RequesterId == requesterId || friend.AddresseeId == addresseeId
                                        select new
                                        {
                                            addressee.NickName,
                                            friend.CreateAt,
                                        }).ToListAsync();
                return friendShip;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> AddFriendShip(string RequesterId, string AddresseeId)
        {
            try
            {
                var checkExist = await _context.Friendships
                    .Where(f => f.RequesterId == RequesterId && f.AddresseeId == AddresseeId)
                    .FirstOrDefaultAsync();

                if (checkExist != null)
                {
                    return "This friend ship has already existed";
                }
                var Friendship = new Friendship()
                {
                    RequesterId = RequesterId,
                    AddresseeId = AddresseeId,
                    CreateAt = DateTime.Now,
                };

                _context.Friendships.Add(Friendship);
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


    }
}
