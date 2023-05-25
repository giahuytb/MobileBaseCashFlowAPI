
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.IRepositories;
using System.Collections;

namespace MobileBasedCashFlowAPI.Repositories
{
    public class AssetTypeRepository : IAssetTypeRepository
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public AssetTypeRepository(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAllAsync()
        {
            var assetType = await (from at in _context.AssetTypes
                                   select new
                                   {
                                       at.AssetTypeId,
                                       at.AssetTypeName,
                                   }).AsNoTracking().ToListAsync();
            return assetType;
        }

        public async Task<object?> GetByIdAsync(int assetTypeId)
        {
            var assetType = await (from at in _context.AssetTypes
                                   where at.AssetTypeId == assetTypeId
                                   select new
                                   {
                                       at.AssetTypeId,
                                       at.AssetTypeName,
                                   }).AsNoTracking().ToListAsync();
            return assetType;
        }
        public async Task<string> CreateAsync(AssetTypeRequest request)
        {
            var checkName = await _context.AssetTypes
                               .Where(at => at.AssetTypeName == request.AssetTypeName)
                               .AsNoTracking()
                               .FirstOrDefaultAsync();
            if (checkName != null)
            {
                return "This asset type name is existed";
            }

            var assetType = new AssetType()
            {
                AssetTypeName = request.AssetTypeName,
            };

            await _context.AssetTypes.AddAsync(assetType);
            await _context.SaveChangesAsync();
            return Constant.Success;
        }

        public async Task<string> UpdateAsync(int assetTypeId, AssetTypeRequest request)
        {
            var oldAssetType = await _context.AssetTypes.Where(at => at.AssetTypeId == assetTypeId).FirstOrDefaultAsync();
            if (oldAssetType != null)
            {
                var checkName = await _context.AssetTypes
                        .Where(at => at.AssetTypeName == request.AssetTypeName && at.AssetTypeName != oldAssetType.AssetTypeName)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                if (checkName != null)
                {
                    return "This asset type name is existed";
                }
                oldAssetType.AssetTypeName = request.AssetTypeName;

                await _context.SaveChangesAsync();
                return Constant.Success;
            }

            return Constant.NotFound;
        }

        public async Task<string> DeleteAsync(int assetTypeId)
        {
            var AssetType = await _context.AssetTypes.Where(at => at.AssetTypeId == assetTypeId).FirstOrDefaultAsync();
            if (AssetType == null)
            {
                return "Can not find this Asset";
            }
            _context.AssetTypes.Remove(AssetType);
            await _context.SaveChangesAsync();

            return Constant.Success;
        }
    }
}
