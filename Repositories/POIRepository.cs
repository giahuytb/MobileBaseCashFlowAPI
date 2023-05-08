using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.IRepositories;
using System.Collections;

namespace MobileBasedCashFlowAPI.Repositories
{
    public class POIRepository : IPOIRepository
    {
        public readonly MobileBasedCashFlowGameContext _context;
        public POIRepository(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAsync()
        {
            var POI = await (from p in _context.PointOfInteractions
                             select new
                             {
                                 p.PoiId,
                                 p.PoiName,
                                 p.PoiDescription,
                                 p.PoiVideoUrl,
                             }).AsNoTracking().ToListAsync();
            return POI;
        }

        public async Task<object?> GetByIdAsync(int poiId)
        {
            var POI = await (from p in _context.PointOfInteractions
                             where p.PoiId == poiId
                             select new
                             {
                                 p.PoiId,
                                 p.PoiName,
                                 p.PoiDescription,
                                 p.PoiVideoUrl,
                             }).AsNoTracking().ToListAsync();
            return POI;
        }
        public async Task<string> CreateAsync(int userId, POIRequest request)
        {
            var checkName = await _context.PointOfInteractions
                             .Where(a => a.PoiName == request.PoiName)
                             .AsNoTracking()
                             .FirstOrDefaultAsync();
            if (checkName != null)
            {
                return "This POI name is existed";
            }

            var POI = new PointOfInteraction()
            {
                PoiName = request.PoiName,
                PoiDescription = request.PoiDescription,
                PoiVideoUrl = request.PoiVideoUrl,
                GameServerId = request.GameServerId,
                CreateBy = userId,
                CreateAt = DateTime.Now,
            };

            await _context.PointOfInteractions.AddAsync(POI);
            await _context.SaveChangesAsync();
            return Constant.Success;
        }

        public async Task<string> UpdateAsync(int userId, int poiId, POIRequest request)
        {
            var oldPOI = await _context.PointOfInteractions.FirstOrDefaultAsync(p => p.PoiId == poiId);
            if (oldPOI != null)
            {
                // check if the new name is already exist in database. (except it's old name)
                var checkName = await _context.PointOfInteractions
                                        .Where(p => p.PoiName == request.PoiName && p.PoiName != oldPOI.PoiName)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync();
                if (checkName != null)
                {
                    return "This POI name is existed";
                }

                oldPOI.PoiName = request.PoiName;
                oldPOI.PoiDescription = request.PoiDescription;
                oldPOI.PoiVideoUrl = request.PoiVideoUrl;
                oldPOI.GameServerId = request.GameServerId;
                oldPOI.UpdateBy = userId;
                oldPOI.UpdateAt = DateTime.Now;

                await _context.SaveChangesAsync();
                return Constant.Success;
            }
            return "Can not find this POI";
        }

        public async Task<string> DeleteAsync(int poiId)
        {
            var POI = await _context.PointOfInteractions.Where(p => p.PoiId == poiId).FirstOrDefaultAsync();
            if (POI == null)
            {
                return "Can not find this POI";
            }
            _context.PointOfInteractions.Remove(POI);
            await _context.SaveChangesAsync();

            return Constant.Success;
        }
    }
}
