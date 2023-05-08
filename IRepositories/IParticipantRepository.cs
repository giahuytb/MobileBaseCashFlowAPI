using MobileBasedCashFlowAPI.Dto;
using System.Collections;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface IParticipantRepository
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(int userId, string matchId);
        public Task<string> CreateAsync(ParticipantRequest request);
        public Task<string> DeleteAsync(int userId, string matchId);
    }
}
