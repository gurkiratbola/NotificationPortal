using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NotificationPortal.Api
{
    public class NotificationController : ApiController
    {
        // GET: api/Notification
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Notification/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Notification
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Notification/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Notification/5
        public void Delete(int id)
        {
        }
    }
}
