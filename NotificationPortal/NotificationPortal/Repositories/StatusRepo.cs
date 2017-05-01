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
    public class StatusRepo : IInterfaceRepo<Status>, IDisposable
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public StatusRepo(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IQueryable<Status> GetAll()
        {
            IQueryable<Status> query = context.Status;
            return query;

        }
        public Status FindBy(int StatusID)
        {

            var query = this.GetAll().FirstOrDefault(x => x.StatusID == StatusID);
            return query;
        }
        public void Add(Status status)
        {

            context.Status.Add(status);
        }

        public void Delete(Status status)
        {

            context.Status.Remove(status);
        }

        public void Edit(Status status)
        {

            context.Entry<Status>(status).State = System.Data.Entity.EntityState.Modified;
        }
        public void Save()
        {

            context.SaveChanges();
        }

        public IEnumerable<SelectListItem> GetStatusTypeList()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<SelectListItem> statusTypeList = db.StatusType
                    .Select(app =>
                                new SelectListItem
                                {
                                    Value = app.StatusTypeID.ToString(),
                                    Text = app.StatusTypeName
                                });

            return new SelectList(statusTypeList, "Value", "Text");
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

        public IQueryable<Status> FindBy(Expression<Func<Status, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }

}

