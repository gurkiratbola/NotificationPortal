using NotificationPortal.Models;
using NotificationPortal.ViewModels;
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

        public IEnumerable<DataCenterVM> GetDataCenterList()
        {
            IEnumerable<DataCenterVM> dataCenterList = _context.DataCenterLocation
                                                .Select(c => new DataCenterVM
                                                {
                                                    Location = c.Location,
                                                    LocationID = c.LocationID,

                                                });
            return dataCenterList;
        }

        //public SelectList GetServerList()
        //{
        //    IEnumerable<SelectListItem> serverList = _context.Server
        //                            .Where(a => a.StatusType.StatusTypeName == APP_STATUS_TYPE_NAME)
        //                            .Select(sv => new SelectListItem()
        //                            {
        //                                Value = sv.StatusID.ToString(),
        //                                Text = sv.StatusName
        //                            });

        //    return new SelectList(statusList, "Value", "Text");
        //}


        public bool AddDataCenter(DataCenterVM dataCenter, out string msg)
        {
            DataCenterLocation d = _context.DataCenterLocation.Where(a => a.Location == dataCenter.Location)
                            .FirstOrDefault();
            if (d != null)
            {
                msg = "DataCenter name already exist.";
                return false;
            }
            try
            {
                DataCenterLocation newDataCenter = new DataCenterLocation();
                newDataCenter.Location = dataCenter.Location;
                //newDataCenter.LocationID = dataCenter.LocationID;
                _context.DataCenterLocation.Add(newDataCenter);
                _context.SaveChanges();
                msg = "Data Center successfully added";
                return true;
            }
            catch
            {
                msg = "Failed to add server.";
                return false;
            }
        }

        public DataCenterVM GetDataCenter(int referenceID)
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

        //public ServerVM GetDeleteDataCenter(int referenceID)
        //{
        //    DataCenterVM server = _context.Server
        //                    .Where(a => a.ReferenceID == referenceID)
        //                    .Select(b => new ServerVM
        //                    {
        //                        ServerName = b.ServerName,
        //                        ReferenceID = b.ReferenceID,
        //                        Description = b.Description,
        //                        StatusID = b.StatusID,
        //                        LocationID = b.LocationID,

        //                    }).FirstOrDefault();
        //    return server;
        //}

        public bool EditDataCenter(DataCenterVM dataCenter, out string msg)
        {
            DataCenterLocation d = _context.DataCenterLocation.Where(a => a.Location == dataCenter.Location).FirstOrDefault();
            if (d != null)
            {
                msg = "Data name already exist.";
                return false;
            }
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

        public bool DeleteDataCenter(int referenceID, out string msg)
        {
            // check if server exists
            DataCenterLocation dataCenterToBeDeleted = _context.DataCenterLocation
                                    .Where(a => a.LocationID == referenceID)
                                    .FirstOrDefault();
            // check applications associated with client
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
                msg = "DataCenter has Server(s) associated, cannot be deleted";
                return false;
            }


            try
            {
                _context.DataCenterLocation.Remove(dataCenterToBeDeleted);
                _context.SaveChanges();
                msg = "Server Successfully Deleted";
                return true;
            }
            catch
            {
                msg = "Failed to update server.";
                return false;
            }

        }
    }
}
