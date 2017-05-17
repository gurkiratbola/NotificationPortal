using NotificationPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NotificationPortal.ViewModels;
using PagedList;
using System.Data.SqlClient;

namespace NotificationPortal.Repositories
{
    public class ServerRepo
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public IEnumerable<ServerListVM> Sort(IEnumerable<ServerListVM> list, string sortOrder, string searchString = null)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(c => c.ServerName.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case ConstantsRepo.SORT_SERVER_BY_NAME_DESC:
                    list = list.OrderByDescending(c => c.StatusName);
                    break;
                case ConstantsRepo.SORT_STATUS_BY_NAME_DESC:
                    list = list.OrderByDescending(c => c.Description);
                    break;
                case ConstantsRepo.SORT_STATUS_BY_NAME_ASCE:
                    list = list.OrderBy(c => c.LocationName);
                    break;
                case ConstantsRepo.SORT_SERVERTYPE_BY_NAME_ASCE:
                    list = list.OrderBy(c => c.ServerTypeName);
                    break;
                case ConstantsRepo.SORT_SERVERTYPE_BY_NAME_DESC:
                    list = list.OrderByDescending(c => c.ServerName);
                    break;
                default:
                    list = list.OrderBy(c => c.ServerName);
                    break;
            }

            return list;
        }

        public ServerIndexVM GetServerList(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
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

                int totalNumOfServers = serverList.Count();
                page = searchString == null ? page : 1;
                int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
                searchString = searchString ?? currentFilter;
                int pageNumber = (page ?? 1);

                int defaultPageSize = ConstantsRepo.PAGE_SIZE;

                // sort by status name default
                sortOrder = sortOrder == null ? ConstantsRepo.SORT_SERVER_BY_STATUS_NAME_DESC : sortOrder;

                ServerIndexVM model = new ServerIndexVM
                {
                    Servers = Sort(serverList, sortOrder, searchString).ToPagedList(pageNumber, defaultPageSize),
                    CurrentFilter = searchString,
                    CurrentSort = sortOrder,
                    TotalItemCount = totalNumOfServers,
                    ItemStart = currentPageIndex * 10 + 1,
                    ItemEnd = totalNumOfServers - (10 * currentPageIndex) >= 10 ? 10 * (currentPageIndex + 1) : totalNumOfServers,

                    DescriptionSort = sortOrder == ConstantsRepo.SORT_SERVER_BY_DESCRIPTION_DESC ? ConstantsRepo.SORT_SERVER_BY_DESCRIPTION_ASCE : ConstantsRepo.SORT_SERVER_BY_DESCRIPTION_DESC,
                    StatusSort = sortOrder == ConstantsRepo.SORT_SERVER_BY_STATUS_NAME_DESC ? ConstantsRepo.SORT_SERVER_BY_STATUS_NAME_ASCE : ConstantsRepo.SORT_SERVER_BY_STATUS_NAME_DESC,
                    LocationSort = sortOrder == ConstantsRepo.SORT_SERVER_BY_LOCATION_NAME_DESC ? ConstantsRepo.SORT_SERVER_BY_LOCATION_NAME_ASCE : ConstantsRepo.SORT_SERVER_BY_LOCATION_NAME_DESC,
                    ServerTypeSort = sortOrder == ConstantsRepo.SORT_SERVERTYPE_BY_NAME_DESC ? ConstantsRepo.SORT_SERVERTYPE_BY_NAME_ASCE : ConstantsRepo.SORT_SERVERTYPE_BY_NAME_DESC,
                    ServerNameSort = sortOrder == ConstantsRepo.SORT_SERVER_BY_NAME_DESC ? ConstantsRepo.SORT_SERVER_BY_NAME_ASCE : ConstantsRepo.SORT_SERVER_BY_NAME_DESC,
                };

                return model;
            }
            catch (Exception e)
            {
                if(e is SqlException)
                { }

                return null;
            }
        }

        public ServerVM GetServer(string referenceID)
        {
            ServerVM server = _context.Server.Where(a => a.ReferenceID == referenceID)
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

        public ServerDetailVM GetServerDetails(string referenceID)
        {
            Server server = _context.Server.Where(a => a.ReferenceID == referenceID).FirstOrDefault();

            IEnumerable<Notification> allServerNotifications = server.Notifications;

            IEnumerable<ServerThreadVM> serverThreads = allServerNotifications.Select(t => new ServerThreadVM()
            {
                ReferenceID = t.ReferenceID,
                ThreadID = t.IncidentNumber,
                ThreadHeading = t.NotificationHeading,
                SentDateTime = t.SentDateTime,
                ThreadType = t.NotificationType.NotificationTypeName,
                LevelOfImpact = t.LevelOfImpact.LevelName,
                ThreadStatus = t.Status.StatusName
            });

            IEnumerable<Application> allServerApplications = server.Applications;

            //.GroupBy(n => n.ClientID)
            //.Select(t => t.OrderBy(i => i.FirstName))
            IEnumerable<ServerApplicationVM> serverApplication = allServerApplications.Select(t => new ServerApplicationVM()
            {
                ApplicationName = t.ApplicationName,
                Status = t.Status.StatusName,
                ClientID = t.Client.ClientName,
                Description = t.Description,
                URL = t.URL
            });

            ServerDetailVM model = new ServerDetailVM
            {
                ServerName = server.ServerName,
                ReferenceID = server.ReferenceID,
                Description = server.Description,
                Status = server.Status.StatusName,
                Location = server.DataCenterLocation.Location,
                ServerType = server.ServerType.ServerTypeName,
                Threads = serverThreads,
                Applications = serverApplication
            };

            return model;
        }

        public bool AddServer(ServerVM server, out string msg)
        {
            Server s = _context.Server.Where(a => a.ServerName == server.ServerName).FirstOrDefault();

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

                if (server.ApplicationsReferenceIDs == null)
                {
                    server.ApplicationsReferenceIDs = new string[0];
                }

                var applications = _context.Application.Where(b => server.ApplicationsReferenceIDs.Contains(b.ReferenceID));
                newServer.Applications = applications.ToList();
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

        public ServerDeleteVM GetDeleteServer(string referenceID)
        {
            ServerDeleteVM server = _context.Server.Where(a => a.ReferenceID == referenceID)
                                    .Select(b => new ServerDeleteVM
                                    {

                                        ServerName = b.ServerName,
                                        ReferenceID = b.ReferenceID,
                                        Description = b.Description,
                                        StatusName = b.Status.StatusName,
                                        Location = b.DataCenterLocation.Location,
                                        ServerTypeName = b.ServerType.ServerTypeName
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
                Server serverUpdated = _context.Server.Where(a => a.ReferenceID == server.ReferenceID).FirstOrDefault();

                serverUpdated.ServerName = server.ServerName;
                serverUpdated.StatusID = server.StatusID;
                serverUpdated.LocationID = server.LocationID;
                serverUpdated.Description = server.Description;
                serverUpdated.ReferenceID = server.ReferenceID;
                serverUpdated.ServerTypeID = server.ServerTypeID;

                if (server.ApplicationsReferenceIDs == null)
                {
                    server.ApplicationsReferenceIDs = new string[0];
                }

                var applications = _context.Application.Where(b => server.ApplicationsReferenceIDs.Contains(b.ReferenceID));

                serverUpdated.Applications.Clear();

                serverUpdated.Applications = applications.ToList();
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
            Server serverToBeDeleted = _context.Server.Where(a => a.ReferenceID == referenceID).FirstOrDefault();

            // check applications associated with client
            var serverApplications = _context.Application.Where(a => a.ReferenceID == referenceID).FirstOrDefault();

            var serverNotifications = _context.Notification.Where(a => a.ReferenceID == referenceID).FirstOrDefault();

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

        public IEnumerable<ServerListVM> GetServerList()
        {
            IEnumerable<ServerListVM> serverList = _context.Server.Select(c => new ServerListVM
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
            IEnumerable<SelectListItem> statusList = _context.Status.Where(a => a.StatusType.StatusTypeName == Key.STATUS_TYPE_SERVER)
            .Select(sv => new SelectListItem()
            {
                Value = sv.StatusID.ToString(),
                Text = sv.StatusName
            });

            return new SelectList(statusList, "Value", "Text");
        }


        public SelectList GetLocationList()
        {

            IEnumerable<SelectListItem> locationList = _context.DataCenterLocation.Select(app => new SelectListItem
            {
                Value = app.LocationID.ToString(),
                Text = app.Location
            });

            return new SelectList(locationList, "Value", "Text");
        }

        public SelectList GetServerTypeList()
        {

            IEnumerable<SelectListItem> serverTypeList = _context.ServerType.Select(server => new SelectListItem
            {
                Value = server.ServerTypeID.ToString(),
                Text = server.ServerTypeName
            });

            return new SelectList(serverTypeList, "Value", "Text");
        }


        public IEnumerable<ServerApplicationVM> GetApplicationList()
        {
            var apps = _context.Application.Select(a => new ServerApplicationVM
            {
                ApplicationName = a.ApplicationName,
                ReferenceID = a.ReferenceID,
                ClientID = a.Client.ClientName,
                Description = a.Description,
                Status = a.Status.StatusName,
            });

            return apps;
        }
    }
}


