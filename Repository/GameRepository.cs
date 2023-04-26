﻿using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.Repository
{
    public interface GameRepository
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(int gameId);
        public Task<string> CreateAsync(int userId, GameRequest request);
        public Task<string> UpdateAsync(int gameId, int userId, GameRequest request);
        public Task<string> DeleteAsync(int gameId);
    }
}
