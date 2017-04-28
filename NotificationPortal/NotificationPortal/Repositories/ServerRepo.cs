using NotificationPortal.Models;
using NotificationPortal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace NotificationPortal.Repositories
{

    public class ServerRepo : IInterfaceRepo<Server>, IDisposable
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public ServerRepo(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IQueryable<Server> GetAll()
        {
            IQueryable<Server> query = context.Server;
            return query;

        }
        public Server FindBy(int ServerID)
        {

            var query = this.GetAll().FirstOrDefault(x => x.ServerID == ServerID);
            return query;
        }
        public void Add(Server server)
        {

            context.Server.Add(server);
        }

        public void Delete(Server server)
        {

            context.Server.Remove(server);
        }

        public void Edit(Server server)
        {

            context.Entry<Server>(server).State = System.Data.Entity.EntityState.Modified;
        }
        public void Save()
        {

            context.SaveChanges();
        }

        public IEnumerable<SelectListItem> GetStatusList()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<SelectListItem> statusList = db.Status
                    .Select(app =>
                                new SelectListItem
                                {
                                    Value = app.StatusID.ToString(),
                                    Text = app.StatusName
                                });

            return new SelectList(statusList, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetLocationList()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<SelectListItem> locationList = db.DataCenterLocation
                    .Select(app =>
                                new SelectListItem
                                {
                                    Value = app.LocationID.ToString(),
                                    Text = app.Location
                                });

            return new SelectList(locationList, "Value", "Text");
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();

                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IQueryable<Server> FindBy(Expression<Func<Server, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }

}
