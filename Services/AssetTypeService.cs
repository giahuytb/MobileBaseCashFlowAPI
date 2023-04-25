
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.Repository;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class AssetTypeService : AssetTypeRepository
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public AssetTypeService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable> GetAsync()
        {
            var assetType = await (from i in _context.AssetTypes
                                   select new
                                   {
                                       i.AssetTypeId,
                                       i.AssetTypeName,
                                   }).ToListAsync();
            return assetType;
        }

        public async Task<object?> GetAsync(int assetTypeId)
        {
            var assetType = await (from i in _context.AssetTypes
                                   where i.AssetTypeId == assetTypeId
                                   select new
                                   {
                                       i.AssetTypeId,
                                       i.AssetTypeName,
                                   }).ToListAsync();
            return assetType;
        }
        public async Task<string> CreateAsync(int userId, AssetTypeRequest request)
        {
            var checkName = await _context.AssetTypes
                               .Where(d => d.AssetTypeName == request.AssetTypeName)
                               .FirstOrDefaultAsync();
            if (checkName != null)
            {
                return "This asset type name is existed";
            }

            var assetType = new AssetType()
            {
                AssetTypeName = request.AssetTypeName,
                CreateAt = DateTime.Now,
                CreateBy = userId,
            };

            await _context.AssetTypes.AddAsync(assetType);
            await _context.SaveChangesAsync();
            return Constant.Success;
        }

        public async Task<string> UpdateAsync(int assetTypeId, int userId, AssetTypeRequest request)
        {
            var AssetType = await _context.AssetTypes.Where(i => i.AssetTypeId == assetTypeId).FirstOrDefaultAsync();
            if (AssetType != null)
            {
                AssetType.AssetTypeName = request.AssetTypeName;
                AssetType.UpdateAt = DateTime.Now;
                AssetType.UpdateBy = userId;

                await _context.SaveChangesAsync();
                return Constant.Success;
            }

            return Constant.NotFound;
        }

        public async Task<string> DeleteAsync(int assetTypeId)
        {
            var AssetType = await _context.AssetTypes.Where(a => a.AssetTypeId == assetTypeId).FirstOrDefaultAsync();
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
