using NotificationPortal.Models;
using NotificationPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.SqlClient;
using PagedList;

namespace NotificationPortal.Repositories
{
    public class StatusRepo
    {
        const string APP_STATUS_TYPE_NAME = "Status";
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public IEnumerable<StatusVM> Sort(IEnumerable<StatusVM> list, string sortOrder, string searchString = null)
        {

            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(c => c.StatusTypeName.ToUpper().Contains(searchString.ToUpper()) ||
                                       c.StatusName.ToUpper().Contains(searchString.ToUpper())
                                    );
            }
            switch (sortOrder)
            {
                case ConstantsRepo.SORT_STATUS_BY_NAME_ASCE:
                    list = list.OrderBy(c => c.StatusName);
                    break;

                case ConstantsRepo.SORT_STATUS_BY_NAME_DESC:
                    list = list.OrderByDescending(c => c.StatusName);
                    break;

                case ConstantsRepo.SORT_STATUS_BY_TYPE_ASCE:
                    list = list.OrderBy(c => c.StatusTypeName);
                    break;

                case ConstantsRepo.SORT_STATUS_BY_TYPE_DESC:
                    list = list.OrderByDescending(c => c.StatusTypeName);
                    break;

                default:
                    list = list.OrderBy(c => c.StatusTypeName);
                    break;
            }
            return list;
        }

        public StatusIndexVM GetStatusList(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                IEnumerable<StatusVM> statusList = _context.Status
                                                    .Select(c => new StatusVM
                                                    {
                                                        StatusName = c.StatusName,
                                                        StatusTypeID = c.StatusTypeID,
                                                        StatusID = c.StatusID,
                                                        StatusTypeName = c.StatusType.StatusTypeName
                                                    });
                int totalNumOfStatuses = statusList.Count();
                page = searchString == null ? page : 1;
                int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
                searchString = searchString ?? currentFilter;
                int pageNumber = (page ?? 1);
                int defaultPageSize = ConstantsRepo.PAGE_SIZE;
                sortOrder = sortOrder == null ? ConstantsRepo.SORT_STATUS_BY_TYPE_DESC : sortOrder;
                StatusIndexVM model = new StatusIndexVM
                {
                    Statuses = Sort(statusList, sortOrder, searchString).ToPagedList(pageNumber, defaultPageSize),
                    CurrentFilter = searchString,
                    CurrentSort = sortOrder,
                    TotalItemCount = totalNumOfStatuses,
                    ItemStart = currentPageIndex * defaultPageSize + 1,
                    ItemEnd = totalNumOfStatuses - (defaultPageSize * currentPageIndex) >= defaultPageSize ? defaultPageSize * (currentPageIndex + 1) : totalNumOfStatuses,
                    StatusTypeSort = sortOrder == ConstantsRepo.SORT_STATUS_BY_TYPE_DESC ? ConstantsRepo.SORT_STATUS_BY_TYPE_ASCE : ConstantsRepo.SORT_STATUS_BY_TYPE_DESC,
                    StatusNameSort = sortOrder == ConstantsRepo.SORT_STATUS_BY_NAME_DESC ? ConstantsRepo.SORT_STATUS_BY_NAME_ASCE : ConstantsRepo.SORT_STATUS_BY_NAME_DESC,
                };
                return model;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public bool AddStatus(StatusVM status, out string msg)
        {
            Status s = _context.Status.Where(e => e.StatusName == status.StatusName)
                            .FirstOrDefault();
        
            if (s != null)
            {
                msg = "Status name already exist.";
                return false;
            }
            try
            {
                Status newStatus = new Status();
                newStatus.StatusName = status.StatusName;
                newStatus.StatusTypeID = status.StatusTypeID;
   
                _context.Status.Add(newStatus);
                _context.SaveChanges();
                msg = "Status successfully created";
                return true;
            }
            catch (SqlException)
            {
                msg = "Failed to add status.";
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
                                StatusID = b.StatusID,
                                StatusTypeName = b.StatusType.StatusTypeName
                            }).FirstOrDefault();
            return status;
        }

        public bool EditStatus(StatusVM status, out string msg)
        {
            Status s = _context.Status
                        .Where(a => a.StatusTypeID == status.StatusTypeID && a.StatusName == status.StatusName)
                        .FirstOrDefault();

            if (s != null)
            {
                if (s.StatusID != status.StatusID)
                {
                    msg = "Status already exist for this status type.";
                    return false;
                }
            }
            try
            {
                Status statusUpdated = _context.Status
                                        .Where(d => d.StatusID == status.StatusID)
                                        .FirstOrDefault();
                statusUpdated.StatusName = status.StatusName;
                statusUpdated.StatusTypeID = status.StatusTypeID;
                
                _context.SaveChanges();
                msg = "Status succesfully updated.";
                return true;
            }
            catch
            {
                msg = "Failed to update status.";
                return false;
            }
        }

        public bool DeleteStatus(int statusID, out string msg)
        {
            Status statusToBeDeleted = _context.Status
                                    .Where(a => a.StatusID == statusID)
                                    .FirstOrDefault();

            if (statusToBeDeleted == null)
            {
                msg = "Status could not be deleted.";
                return false;
            }
          
            try
            {
                _context.Status.Remove(statusToBeDeleted);
                _context.SaveChanges();
                msg = "Status Successfully Deleted";
                return true;
            }
            catch
            {
                msg = "Failed to update status.";
                return false;
            }
        }
    }
}