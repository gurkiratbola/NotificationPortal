using NotificationPortal.Models;
using NotificationPortal.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NotificationPortal.Repositories
{
    public class DataCenterRepo
    {
        const string APP_STATUS_TYPE_NAME = "DataCenter";
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        // sort function for data center
        public IEnumerable<DataCenterVM> Sort(IEnumerable<DataCenterVM> list, string sortOrder, string searchString = null)
        {

            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(c => c.Location.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case ConstantsRepo.SORT_DATACENTER_BY_NAME_DESC:
                    list = list.OrderByDescending(c => c.Location);
                    break;

                case ConstantsRepo.SORT_DATACENTER_BY_NAME_ASCE:
                    list = list.OrderBy(c => c.Location);
                    break;

                default:
                    list = list.OrderBy(c => c.Location);
                    break;
            }
            return list;
        }
        // get all data centers
        public DataCenterIndexVM GetDataCenterList(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                IEnumerable<DataCenterVM> dataCenterList = _context.DataCenterLocation
                                                            .Select(c => new DataCenterVM
                                                            {
                                                                Location = c.Location,
                                                                LocationID = c.LocationID,

                                                            });
                int totalNumOfCenters = dataCenterList.Count();
                page = searchString == null ? page : 1;
                int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
                searchString = searchString ?? currentFilter;
                int pageNumber = (page ?? 1);
                int defaultPageSize = ConstantsRepo.PAGE_SIZE;
                sortOrder = sortOrder == null ? ConstantsRepo.SORT_DATACENTER_BY_NAME_ASCE : sortOrder;
                DataCenterIndexVM model = new DataCenterIndexVM
                {
                    DataCenters = Sort(dataCenterList, sortOrder, searchString).ToPagedList(pageNumber, defaultPageSize),
                    LocationSort = sortOrder == ConstantsRepo.SORT_DATACENTER_BY_NAME_DESC ? ConstantsRepo.SORT_DATACENTER_BY_NAME_ASCE : ConstantsRepo.SORT_DATACENTER_BY_NAME_DESC,
                    CurrentFilter = searchString,
                    CurrentSort = sortOrder,
                    TotalItemCount = totalNumOfCenters,
                    ItemStart = currentPageIndex * defaultPageSize + 1,
                    ItemEnd = totalNumOfCenters - (defaultPageSize * currentPageIndex) >= defaultPageSize ? defaultPageSize * (currentPageIndex + 1) : totalNumOfCenters,
                };
                return model;
            }
            catch (Exception)
            {
                return null;
            }
        }
        // create data center
        public bool AddDataCenter(DataCenterVM dataCenter, out string msg)
        {
            DataCenterLocation d = _context.DataCenterLocation.Where(a => a.Location == dataCenter.Location)
                            .FirstOrDefault();
            if (d != null)
            {
                msg = "Data Center name already exist.";
                return false;
            }
            try
            {
                DataCenterLocation newDataCenter = new DataCenterLocation();
                newDataCenter.Location = dataCenter.Location;
                _context.DataCenterLocation.Add(newDataCenter);
                _context.SaveChanges();
                msg = "Data Center successfully added";
                return true;
            }
            catch
            {
                msg = "Failed to add data center.";
                return false;
            }
        }
        // get data center by id
        public DataCenterVM GetDataCenter(int referenceID)
        {
            try
            {
                DataCenterVM dataCenter = _context.DataCenterLocation
                                        .Where(a => a.LocationID == referenceID)
                                        .Select(b => new DataCenterVM
                                        {
                                            Location = b.Location,
                                            LocationID = b.LocationID,

                                        }).FirstOrDefault();
                return dataCenter;
            }
            catch {
                return null;
            }
        }

        // update data center
        public bool EditDataCenter(DataCenterVM dataCenter, out string msg)
        {
            DataCenterLocation d = _context.DataCenterLocation.Where(a => a.Location == dataCenter.Location).FirstOrDefault();
            if (d != null)
            {
                if (d.LocationID != dataCenter.LocationID)
                {
                    msg = "Data Center name already exist.";
                    return false;
                }
            }
            DataCenterLocation original = _context.DataCenterLocation.Where(a => a.LocationID == dataCenter.LocationID).FirstOrDefault();
            bool changed = original.Location != dataCenter.Location;
            // check if any data center info changed
            if (changed)
            {
                try
                {
                    DataCenterLocation dataCenterUpdated = _context.DataCenterLocation
                                                            .Where(a => a.LocationID == dataCenter.LocationID)
                                                            .FirstOrDefault();
                    dataCenterUpdated.Location = dataCenter.Location;
                    dataCenterUpdated.LocationID = dataCenter.LocationID;

                    _context.SaveChanges();
                    msg = "Data Center information succesfully updated.";
                    return true;
                }
                catch
                {
                    msg = "Failed to update data center.";
                    return false;
                }
            }
            else
            {
                msg = "Information is identical, no update performed.";
                return false;
            }

        }
        // delete data center
        public bool DeleteDataCenter(int referenceID, out string msg)
        {
            DataCenterLocation dataCenterToBeDeleted = _context.DataCenterLocation
                                    .Where(a => a.LocationID == referenceID)
                                    .FirstOrDefault();
            // check servers associated with the data center
            var dataCenterServers = _context.Server
                                       .Where(a => a.LocationID == referenceID)
                                       .FirstOrDefault();

            if (dataCenterToBeDeleted == null)
            {
                msg = "Data Center could not be deleted.";
                return false;
            }

            if (dataCenterServers != null)
            {
                msg = "Data Center has Server(s) associated, cannot be deleted";
                return false;
            }

            try
            {
                _context.DataCenterLocation.Remove(dataCenterToBeDeleted);
                _context.SaveChanges();
                msg = "Data Center Successfully Deleted";
                return true;
            }
            catch
            {
                msg = "Failed to update Data Center.";
                return false;
            }

        }
    }
}
