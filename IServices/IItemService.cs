using MobieBasedCashFlowAPI.Models;
using MobieBasedCashFlowAPI.ViewModels;
using System.Collections;

namespace MobieBasedCashFlowAPI.IServices
{
    public interface IItemService
    {
        public Task<IEnumerable> GetAsync();
        public Task<Object?> GetAsync(string name);
        public Task<string> CreateAsync(string userId, itemRequest item);
        public Task<string> UpdateAsync(string ItemId, string userId, itemRequest item);
        public Task<string> DeleteAsync(string ItemId);
    }
}
