
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

        public async Task<IEnumerable> GetAllFriendShip()
        {
            try
            {
                var friendShip = await (from FS in _context.Friendships
                        .Include(fs => fs.Requester)
                        .Include(fs => fs.Addressee)
                                        select new
                                        {
                                            requesterId = FS.RequesterId,
                                            requesterName = FS.Requester.NickName,
                                            adresseeId = FS.AddresseeId,
                                            adresseeName = FS.Addressee.NickName,
                                        }).ToListAsync();
                return friendShip;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<IEnumerable> GetAllFriendShipStatus()
        {
            try
            {
                var friendShip = await (from FS in _context.FriendshipStatuses
                                        join Reqter in _context.UserAccounts on FS.RequesterId equals Reqter.UserId
                                        join Adrsee in _context.UserAccounts on FS.AddresseeId equals Adrsee.UserId
                                        join Sp in _context.UserAccounts on FS.SpecifierId equals Sp.UserId
                                        select new
                                        {
                                            requester = Reqter.NickName,
                                            adressee = Adrsee.NickName,
                                            specifiedDateTime = FS.SpecifiedDateTime,
                                            statusCode = FS.StatusCode,
                                            speccifier = Sp.NickName,
                                        }).ToListAsync();
                return friendShip;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<object?> GetFriendList(string userId, string statusCode)
        {
            try
            {
                // Get the list of friends you have requested, and they are accepted your request at the last specifie_date_time
                // Union 
                // Get the list of friends you are addreseed, and you are accepted their request at the last specifie_date_time
                var friendShip = await (from FS in _context.FriendshipStatuses
                                        join Reqter in _context.UserAccounts on FS.RequesterId equals Reqter.UserId
                                        join Adrsee in _context.UserAccounts on FS.AddresseeId equals Adrsee.UserId
                                        where FS.RequesterId == userId
                                            & FS.StatusCode == statusCode
                                            & FS.SpecifiedDateTime == (from NestedFS in _context.FriendshipStatuses
                                                                       where NestedFS.RequesterId == FS.RequesterId
                                                                           & NestedFS.AddresseeId == FS.AddresseeId
                                                                       orderby NestedFS.SpecifiedDateTime descending
                                                                       select NestedFS.SpecifiedDateTime)
                                                                       .FirstOrDefault()
                                        select new
                                        {
                                            friendName = Adrsee.NickName,
                                            FS.StatusCode,
                                        }).Union(from FS in _context.FriendshipStatuses
                                                 join Reqter in _context.UserAccounts on FS.RequesterId equals Reqter.UserId
                                                 join Adrsee in _context.UserAccounts on FS.AddresseeId equals Adrsee.UserId
                                                 where FS.AddresseeId == userId
                                                     & FS.StatusCode == statusCode
                                                     & FS.SpecifiedDateTime == (from NestedFS in _context.FriendshipStatuses
                                                                                where NestedFS.RequesterId == FS.RequesterId
                                                                                    & NestedFS.AddresseeId == FS.AddresseeId
                                                                                orderby NestedFS.SpecifiedDateTime descending
                                                                                select NestedFS.SpecifiedDateTime)
                                                                                .FirstOrDefault()

                                                 select new
                                                 {
                                                     friendName = Reqter.NickName,
                                                     FS.StatusCode,
                                                 }).ToListAsync();
                return friendShip;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public async Task<object?> SearchFriend(string userId, string friendName)
        {
            var friendShip = await (from FS in _context.FriendshipStatuses
                                    join Reqter in _context.UserAccounts on FS.RequesterId equals Reqter.UserId
                                    join Adrsee in _context.UserAccounts on FS.AddresseeId equals Adrsee.UserId
                                    where FS.RequesterId == userId
                                        & Adrsee.NickName.Contains(friendName)
                                        & FS.StatusCode == "Accepted"
                                        & FS.SpecifiedDateTime == (from NestedFS in _context.FriendshipStatuses
                                                                   where NestedFS.RequesterId == FS.RequesterId
                                                                       & NestedFS.AddresseeId == FS.AddresseeId
                                                                   orderby NestedFS.SpecifiedDateTime descending
                                                                   select NestedFS.SpecifiedDateTime)
                                                                   .FirstOrDefault()
                                    select new
                                    {
                                        Adrsee.NickName,
                                        FS.StatusCode,
                                    }).Union(from FS in _context.FriendshipStatuses
                                             join Reqter in _context.UserAccounts on FS.RequesterId equals Reqter.UserId
                                             join Adrsee in _context.UserAccounts on FS.AddresseeId equals Adrsee.UserId
                                             where FS.AddresseeId == userId
                                                 & Reqter.NickName.Contains(friendName)
                                                 & FS.StatusCode == "Accepted"
                                                 & FS.SpecifiedDateTime == (from NestedFS in _context.FriendshipStatuses
                                                                            where NestedFS.RequesterId == FS.RequesterId
                                                                                & NestedFS.AddresseeId == FS.AddresseeId
                                                                            orderby NestedFS.SpecifiedDateTime descending
                                                                            select NestedFS.SpecifiedDateTime)
                                                                            .FirstOrDefault()
                                             select new
                                             {
                                                 Reqter.NickName,
                                                 FS.StatusCode,
                                             }).ToListAsync();
            return friendShip;
        }

        public async Task<string> AddFriend(string userId, string friendId)
        {
            try
            {
                // Check if the two user is in relation ship
                // The user is the requester and this request is declined
                // Update the statuscode of this friendship to Requested again
                var friendshipAsRequester = await (from FS in _context.FriendshipStatuses
                                                   join Reqter in _context.UserAccounts on FS.RequesterId equals Reqter.UserId
                                                   join Adrsee in _context.UserAccounts on FS.AddresseeId equals Adrsee.UserId
                                                   where FS.RequesterId == userId
                                                       & FS.AddresseeId == friendId
                                                       & FS.SpecifiedDateTime == (from NestedFS in _context.FriendshipStatuses
                                                                                  where NestedFS.RequesterId == FS.RequesterId
                                                                                        & NestedFS.AddresseeId == FS.AddresseeId
                                                                                  orderby NestedFS.SpecifiedDateTime descending
                                                                                  select NestedFS.SpecifiedDateTime)
                                                                                  .FirstOrDefault()
                                                   select new
                                                   {
                                                       Adrsee.NickName,
                                                       FS.StatusCode,
                                                   }).FirstOrDefaultAsync();

                // Check if the two user is in relation ship
                // The user is the adressee and this request is declined
                // Update the statuscode of this friendship to Requested again
                var friendshipAsAdressee = await (from FS in _context.FriendshipStatuses
                                                  join Reqter in _context.UserAccounts on FS.RequesterId equals Reqter.UserId
                                                  join Adrsee in _context.UserAccounts on FS.AddresseeId equals Adrsee.UserId
                                                  where FS.AddresseeId == userId
                                                      & FS.RequesterId == friendId
                                                      & FS.SpecifiedDateTime == (from NestedFS in _context.FriendshipStatuses
                                                                                 where NestedFS.RequesterId == FS.RequesterId
                                                                                     & NestedFS.AddresseeId == FS.AddresseeId
                                                                                 orderby NestedFS.SpecifiedDateTime descending
                                                                                 select NestedFS.SpecifiedDateTime)
                                                                                 .FirstOrDefault()
                                                  select new
                                                  {
                                                      Reqter.NickName,
                                                      FS.StatusCode,
                                                  }).FirstOrDefaultAsync();

                if (friendshipAsRequester == null && friendshipAsAdressee == null)
                {
                    var Friendship = new Friendship()
                    {
                        RequesterId = userId,
                        AddresseeId = friendId,
                        CreateAt = DateTime.Now,
                    };
                    var FriendShipStatus = new FriendshipStatus()
                    {
                        RequesterId = userId,
                        AddresseeId = friendId,
                        SpecifiedDateTime = DateTime.Now,
                        StatusCode = "Requested",
                        SpecifierId = userId,
                    };
                    _context.Friendships.Add(Friendship);
                    _context.FriendshipStatuses.Add(FriendShipStatus);
                    await _context.SaveChangesAsync();
                    return SUCCESS;
                }

                // the user is in relation ship before as requester or adressee
                else
                {
                    // if they user in friendship but any of them declined then request to add friend again
                    if (friendshipAsRequester != null && friendshipAsRequester.StatusCode.Equals("Declined"))
                    {
                        try
                        {
                            var newFriendshipStatus = new FriendshipStatus()
                            {
                                RequesterId = userId,
                                AddresseeId = friendId,
                                SpecifiedDateTime = DateTime.Now,
                                StatusCode = "Requested",
                                SpecifierId = userId,
                            };
                            _context.FriendshipStatuses.Add(newFriendshipStatus);
                            await _context.SaveChangesAsync();
                            return SUCCESS;
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                    else if (friendshipAsRequester != null && friendshipAsRequester.StatusCode.Equals("Accepted"))
                    {
                        return "this person is your friend already";
                    }
                    else if (friendshipAsRequester != null && friendshipAsRequester.StatusCode.Equals("Blocked"))
                    {
                        return "this person Blocked you";
                    }

                    // if they user in friendship but any of them declined then request to add friend again
                    if (friendshipAsAdressee != null && friendshipAsAdressee.StatusCode.Equals("Declined"))
                    {
                        try
                        {
                            var newFriendshipStatus = new FriendshipStatus()
                            {
                                RequesterId = friendId,
                                AddresseeId = userId,
                                SpecifiedDateTime = DateTime.Now,
                                StatusCode = "Requested",
                                SpecifierId = userId,
                            };
                            _context.FriendshipStatuses.Add(newFriendshipStatus);
                            await _context.SaveChangesAsync();
                            return SUCCESS;
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                    else if (friendshipAsAdressee != null && friendshipAsAdressee.StatusCode.Equals("Accepted"))
                    {
                        return "this person is your friend already";
                    }
                    else if (friendshipAsAdressee != null && friendshipAsAdressee.StatusCode.Equals("Blocked"))
                    {
                        return "this person Blocked you";
                    }
                }
                return "Add Friend Failed";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }


        public async Task<string> UpdateFriendShipStatus(string userId, string friendId, string statusCode)
        {
            try
            {
                // Check if the two user is in relation ship
                // The user is the requester and this request is declined
                // Update the statuscode of this friendship to Requested again
                var friendshipAsRequester = await (from FS in _context.FriendshipStatuses
                                                   join Reqter in _context.UserAccounts on FS.RequesterId equals Reqter.UserId
                                                   join Adrsee in _context.UserAccounts on FS.AddresseeId equals Adrsee.UserId
                                                   where FS.RequesterId == userId
                                                       & FS.AddresseeId == friendId
                                                       & FS.SpecifiedDateTime == (from NestedFS in _context.FriendshipStatuses
                                                                                  where NestedFS.RequesterId == FS.RequesterId
                                                                                        & NestedFS.AddresseeId == FS.AddresseeId
                                                                                  orderby NestedFS.SpecifiedDateTime descending
                                                                                  select NestedFS.SpecifiedDateTime)
                                                                                  .FirstOrDefault()
                                                   select new
                                                   {
                                                       Adrsee.NickName,
                                                       FS.StatusCode,
                                                   }).FirstOrDefaultAsync();

                // Check if the two user is in relation ship
                // The user is the adressee and this request is declined
                // Update the statuscode of this friendship to Requested again
                var friendshipAsAdressee = await (from FS in _context.FriendshipStatuses
                                                  join Reqter in _context.UserAccounts on FS.RequesterId equals Reqter.UserId
                                                  join Adrsee in _context.UserAccounts on FS.AddresseeId equals Adrsee.UserId
                                                  where FS.AddresseeId == userId
                                                      & FS.RequesterId == friendId
                                                      & FS.SpecifiedDateTime == (from NestedFS in _context.FriendshipStatuses
                                                                                 where NestedFS.RequesterId == FS.RequesterId
                                                                                     & NestedFS.AddresseeId == FS.AddresseeId
                                                                                 orderby NestedFS.SpecifiedDateTime descending
                                                                                 select NestedFS.SpecifiedDateTime)
                                                                                 .FirstOrDefault()
                                                  select new
                                                  {
                                                      Reqter.NickName,
                                                      FS.StatusCode,
                                                  }).FirstOrDefaultAsync();

                if (friendshipAsRequester == null && friendshipAsAdressee == null)
                {

                    return "Can not found this relation ship";
                }
                else
                {
                    // if they user in friendship but any of them declined then request to add friend again
                    if (friendshipAsRequester != null)
                    {
                        try
                        {
                            var newFriendshipStatus = new FriendshipStatus()
                            {
                                RequesterId = userId,
                                AddresseeId = friendId,
                                SpecifiedDateTime = DateTime.Now,
                                StatusCode = statusCode,
                                SpecifierId = userId,
                            };
                            _context.FriendshipStatuses.Add(newFriendshipStatus);
                            await _context.SaveChangesAsync();
                            return SUCCESS;
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                    else if (friendshipAsRequester != null && friendshipAsRequester.StatusCode.Equals("Accepted") && statusCode.Equals("Accepted"))
                    {
                        return "this person is your friend already";
                    }
                    else if (friendshipAsRequester != null && friendshipAsRequester.StatusCode.Equals("Blocked"))
                    {
                        return "this person Blocked you";
                    }

                    // if they user in friendship but any of them declined then request to add friend again
                    if (friendshipAsAdressee != null && friendshipAsAdressee.StatusCode.Equals("Declined"))
                    {
                        try
                        {
                            var newFriendshipStatus = new FriendshipStatus()
                            {
                                RequesterId = friendId,
                                AddresseeId = userId,
                                SpecifiedDateTime = DateTime.Now,
                                StatusCode = "Requested",
                                SpecifierId = userId,
                            };
                            _context.FriendshipStatuses.Add(newFriendshipStatus);
                            await _context.SaveChangesAsync();
                            return SUCCESS;
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                    else if (friendshipAsAdressee != null && friendshipAsAdressee.StatusCode.Equals("Accepted") && statusCode.Equals("Accepted"))
                    {
                        return "this person is your friend already";
                    }
                    else if (friendshipAsAdressee != null && friendshipAsAdressee.StatusCode.Equals("Blocked"))
                    {
                        return "this person Blocked you";
                    }
                }
                return "Update friendship status failed";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }





    }
}
