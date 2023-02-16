
using Microsoft.EntityFrameworkCore;
using MobieBasedCashFlowAPI.Common;
using MobieBasedCashFlowAPI.IServices;
using MobieBasedCashFlowAPI.Models;
using MobieBasedCashFlowAPI.ViewModels;
using System.Collections;

namespace MobieBasedCashFlowAPI.Services
{
    public class ItemService : IItemService
    {
        public const string SUCCESS = "success";
        public const string FAILED = "failed";
        public const string NOTFOUND = "not found";
        private readonly MobileBasedCashFlowGameContext _context;

        public ItemService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAsync()
        {
            var item = await (from i in _context.Items
                              select new
                              {
                                  itemId = i.ItemId,
                                  itemName = i.ItemName,
                                  itemImageUrl = i.ItemImageUrl,
                                  itemPrice = i.ItemPrice,
                                  description = i.Description,
                                  isInShop = i.IsInShop,
                                  createBy = i.CreateBy,
                              }).ToListAsync();
            return item;
        }

        public async Task<Object?> GetAsync(string name)
        {
            var item = await _context.Items.Select(i => new
            {
                itemName = i.ItemName,
                itemImageUrl = i.ItemImageUrl,
                itemPrice = i.ItemPrice,
                description = i.Description,
                isInShop = i.IsInShop,
            }).Where(i => i.itemName == name).FirstOrDefaultAsync();
            return item;
        }

        public async Task<string> CreateAsync(string userId, itemRequest item)
        {
            try
            {
                var item1 = new Item()
                {
                    ItemId = Guid.NewGuid() + "",
                    ItemName = item.ItemName,
                    ItemImageUrl = item.ItemImageUrl,
                    ItemPrice = item.ItemPrice,
                    Description = item.Description,
                    IsInShop = item.IsInShop,
                    CreateAt = DateTime.Now,
                    CreateBy = userId,
                };
                var checkName = await _context.Items.Where(i => i.ItemName == item.ItemName).FirstOrDefaultAsync();
                if (checkName != null)
                {
                    return "This Item is existed";
                }
                _context.Items.Add(item1);
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        public async Task<string> UpdateAsync(string id, string userId, itemRequest item)
        {
            var oldItem = await _context.Items.FirstOrDefaultAsync(i => id == i.ItemId);
            if (oldItem != null)
            {
                try
                {
                    var checkName = await _context.Items.Where(i => i.ItemName == item.ItemName).FirstOrDefaultAsync();
                    if (checkName != null)
                    {
                        return "This Item name is existed";
                    }
                    if (!ValidateInput.isNumber(item.ItemPrice.ToString()) || item.ItemPrice <= 0)
                    {
                        return "Price must be mumber and bigger than 0";
                    }

                    oldItem.ItemName = item.ItemName;
                    oldItem.ItemImageUrl = item.ItemImageUrl;
                    oldItem.ItemPrice = item.ItemPrice;
                    oldItem.Description = item.Description;
                    oldItem.IsInShop = item.IsInShop;
                    oldItem.CreateAt = oldItem.CreateAt;
                    oldItem.CreateBy = oldItem.CreateBy;
                    oldItem.UpdateAt = DateTime.Now;
                    oldItem.UpdateBy = userId;

                    await _context.SaveChangesAsync();
                    return SUCCESS;
                }
                catch (Exception ex)
                {
                    if (!ItemExists(id))
                    {
                        return NOTFOUND;
                    }
                    return ex.ToString();
                }
            }
            return FAILED;
        }

        public async Task<string> DeleteAsync(string ItemId)
        {
            var item = await _context.Items.FindAsync(ItemId);
            if (item == null)
            {
                return NOTFOUND;
            }
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return SUCCESS;
        }

        private bool ItemExists(string id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }

    }
}
