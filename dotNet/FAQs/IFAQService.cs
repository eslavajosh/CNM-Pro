using Sabio.Models;
using Sabio.Models.Domain.FAQs;
using Sabio.Models.Requests.FAQ;
using Sabio.Models.Requests.FAQs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services.FAQs
{
    public interface IFAQService
    {
        int AddFAQ(FAQAddRequest request, int userId);

        FAQ GetFAQById(int id);

        Paged<FAQ> GetFAQsByPage(int pageIndex, int pageSize);

        Paged<FAQ> GetFAQsByCreatedBy(int pageIndex, int pageSize, string query);

        void UpdateFAQ(FAQUpdateRequest request, int userId);

        void DeleteFAQById(int id);
    }
}
