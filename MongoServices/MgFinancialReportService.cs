﻿using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MongoDB.Driver;

namespace MobileBasedCashFlowAPI.MongoServices
{
    public class MgFinancialReportService : IMgFinancialReportService
    {
        private readonly IMongoCollection<FinancialReportMg> _finacial;

        public MgFinancialReportService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _finacial = database.GetCollection<FinancialReportMg>("Financial_report");
        }

        public async Task<List<FinancialReportMg>> GetAsync()
        {
            return await _finacial.Find(_ => true).ToListAsync();
        }

        public async Task<FinancialReportMg?> GetAsync(string userId)
        {
            return await _finacial.Find(fr => fr.User_id == userId).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(FinancialReportMg financialReport)
        {
            await _finacial.InsertOneAsync(financialReport);
        }

        public async Task RemoveAsync(string id)
        {
            await _finacial.DeleteOneAsync(x => x._id == id);
        }

        public async Task UpdateAsync(string id, FinancialReportMg financialReport)
        {
            await _finacial.ReplaceOneAsync(x => x._id == id, financialReport);
        }
    }
}