using MobileBasedCashFlowAPI.Dto;
using System.Collections;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface IGameReportRepository
    {
        public Task<IEnumerable> GetAllAsync();
        public Task<object?> MyReport(int userId);
        public Task<string> CreateAsync(int userId, GameReportRequest request);
        public Task<string> DeleteAllRecord();
    }
}
