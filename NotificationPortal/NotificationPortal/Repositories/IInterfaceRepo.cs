using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationPortal.Repositories
{
   
        public interface IInterfaceRepo<T> where T : class
        {

            IQueryable<T> GetAll();
            //IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
            T FindBy(int Id);
            void Add(T entity);
            void Delete(T entity);
            void Edit(T entity);
            void Save();
            void Dispose();
       
        }
}
