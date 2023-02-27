using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MongoDB.Driver;

namespace MobileBasedCashFlowAPI.MongoServices
{
    public class FinancialReportService : IFinancialReportService
    {
        public const string SUCCESS = "success";
        private readonly IMongoCollection<FinancialReport> _finacial;

        public FinancialReportService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _finacial = database.GetCollection<FinancialReport>("Financial_report");
        }

        public async Task<List<FinancialReport>> GetAsync()
        {
            var result = await _finacial.Find(_ => true).ToListAsync();
            return result;
        }

        public async Task<FinancialReport?> GetAsync(string id)
        {
            var result = await _finacial.Find(fr => fr._id == id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<FinancialReport>> GetFinancialAccount(string id)
        {
            return await _finacial.Find(fr => fr._id == id).ToListAsync();
        }

        public async Task<string> CreateAsync(FinancialReport financialReport)
        {
            var finanReport = new FinancialReport()
            {
                Children_amount = financialReport.Children_amount,
                User_id = financialReport.User_id,
                Job_card_id = financialReport.User_id,
                Income_per_month = financialReport.Income_per_month,
                Expense_per_month = financialReport.Expense_per_month,
                Game_accounts = financialReport.Game_accounts,
            };
            await _finacial.InsertOneAsync(finanReport);
            return SUCCESS;
        }

        public async Task<string> RemoveAsync(string id)
        {
            await _finacial.DeleteOneAsync(x => x._id == id);
            return SUCCESS;
        }

        public async Task<string> UpdateAsync(string id, int childrenAmount, GameAccount gameAccount)
        {
            bool checkAccountExist = false;
            double salary = 0;
            double expensePerMonth = 0;
            double passiveIncome = 0;

            var oldFinanReport = await _finacial.Find(fr => fr._id == id).FirstOrDefaultAsync();

            if (oldFinanReport == null)
            {
                return "Can not find this financial report";
            }

            else
            {
                // find if game account is already exist in this financial report 
                for (int i = 0; i < oldFinanReport.Game_accounts.Count(); i++)
                {
                    // if find update it cost
                    if (oldFinanReport.Game_accounts[i].GameAccount_name == gameAccount.GameAccount_name)
                    {
                        oldFinanReport.Game_accounts[i].GameAccount_cost = gameAccount.GameAccount_cost;
                        checkAccountExist = true;
                        break;
                    }
                }
                // if not add this new account
                if (!checkAccountExist)
                {
                    oldFinanReport.Game_accounts.Add(gameAccount);
                }

                // calculation income per month and expense permonth
                for (int i = 0; i < oldFinanReport.Game_accounts.Count(); i++)
                {
                    switch (oldFinanReport.Game_accounts[i].GameAccount_type)
                    {
                        // income
                        case 0:
                            salary = oldFinanReport.Game_accounts[i].GameAccount_cost;
                            break;
                        // expense
                        case 1:
                            expensePerMonth += oldFinanReport.Game_accounts[i].GameAccount_cost;
                            break;
                        // asset
                        case 2:
                            if (!oldFinanReport.Game_accounts[i].GameAccount_name.Equals("Tiền mặt"))
                            {
                                passiveIncome += oldFinanReport.Game_accounts[i].GameAccount_cost;
                            }
                            break;
                        default:
                            break;
                    }

                }

                var finanReport = new FinancialReport()
                {
                    _id = oldFinanReport._id,
                    User_id = oldFinanReport.User_id,
                    Job_card_id = oldFinanReport.Job_card_id,
                    Children_amount = childrenAmount,
                    Income_per_month = salary + passiveIncome,
                    Expense_per_month = expensePerMonth,
                    Game_accounts = oldFinanReport.Game_accounts,
                };

                await _finacial.ReplaceOneAsync(x => x._id == id, finanReport);
                return SUCCESS;
            }
        }

    }
}
