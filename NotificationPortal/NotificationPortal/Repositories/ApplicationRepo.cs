using NotificationPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NotificationPortal.Repositories
{
    public class ApplicationRepo : IInterfaceRepo<Application>, IDisposable
    {    
            ApplicationDbContext context = new ApplicationDbContext();
            public ApplicationRepo(ApplicationDbContext context)
            {
                this.context = context;
            }
            public IQueryable<Application> GetAll()
            {
                IQueryable<Application> query = context.Application;
                return query;

            }
            public Application FindBy(int ApplicationID)
            {

                var query = this.GetAll().FirstOrDefault(x => x.ApplicationID == ApplicationID);
                return query;
            }
            public void Add(Application application)
            {

                context.Application.Add(application);
            }

            public void Delete(Application application)
            {

                context.Application.Remove(application);
            }

            public void Edit(Application application)
            {

                context.Entry<Application>(application).State = System.Data.Entity.EntityState.Modified;
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

        public IEnumerable<SelectListItem> GetClientList()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<SelectListItem> clientList = db.Client
                    .Select(app =>
                                new SelectListItem
                                {
                                    Value = app.ClientID.ToString(),
                                    Text = app.ClientName
                                });

            return new SelectList(clientList, "Value", "Text");
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

            public IQueryable<Application> FindBy(Expression<Func<Application, bool>> predicate)
            {
                throw new NotImplementedException();
            }
        }
    }

