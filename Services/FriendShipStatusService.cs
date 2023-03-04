using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using NuGet.ContentModel;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class FriendShipStatusService : IFriendShipStatusService
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public FriendShipStatusService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable> GetAll()
        {
            var friendShip = await _context.FriendshipStatuses.ToListAsync();
            return friendShip;
        }

        public async Task<object?> GetFriendList(string userId)
        {
            try
            {
                //Get the list of friends you requested, and they are accepter your request at the last specifie_date_time
                var friendShip = await (from FS in _context.FriendshipStatuses
                                        join Reqter in _context.UserAccounts on FS.RequesterId equals Reqter.UserId
                                        join Adrsee in _context.UserAccounts on FS.RequesterId equals Adrsee.UserId
                                        where FS.RequesterId == userId
                                            & FS.StatusCode == "Accepted"
                                            & FS.SpecifiedDateTime == (from NestedFS in _context.FriendshipStatuses
                                                                       where NestedFS.RequesterId == FS.RequesterId
                                                                       & NestedFS.AddresseeId == FS.AddresseeId
                                                                       select NestedFS.SpecifiedDateTime).FirstOrDefault()
                                        select new
                                        {
                                            Adrsee.NickName,
                                            FS.StatusCode,
                                        }).ToListAsync();
                return friendShip;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Task<string> AddFriend(string requesterId, string addresseeId)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteFriend(string requesterId, string addresseeId)
        {
            throw new NotImplementedException();
        }




    }
}
