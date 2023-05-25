using MobileBasedCashFlowAPI.Dto;
using System.Collections;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface IParticipantRepository
    {
        public Task<IEnumerable> GetAllAsync();
        public Task<object?> GetByIdAsync(int userId, string matchId);
        public Task<int> GetTotalUserPlayGameInDay();
        public Task<string> CreateAsync(ParticipantRequest request);
        public Task<string> DeleteAsync(int userId, string matchId);
    }
}
