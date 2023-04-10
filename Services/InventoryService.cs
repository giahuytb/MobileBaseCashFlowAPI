
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.MongoModels;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class InventoryService : IInventoryService
    {
        public const string SUCCESS = "success";
        private readonly MobileBasedCashFlowGameContext _context;

        public InventoryService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAsync()
        {
            try
            {
                var inventory = await (from invent in _context.Inventories
                                       join user in _context.UserAccounts on invent.UserId equals user.UserId
                                       join item in _context.Items on invent.ItemId equals item.ItemId
                                       select new
                                       {
                                           user.UserName,
                                           item.ItemName,
                                           invent.CreateAt,
                                       }).ToListAsync();
                return inventory;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<object?> GetAsync(string id)
        {
            try
            {

                var AllInventory = await (from invent in _context.Inventories
                                          join user in _context.UserAccounts on invent.UserId equals user.UserId
                                          join item in _context.Items on invent.ItemId equals item.ItemId
                                          where user.UserId == id
                                          select new
                                          {
                                              user.UserId,
                                              user.UserName,
                                              invent.ItemId,
                                              item.ItemName,
                                              invent.CreateAt,
                                          }).ToListAsync();
                return AllInventory;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public async Task<string> CreateAsync(string itemId, string userId)
        {
            var item = await _context.Items.Where(i => i.ItemId == itemId).FirstOrDefaultAsync();
            if (item == null)
            {
                return "Can not Found this Item";
            }
            // check if this item has been purchased by the user 
            var check = await _context.Inventories.FirstOrDefaultAsync(i => i.UserId == userId && i.ItemId == itemId);
            if (check != null)
            {
                return "You already bought this item";
            }
            var user = await _context.UserAccounts.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return "Can not found this user";
            }
            if (user.Coin < item.ItemPrice)
            {
                return "You don't have enough coin to buy this item";
            }
            try
            {
                var invent = new Inventory()
                {
                    ItemId = itemId,
                    UserId = userId,
                    CreateAt = DateTime.Now,
                };
                user.Coin = user.Coin - item.ItemPrice;
                _context.Inventories.Add(invent);
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
