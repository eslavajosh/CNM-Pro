using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.FAQs;
using Sabio.Models.Requests.FAQ;
using Sabio.Models.Requests.FAQs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services.FAQs
{
    public class FAQService : IFAQService
    {
        IDataProvider _data = null; 

        public FAQService(IDataProvider data)
        {
            _data = data;
        }

        public int AddFAQ(FAQAddRequest request, int userId)
        {
            int id = 0;

            string procName = "[dbo].[FAQs_Insert]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(request, col, userId);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    col.Add(idOut);
                },
            returnParameters: delegate (SqlParameterCollection returnCollection)
            {

                object oId = returnCollection["@Id"].Value;

                int.TryParse(oId.ToString(), out id);

            });
            return id;
        }

        public FAQ GetFAQById(int id)
        {
            string procName = "[dbo].[FAQs_Select_ById]";

            FAQ faq = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            },
            delegate (IDataReader reader, short set)
            {
                int startingIdex = 0;
                faq = MapSingleFAQ(reader, ref startingIdex);

            });
            return faq;
        }

        public Paged<FAQ> GetFAQsByPage(int pageIndex, int pageSize)
        {
            Paged<FAQ> pagedList = null;

            List<FAQ> list = null;

            int totalCount = 0;

            string procName = "[dbo].[FAQs_SelectAll]";

            _data.ExecuteCmd
                (procName, inputParamMapper: delegate (SqlParameterCollection param)
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);

                },
                delegate (IDataReader reader, short set)
                {
                    int startingIdex = 0;
                    FAQ faq = MapSingleFAQ(reader, ref startingIdex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIdex++);
                    }

                    if (list == null)
                    {
                        list = new List<FAQ>();
                    }
                    list.Add(faq);
                });
            if (list != null)
            {
                pagedList = new Paged<FAQ>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<FAQ> GetFAQsByCreatedBy(int pageIndex, int pageSize, string query)
        {
            Paged<FAQ> pagedList = null;

            List<FAQ> list = null;

            int totalCount = 0;

            string procName = "[dbo].[FAQs_Select_ByCreatedBy]";

            _data.ExecuteCmd
                (procName, inputParamMapper: delegate (SqlParameterCollection param)
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                    param.AddWithValue("@CreatedBy", query);

                },
                delegate (IDataReader reader, short set)
                {
                    int startingIdex = 0;
                    FAQ faq = MapSingleFAQ(reader, ref startingIdex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIdex++);
                    }

                    if (list == null)
                    {
                        list = new List<FAQ>();
                    }
                    list.Add(faq);
                });
            if (list != null)
            {
                pagedList = new Paged<FAQ>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public void UpdateFAQ(FAQUpdateRequest request, int userId)

        {
            string procName = "[dbo].[FAQs_Update]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(request, col, userId);

                    col.AddWithValue("@Id", request.Id);
                },
                returnParameters: null);
        }

        public void DeleteFAQById(int id)
        {
            string procName = "[dbo].[FAQs_Delete_ById]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);
                },
                returnParameters: null);
        }

        private static FAQ MapSingleFAQ(IDataReader reader, ref int startingIdex)
        {
            FAQ faq = new FAQ();
            faq.Id = reader.GetSafeInt32(startingIdex++);
            faq.Question = reader.GetSafeString(startingIdex++);
            faq.Answer = reader.GetSafeString(startingIdex++);
            faq.CategoryId = reader.GetSafeInt32(startingIdex++);
            faq.SortOrder = reader.GetSafeInt32(startingIdex++);
            faq.DateCreated = reader.GetSafeDateTime(startingIdex++);
            faq.DateModified = reader.GetSafeDateTime(startingIdex++);
            faq.CreatedBy = reader.GetSafeInt32(startingIdex++);
            faq.ModifiedBy = reader.GetSafeInt32(startingIdex++);
            return faq;
        }

        private static void AddCommonParams(FAQAddRequest request, SqlParameterCollection col, int userId)
        {
            col.AddWithValue("@Question", request.Question);
            col.AddWithValue("@Answer", request.Answer);
            col.AddWithValue("@CategoryId", request.CategoryId);
            col.AddWithValue("@SortOrder", request.SortOrder);
            col.AddWithValue("@CreatedBy", userId);
            col.AddWithValue("@ModifiedBy", userId);
        }
    }
}
