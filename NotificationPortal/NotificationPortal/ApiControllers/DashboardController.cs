using NotificationPortal.ApiModels;
using NotificationPortal.ApiRepositories;
using System.Web.Http;

namespace NotificationPortal.ApiControllers
{
    public class DashboardController : ApiController
    {
        private readonly DashboardApiRepo _dApiRepo = new DashboardApiRepo();
        // POST: api/Dashboard
        public DashboardIndexFiltered Post([FromBody] IndexBody model)
        {
            DashboardIndexFiltered result = _dApiRepo.GetFilteredAndSortedDasboard(model);
            return result;
        }
    }
}