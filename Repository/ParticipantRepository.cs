using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.Repository
{
    public interface ParticipantRepository
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(int userId, int matchId);
        public Task<string> CreateAsync(ParticipantRequest request);
        public Task<string> DeleteAsync(int userId, int matchId);
    }
}
