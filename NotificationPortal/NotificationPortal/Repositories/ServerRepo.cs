using NotificationPortal.Models;
using NotificationPortal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Web.Mvc;
using NotificationPortal.ViewModels;

namespace NotificationPortal.Repositories
{

    public class ServerRepo
    {
        const string APP_STATUS_TYPE_NAME = "Server";
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public IEnumerable<ServerListVM> GetServerList()
        {
            IEnumerable<ServerListVM> serverList = _context.Server
                                                .Select(c => new ServerListVM
                                                {
                                                    ServerName = c.ServerName,
                                                    ReferenceID = c.ReferenceID,
                                                    StatusName = c.Status.StatusName,
                                                    ServerTypeName = c.ServerType.ServerTypeName,
                                                    LocationName = c.DataCenterLocation.Location,
                                                    Description = c.Description
                                                });
            return serverList;
        }

        public SelectList GetStatusList()
        {
            IEnumerable<SelectListItem> statusList = _context.Status
                                    .Where(a => a.StatusType.StatusTypeName == APP_STATUS_TYPE_NAME)
                                    .Select(sv => new SelectListItem()
                                    {
                                        Value = sv.StatusID.ToString(),
                                        Text = sv.StatusName
                                    });

            return new SelectList(statusList, "Value", "Text");
        }


        public SelectList GetLocationList()
        {

            IEnumerable<SelectListItem> locationList = _context.DataCenterLocation
                    .Select(app =>
                                new SelectListItem
                                {
                                    Value = app.LocationID.ToString(),
                                    Text = app.Location
                                });

            return new SelectList(locationList, "Value", "Text");
        }

        public SelectList GetServerTypeList()
        {

            IEnumerable<SelectListItem> serverTypeList = _context.ServerType
                    .Select(server =>
                                new SelectListItem
                                {
                                    Value = server.ServerTypeID.ToString(),
                                    Text = server.ServerTypeName
                                });

            return new SelectList(serverTypeList, "Value", "Text");
        }



        //public SelectList GetLocationList()
        //{
        //    IEnumerable<SelectListItem> locationList = _context.DataCenterLocation
        //                            .Where(a => a.StatusType.StatusTypeName == APP_STATUS_TYPE_NAME)
        //                            .Select(sv => new SelectListItem()
        //                            {
        //                                Value = sv.StatusID.ToString(),
        //                                Text = sv.StatusName
        //                            });

        //    return new SelectList(locationList, "Value", "Text");
        //}


        public bool AddServer(ServerVM server, out string msg)
        {
            Server s = _context.Server.Where(a => a.ServerName == server.ServerName)
                            .FirstOrDefault();
            if (s != null)
            {
                msg = "Server name already exist.";
                return false;
            }
            try
            {

                Server newServer = new Server();
                newServer.ServerName = server.ServerName;
                newServer.StatusID = server.StatusID;
                newServer.LocationID = server.LocationID;
                newServer.Description = server.Description;
                newServer.ServerTypeID = server.ServerTypeID;
                newServer.ReferenceID = Guid.NewGuid().ToString();
                _context.Server.Add(newServer);
                _context.SaveChanges();
                msg = "Server successfully added";
                return true;
            }
            catch
            {
                msg = "Failed to add server.";
                return false;
            }
        }

        public ServerVM GetServer(string referenceID)
        {
            ServerVM server = _context.Server
                            .Where(a => a.ReferenceID == referenceID)
                            .Select(b => new ServerVM
                            {
                                ServerName = b.ServerName,
                                ReferenceID = b.ReferenceID,
                                Description = b.Description,
                                StatusID = b.StatusID,
                                LocationID = b.LocationID,
                                ServerTypeID = b.ServerTypeID,

                            }).FirstOrDefault();
            return server;
        }

        public ServerDetailVM GetDetailServer(string referenceID)
        {
            Server server = _context.Server
                            .Where(a => a.ReferenceID == referenceID).FirstOrDefault();


            IEnumerable<Notification> allServerNotifications = server.Notifications;
            IEnumerable<ServerThreadVM> serverThreads = allServerNotifications
                .GroupBy(n => n.IncidentNumber)
                .Select(t => t.OrderBy(i => i.SentDateTime))
                .Select(
                    t => new ServerThreadVM()
                    {
                        ReferenceID = t.FirstOrDefault().ReferenceID,
                        ThreadID = t.FirstOrDefault().IncidentNumber,
                        ThreadHeading = t.FirstOrDefault().NotificationHeading,
                        SentDateTime = t.FirstOrDefault().SentDateTime,
                        ThreadType = t.LastOrDefault().NotificationType.NotificationTypeName,
                        LevelOfImpact = t.LastOrDefault().LevelOfImpact.LevelName,
                        ThreadStatus = t.LastOrDefault().Status.StatusName
                    })
                .GroupBy(n => n.ThreadID)
                .Select(t => t.OrderByDescending(i => i.SentDateTime).FirstOrDefault());

            ServerDetailVM model =  new ServerDetailVM
             {
                ServerName = server.ServerName,
                 ReferenceID = server.ReferenceID,
                 Description = server.Description,
                 Status = server.Status.StatusName,
                 Location = server.DataCenterLocation.Location,
                 ServerType = server.ServerType.ServerTypeName,
                 Threads = serverThreads
             };
            return model;
        }

        public ServerDeleteVM GetDeleteServer(string referenceID)
        {
            ServerDeleteVM server = _context.Server
                            .Where(a => a.ReferenceID == referenceID)
                            .Select(b => new ServerDeleteVM
                            {

                                ServerName = b.ServerName,
                                ReferenceID = b.ReferenceID,
                                Description = b.Description,
                                StatusID = b.StatusID,
                                LocationID = b.LocationID,
                                ServerTypeID = b.ServerTypeID,

                            }).FirstOrDefault();
            return server;
        }

        public bool EditServer(ServerVM server, out string msg)
        {
            Server s = _context.Server.Where(a => a.ServerName == server.ServerName).FirstOrDefault();
            //if (s != null)
            //{
            //    msg = "Server name already exist.";
            //    return false;
            //}
            try
            {
                Server serverUpdated = _context.Server
                                        .Where(a => a.ReferenceID == server.ReferenceID)
                                        .FirstOrDefault();
                serverUpdated.ServerName = server.ServerName;
                serverUpdated.StatusID = server.StatusID;
                serverUpdated.LocationID = server.LocationID;
                serverUpdated.Description = server.Description;
                serverUpdated.ReferenceID = server.ReferenceID;
                serverUpdated.ServerTypeID = server.ServerTypeID;
                _context.SaveChanges();
                msg = "Server information succesfully updated.";
                return true;
            }
            catch
            {
                msg = "Failed to update server.";
                return false;
            }
        }

        public bool DeleteServer(string referenceID, out string msg)
        {
            // check if server exists
            Server serverToBeDeleted = _context.Server
                                    .Where(a => a.ReferenceID == referenceID)
                                    .FirstOrDefault();
            // check applications associated with client
            var serverApplications = _context.Application
                                       .Where(a => a.ReferenceID == referenceID)
                                       .FirstOrDefault();
            var serverNotifications = _context.Notification
                                      .Where(a => a.ReferenceID == referenceID)
                                      .FirstOrDefault();
            if (serverToBeDeleted == null)
            {
                msg = "Server could not be deleted.";
                return false;
            }
            if (serverApplications != null)
            {
                msg = "Server has application(s) associated, cannot be deleted";
                return false;
            }
            if (serverNotifications != null)
            {
                msg = "Server has application(s) associated, cannot be deleted";
                return false;
            }

            try
            {
                _context.Server.Remove(serverToBeDeleted);
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


