using NotificationPortal.Models;
using NotificationPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NotificationPortal.Repositories
{
    public class StatusRepo
    {
        const string APP_STATUS_TYPE_NAME = "Status";
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public IEnumerable<ApplicationListVM> Sort(IEnumerable<ApplicationListVM> list, string sortOrder, string searchString = null)
        {

            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(c => c.ApplicationName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
             
               

                case ConstantsRepo.SORT_STATUS_BY_NAME_ASCE:
                    list = list.OrderBy(c => c.StatusName);
                    break;

                default:
                    list = list.OrderBy(c => c.StatusName);
                    break;
            }
            return list;
        }


        public IEnumerable<StatusVM> GetStatusList()
        {
            IEnumerable<StatusVM> statusList = _context.Status
                                                .Select(c => new StatusVM
                                                {
                                                    
                                                    StatusID = c.StatusID,
                                                    //ClientRefID = c.Client.ReferenceID,
                                                });
            return statusList;
        }




        public bool AddStatus(StatusVM status, out string msg)
        {
            Status s = _context.Status.Where(e => e.StatusName == status.StatusName)
                            .FirstOrDefault();
        
            if (s != null)
            {
                msg = "Application name already exist.";
                return false;
            }
            try
            {
                Status newStatus = new Status();
                newStatus.StatusName = status.StatusName;
                newStatus.StatusID = status.StatusID;
                _context.Status.Add(newStatus);
                _context.SaveChanges();
                msg = "Application successfully added";
                return true;
            }
            catch
            {
                msg = "Failed to add application.";
                return false;
            }
        }

        public StatusVM GetStatus(int statusID)
        {
            StatusVM status = _context.Status
                            .Where(a => a.StatusID == statusID)
                            .Select(b => new StatusVM
                            {
                                StatusName = b.StatusName,
                                StatusTypeID = b.StatusTypeID,
                            }).FirstOrDefault();
            return status;
        }

        //public ApplicationVM GetDeleteStatus(int statusID)
        //{
        //    ApplicationVM application = _context.Application
        //                    .Where(a => a.ReferenceID == referenceID)
        //                    .Select(b => new ApplicationVM
        //                    {
        //                        ApplicationName = b.ApplicationName,
        //                        ReferenceID = b.ReferenceID,
        //                        Description = b.Description,
        //                        URL = b.URL,
        //                        StatusID = b.StatusID,
        //                        ClientRefID = b.Client.ReferenceID,
        //                        //ClientID = b.ClientID,
        //                    }).FirstOrDefault();
        //    return application;
        //}

        public bool EditStatus(StatusVM status, out string msg)
        {
            Status s = _context.Status.Where(b => b.StatusName == status.StatusName).FirstOrDefault();
           
            //if (a != null)
            //{
            //    msg = "Application name already exist.";
            //    return false;
            //}
            try
            {
                Status statusUpdated = _context.Status
                                        .Where(d => d.StatusID == status.StatusID)
                                        .FirstOrDefault();
                statusUpdated.StatusName = status.StatusName;
                statusUpdated.StatusTypeID = status.StatusTypeID;
                _context.SaveChanges();
                msg = "Application information succesfully updated.";
                return true;
            }
            catch
            {
                msg = "Failed to update application.";
                return false;
            }
        }

        public bool DeleteStatus(int statusID, out string msg)
        {
            // check if applications exists
            Status statusToBeDeleted = _context.Status
                                    .Where(a => a.StatusID == statusID)
                                    .FirstOrDefault();
            // check applications associated with Server
     
            //var statusStatusTypes = _context.StatusType
            //                         .Where(a => a.StatusTypeID == Status,)
            //                         .FirstOrDefault();
            if (statusToBeDeleted == null)
            {
                msg = "application could not be deleted.";
                return false;
            }
           

            try
            {
                _context.Status.Remove(statusToBeDeleted);
                _context.SaveChanges();
                msg = "Application Successfully Deleted";
                return true;
            }
            catch
            {
                msg = "Failed to update application.";
                return false;
            }

        }
    }
}


