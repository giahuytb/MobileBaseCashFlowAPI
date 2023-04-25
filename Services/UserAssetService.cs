
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class UserAssetService : UserAssetRepository
    {
        public const string SUCCESS = "success";
        private readonly MobileBasedCashFlowGameContext _context;

        public UserAssetService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAsync()
        {
            try
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
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<object?> GetAsync(int id)
        {
            try
            {
                var inventory = await (from userAsset in _context.UserAssets
                                       join user in _context.UserAccounts on userAsset.UserId equals user.UserId
                                       join asset in _context.Assets on userAsset.AssetId equals asset.AssetId
                                       where user.UserId == id
                                       select new
                                       {
                                           user.UserName,
                                           asset.AssetName,
                                           userAsset.CreateAt,
                                           userAsset.LastUsed,
                                       }).AsNoTracking().ToListAsync();
                return inventory;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
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
                return "You don't have enough coin to buy this item";
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
