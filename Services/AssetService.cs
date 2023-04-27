
using Microsoft.EntityFrameworkCore;
using System.Collections;

using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.Repository;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.DTO;
using Org.BouncyCastle.Asn1.Ocsp;

namespace MobileBasedCashFlowAPI.Services
{
    public class AssetService : AssetRepository
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public AssetService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAsync(int userId)
        {
            var asset = await (from i in _context.Assets
                               join userAsset in _context.UserAssets on i.AssetId equals userAsset.AssetId
                               into g
                               from userAsset in g.DefaultIfEmpty()
                               where userAsset.UserId != userId
                               select new
                               {
                                   i.AssetId,
                                   i.AssetName,
                                   i.ImageUrl,
                                   i.AssetPrice,
                                   i.Description,
                                   i.IsInShop,
                                   i.CreateBy,
                                   i.AssetType,
                               }).ToListAsync();
            return asset;
        }

        public async Task<Object?> GetByIdAsync(int id)
        {
            var asset = await _context.Assets.Select(i => new
            {
                i.AssetId,
                i.AssetName,
                i.ImageUrl,
                i.AssetPrice,
                i.Description,
                i.IsInShop,
                i.CreateBy,
                i.AssetType,
            }).Where(i => i.AssetId == id).AsNoTracking().FirstOrDefaultAsync();
            return asset;
        }

        public async Task<string> CreateAsync(int userId, AssetRequest request)
        {
            var checkName = await _context.Assets
                            .Where(d => d.AssetName == request.AssetName)
                            .Select(d => new { assetName = d.AssetName }).FirstOrDefaultAsync();
            if (checkName != null)
            {
                return "This asset name is existed";
            }
            if (request.AssetName == null)
            {
                return "You need to fill name for this Asset";
            }
            if (request.ImageUrl == null)
            {
                return "Please Select Image for this Asset";
            }
            if (request.Description == null)
            {
                return "You need to fill description for this Asset";
            }
            if (!ValidateInput.isNumber(request.AssetPrice.ToString()) || request.AssetPrice <= 0)
            {
                return "Price must be mumber and bigger than 0";
            }
            if (!ValidateInput.isNumber(request.AssetType.ToString()) || request.AssetType <= 0)
            {
                return "Asset Type must be number and bigger than 0";
            }

            var asset = new Asset()
            {
                AssetName = request.AssetName,
                ImageUrl = request.ImageUrl,
                AssetPrice = request.AssetPrice,
                Description = request.Description,
                IsInShop = request.IsInShop,
                CreateAt = DateTime.Now,
                CreateBy = userId,
                AssetType = request.AssetType,
            };

            await _context.Assets.AddAsync(asset);
            await _context.SaveChangesAsync();
            return Constant.Success;

        }

        public async Task<string> UpdateAsync(int assetId, int userId, AssetRequest request)
        {
            var oldAsset = await _context.Assets.FirstOrDefaultAsync(i => assetId == i.AssetId);
            if (oldAsset != null)
            {
                if (request.AssetName == null)
                {
                    return "You need to fill name for this Asset";
                }
                if (request.ImageUrl == null)
                {
                    return "Please Select Image for this Asset";
                }
                if (request.Description == null)
                {
                    return "You need to fill description for this Asset";
                }
                if (!ValidateInput.isNumber(request.AssetPrice.ToString()) || request.AssetPrice <= 0)
                {
                    return "Price must be mumber and bigger than 0";
                }
                if (!ValidateInput.isNumber(request.AssetType.ToString()) || request.AssetPrice <= 0)
                {
                    return "Asset Type must be 1 - 127";
                }

                oldAsset.AssetName = request.AssetName;
                oldAsset.ImageUrl = request.ImageUrl;
                oldAsset.AssetPrice = request.AssetPrice;
                oldAsset.Description = request.Description;
                oldAsset.IsInShop = request.IsInShop;
                oldAsset.CreateAt = oldAsset.CreateAt;
                oldAsset.CreateBy = oldAsset.CreateBy;
                oldAsset.UpdateAt = DateTime.Now;
                oldAsset.UpdateBy = userId;
                oldAsset.AssetType = request.AssetType;

                await _context.SaveChangesAsync();
                return Constant.Success;
            }
            return "Can not find this Asset";
        }

        public async Task<string> DeleteAsync(int AssetId)
        {
            var Asset = await _context.Assets.Where(a => a.AssetId == AssetId).FirstOrDefaultAsync();
            if (Asset == null)
            {
                return "Can not find this Asset";
            }
            _context.Assets.Remove(Asset);
            await _context.SaveChangesAsync();

            return Constant.Success;
        }

    }
}
