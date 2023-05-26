
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Utils;
using MobileBasedCashFlowAPI.IRepositories;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Repositories
{
    public class UserAssetRepository : IUserAssetRepository
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public UserAssetRepository(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAllAsync()
        {
            var inventory = await (from userAsset in _context.UserAssets
                                   join user in _context.UserAccounts on userAsset.UserId equals user.UserId
                                   join asset in _context.Assets on userAsset.AssetId equals asset.AssetId
                                   select new
                                   {
                                       user.UserName,
                                       asset.AssetName,
                                       userAsset.CreateAt,
                                       userAsset.LastUsed,
                                   }).AsNoTracking().ToListAsync();
            return inventory;
        }

        public async Task<object?> GetByIdAsync(int userId)
        {
            var inventory = await (from userAsset in _context.UserAssets
                                   join user in _context.UserAccounts on userAsset.UserId equals user.UserId
                                   join asset in _context.Assets on userAsset.AssetId equals asset.AssetId
                                   where user.UserId == userId
                                   select new
                                   {
                                       user.UserName,
                                       asset.AssetName,
                                       userAsset.CreateAt,
                                       userAsset.LastUsed,
                                   }).AsNoTracking().ToListAsync();
            return inventory;
        }


        public async Task<string> CreateAsync(int assetId, int userId)
        {
            var asset = await _context.Assets.Where(i => i.AssetId == assetId).FirstOrDefaultAsync();
            if (asset == null)
            {
                return "Can not found this asset";
            }
            // check if this asset has been purchased by the user 
            var check = await _context.UserAssets.FirstOrDefaultAsync(i => i.UserId == userId && i.AssetId == assetId);
            if (check != null)
            {
                return "You already bought this asset";
            }
            var user = await _context.UserAccounts.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return "Can not found this user";
            }
            if (user.Coin < asset.AssetPrice)
            {
                return "You don't have enough coin to buy this asset";
            }
            var invent = new UserAsset()
            {
                AssetId = assetId,
                UserId = userId,
                CreateAt = DateTime.Now,
            };

            user.Coin = user.Coin - asset.AssetPrice;
            await _context.UserAssets.AddAsync(invent);
            await _context.SaveChangesAsync();
            return Constant.Success;

        }

        public async Task<string> UpdateLastUsedAsync(int assetId, int userId)
        {
            var userAsset = await _context.UserAssets.Where(i => i.UserId == assetId && i.AssetId == assetId).FirstOrDefaultAsync();
            if (userAsset != null)
            {
                userAsset.LastUsed = DateTime.Now;
                await _context.SaveChangesAsync();
                return Constant.Success;
            }
            return Constant.NotFound;
        }
    }
}
