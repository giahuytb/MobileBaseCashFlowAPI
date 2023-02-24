
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

        public async Task<object?> GetAsync(string searchBy, string id)
        {
            try
            {

                var AllInventory = await (from invent in _context.Inventories
                                          join user in _context.UserAccounts on invent.UserId equals user.UserId
                                          join item in _context.Items on invent.ItemId equals item.ItemId
                                          select new
                                          {
                                              user.UserId,
                                              user.UserName,
                                              invent.ItemId,
                                              item.ItemName,
                                              invent.CreateAt,
                                          }).ToListAsync();

                if (searchBy.Equals("user"))
                {
                    AllInventory = AllInventory.Where(i => i.UserId == id).ToList();
                }
                else if (searchBy.Equals("item"))
                {
                    AllInventory = AllInventory.Where(i => i.ItemId == id).ToList();
                }
                else
                {
                    return "Your searchBy field must be user or item";
                }
                return AllInventory;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public async Task<string> CreateAsync(string itemId, string userId)
        {
            // check if this item has been purchased by the user 
            var check = await _context.Inventories.FirstOrDefaultAsync(i => i.UserId == userId && i.ItemId == itemId);
            if (check != null)
            {
                return "You already bought this item";
            }
            try
            {
                var invent = new Inventory()
                {
                    ItemId = itemId,
                    UserId = userId,
                    CreateAt = DateTime.Now,
                };
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
