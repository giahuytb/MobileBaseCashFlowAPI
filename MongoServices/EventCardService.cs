using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.MongoDTO;
using System.Collections;
using X.PagedList;
using MongoDB.Driver.Linq;

namespace MobileBasedCashFlowAPI.MongoServices
{
    public class EventCardService : IEventCardService
    {
        public const string SUCCESS = "success";
        private readonly IMongoCollection<EventCard> _collection;
        public EventCardService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<EventCard>("Event_card");
        }

        public async Task<IEnumerable> GetAsync()
        {
            var eventCards = await _collection.Find(_ => true).ToListAsync();
            return eventCards;
        }

        public async Task<object?> GetAsync(PaginationFilter filter, double? from, double? to)
        {
            var AllEventCard = _collection.AsQueryable();
            #region Filter
            if (from.HasValue)
            {
                AllEventCard = AllEventCard.Where(evt => evt.Cost >= from);
            }
            if (to.HasValue)
            {
                AllEventCard = AllEventCard.Where(evt => evt.Cost <= to);
            }
            #endregion

            #region Paging
            var PagedData = await AllEventCard.ToPagedListAsync(filter.PageIndex, filter.PageSize);
            var TotalPage = ValidateInput.totaPage(PagedData.TotalItemCount, filter.PageSize);
            #endregion
            return new
            {
                filter.PageIndex,
                filter.PageSize,
                totalPage = TotalPage,
                data = PagedData,
            };
        }

        public async Task<EventCard?> GetAsync(string id)
        {
            var eventCard = await _collection.Find(evt => evt.id == id).FirstOrDefaultAsync();
            return eventCard;
        }

        public async Task<string> CreateAsync(EventCardRequest request)
        {
            try
            {
                var checkNameExist = _collection.Find(evt => evt.Event_name == request.Event_name).FirstOrDefaultAsync();
                if (checkNameExist.Result == null)
                {
                    return "This event card has already existed";
                }
                else if (request.Event_name == null)
                {
                    return "You need to fill name for this event card";
                }
                else if (request.Trading_range == null)
                {
                    return "You need to fill tranding range for this event card";
                }
                else if (request.Event_description == null)
                {
                    return "You need to fill description for this event card";
                }
                else if (!ValidateInput.isNumber(request.Cost.ToString()) || request.Cost < 0)
                {
                    return "Cost must be greater than or equal to 0";
                }
                else if (!ValidateInput.isNumber(request.Down_pay.ToString()) || request.Down_pay < 0)
                {
                    return "Down pay must be greater than or equal to 0";
                }
                else if (!ValidateInput.isNumber(request.Dept.ToString()) || request.Down_pay < 0)
                {
                    return "Dept must be greater than or equal to 0";
                }
                else if (!ValidateInput.isNumber(request.Cash_flow.ToString()) || request.Cash_flow < 0)
                {
                    return "Cash flow must be greater than or equal to 0";
                }
                var eventCard = new EventCard()
                {
                    Event_name = request.Event_name,
                    Image_url = request.Image_url,
                    Account_name = request.Account_Name,
                    Event_description = request.Event_description,
                    Trading_range = request.Trading_range,
                    Cost = request.Cost,
                    Down_pay = request.Down_pay,
                    Dept = request.Dept,
                    Cash_flow = request.Cash_flow,
                    Event_type_id = request.Event_type_id,
                    Action = request.Action,
                };
                await _collection.InsertOneAsync(eventCard);
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> UpdateAsync(string id, EventCardRequest request)
        {
            try
            {
                var oldEventCard = await _collection.Find(account => account.id == id).FirstOrDefaultAsync();
                if (oldEventCard != null)
                {
                    if (request.Event_name == null)
                    {
                        return "You need to fill name for this event card";
                    }
                    else if (request.Trading_range == null)
                    {
                        return "You need to fill tranding range for this event card";
                    }
                    else if (request.Event_description == null)
                    {
                        return "You need to fill description for this event card";
                    }
                    else if (!ValidateInput.isNumber(request.Cost.ToString()) || request.Cost < 0)
                    {
                        return "Cost must be greater than or equal to 0";
                    }
                    else if (!ValidateInput.isNumber(request.Down_pay.ToString()) || request.Down_pay < 0)
                    {
                        return "Down pay must be greater than or equal to 0";
                    }
                    else if (!ValidateInput.isNumber(request.Dept.ToString()) || request.Down_pay < 0)
                    {
                        return "Dept must be greater than or equal to 0";
                    }
                    else if (!ValidateInput.isNumber(request.Cash_flow.ToString()) || request.Cash_flow < 0)
                    {
                        return "Cash flow must be greater than or equal to 0";
                    }

                    oldEventCard.Event_name = request.Event_name;
                    oldEventCard.Image_url = request.Image_url;
                    oldEventCard.Account_name = request.Account_Name;
                    oldEventCard.Event_description = request.Event_description;
                    oldEventCard.Trading_range = request.Trading_range;
                    oldEventCard.Cost = request.Cost;
                    oldEventCard.Down_pay = request.Down_pay;
                    oldEventCard.Dept = request.Dept;
                    oldEventCard.Cash_flow = request.Cash_flow;
                    oldEventCard.Event_type_id = request.Event_type_id;
                    oldEventCard.Action = request.Action;

                    await _collection.ReplaceOneAsync(x => x.id == id, oldEventCard);
                    return SUCCESS;
                }
                else
                {
                    return "Can not found this event card";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> RemoveAsync(string id)
        {
            //var checkExist = _
            var result = await _collection.DeleteOneAsync(x => x.id == id);
            if (result != null)
            {
                return SUCCESS;
            }
            return "Can not found this event card";
        }


    }
}
