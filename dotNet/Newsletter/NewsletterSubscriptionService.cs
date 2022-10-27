using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.Newsletters;
using Sabio.Models.Requests.Newsletters;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class NewsletterSubscriptionService : INewsletterSubscriptionService
    {
        IDataProvider _data = null;
        public NewsletterSubscriptionService(IDataProvider data)
        {
            _data = data;
        }

        public void Add(NewsletterSubscriptionAddRequest model)
        {
            string procName = "[dbo].[NewsletterSubscriptions_Insert]";
            
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Email", model.Email);
                    col.AddWithValue("@isSubscribed", model.IsSubscribed);

                }, returnParameters: null);
        }

        public void Update(NewsletterSubscriptionUpdateRequest model)
        {

            string procName = "[dbo].[NewsletterSubscriptions_Update]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Email", model.Email);
                    col.AddWithValue("@isSubscribed", model.IsSubscribed);
                }, returnParameters: null);
        }

        public List<NewsletterSubscription> GetAllSubscribed()
        {
            List<NewsletterSubscription> list = null;
            string procName = "[dbo].[NewsletterSubscriptions_SelectAll_Subscribed]";

            _data.ExecuteCmd(procName, inputParamMapper: null
            ,singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                NewsletterSubscription aNewsSub = MapNewsletterSubscription(reader, ref index);

                if (list == null)
                {
                    list = new List<NewsletterSubscription>();
                }

                list.Add(aNewsSub);
            });
            return list;
        }
        public Paged<NewsletterSubscription> GetAllByPage(int pageIndex, int pageSize)
        {
            Paged<NewsletterSubscription> pagedResult = null;

            List<NewsletterSubscription> result = null;

            string procName = "dbo.NewsletterSubscriptions_SelectAll";

            int totalCount = 0;

            _data.ExecuteCmd(
                procName,
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {

                    int index = 0;
                    NewsletterSubscription model = MapNewsletterSubscription(reader, ref index );

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index);
                    }

                    if (result == null)
                    {
                        result = new List<NewsletterSubscription>();
                    }

                    result.Add(model);
                }

            );
            if (result != null)
            {
                pagedResult = new Paged<NewsletterSubscription>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }

        private static NewsletterSubscription MapNewsletterSubscription(IDataReader reader, ref int startingIndex)
        {
            NewsletterSubscription aNewsSub = new NewsletterSubscription();

            aNewsSub.Email = reader.GetSafeString(startingIndex++);
            aNewsSub.IsSubscribed = reader.GetSafeBool(startingIndex++);
            
            return aNewsSub;
        }
    }
}
