using NotificationPortal.ApiModels;
using NotificationPortal.ApiRepositories;
using System.Web.Http;

namespace NotificationPortal.ApiControllers
{
    public class NotificationController : ApiController
    {
        private readonly NotificationApiRepo _nApiRepo = new NotificationApiRepo();
        
        // POST: api/Notification
        public NotificationIndexFiltered Post([FromBody] NotificationIndexBody model)
        {
            NotificationIndexFiltered result = _nApiRepo.GetFilteredAndSortedNotifications(model);
            return result;
        }
    }
}
