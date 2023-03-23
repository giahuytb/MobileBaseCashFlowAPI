using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MobileBasedCashFlowAPI.MongoDTO;
using MongoDB.Driver;
using MobileBasedCashFlowAPI.Common;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace MobileBasedCashFlowAPI.MongoServices
{
    public class FinancialReportService : IFinancialReportService
    {
        public const string SUCCESS = "success";
        private readonly IMongoCollection<FinancialReport> _collection;

        public FinancialReportService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<FinancialReport>("Financial_report");
        }

        public async Task<IEnumerable> GetAsync()
        {
            var result = await _collection.Find(_ => true).ToListAsync();
            return result;
        }

        public async Task<object?> GetAsync(int pageIndex, int pageSize)
        {
            var AllFinancialReport = await _collection.Find(_ => true).ToListAsync();
            var PagedData = await AllFinancialReport.ToPagedListAsync(pageIndex, pageSize);
            var TotalPage = ValidateInput.totaPage(PagedData.TotalItemCount, pageSize);
            return new
            {
                pageIndex,
                pageSize,
                totalPage = TotalPage,
                data = PagedData,
            };
        }

        public async Task<FinancialReport?> GetAsync(string id)
        {
            var result = await _collection.Find(fr => fr.id == id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<string> GenerateAsync(string userId, FinancialRequest request)
        {
            try
            {
                if (!ValidateInput.isNumber(request.Children_amount.ToString()) || request.Children_amount < 0)
                {
                    return "Cost must be mumber and bigger than 0";
                }
                var checkJob = await _collection.Find(fr => fr.Job_card_id == request.Job_card_id).FirstOrDefaultAsync();
                if (checkJob == null)
                {
                    return "Can not found this job card";
                }
                var finanReport = new FinancialReport()
                {
                    Children_amount = request.Children_amount,
                    User_id = userId,
                    Job_card_id = request.Job_card_id,
                    Game_accounts = request.Game_accounts,
                    Create_at = DateTime.Now,
                };
                await _collection.InsertOneAsync(finanReport);
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> CreateAsync(string id, int childrenAmount, GameAccountRequest request)
        {
            bool checkAccountExist = false;

            var oldFinanReport = await _collection.Find(fr => fr.id == id)
                .SortByDescending(d => d.Create_at)
                .FirstOrDefaultAsync();

            if (oldFinanReport == null)
            {
                return "Can not find this financial report";
            }
            if (!ValidateInput.isNumber(request.Game_account_value.ToString()) || request.Game_account_value < 0)
            {
                return "Game account cost must be a number greater than or equal to 0";
            }
            if (!ValidateInput.isNumber(childrenAmount.ToString()) || childrenAmount < 0)
            {
                return "children amount must be a number greater than or equal to 0";
            }
            else
            {
                //find if game account is already exist in this financial report
                for (int i = 0; i < oldFinanReport.Game_accounts.Count; i++)
                {
                    // if this account already existed then update it value
                    if (oldFinanReport.Game_accounts[i].Game_account_name == request.Game_account_name)
                    {
                        oldFinanReport.Game_accounts[i].Game_account_value = request.Game_account_value;
                        checkAccountExist = true;
                        break;
                    }
                }
                // if not add this new account
                if (!checkAccountExist)
                {
                    oldFinanReport.Game_accounts.Add(request);
                }

                var finanReport = new FinancialReport()
                {
                    User_id = oldFinanReport.User_id,
                    Job_card_id = oldFinanReport.Job_card_id,
                    Children_amount = childrenAmount,
                    Game_accounts = oldFinanReport.Game_accounts,
                };

                await _collection.InsertOneAsync(finanReport);
                return SUCCESS;
            }
        }

        public async Task<string> RemoveAsync(string id)
        {
            await _collection.DeleteOneAsync(x => x.id == id);
            return SUCCESS;
        }


    }
}
