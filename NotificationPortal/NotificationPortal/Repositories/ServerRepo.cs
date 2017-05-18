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
        private readonly SelectListRepo _selectRepo = new SelectListRepo();
        //Sort function for index of Servers
        public IEnumerable<ServerListVM> Sort(IEnumerable<ServerListVM> list, string sortOrder, string searchString = null)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(c => c.ServerName.ToUpper().Contains(searchString.ToUpper()) || c.StatusName.ToUpper().Contains(searchString.ToUpper()) || c.ServerTypeName.ToUpper().Contains(searchString.ToUpper()) || c.LocationName.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case ConstantsRepo.SORT_SERVER_BY_NAME_DESC:
                    list = list.OrderByDescending(c => c.ServerName);
                    break;
                case ConstantsRepo.SORT_SERVER_BY_STATUS_NAME_DESC:
                    list = list.OrderByDescending(c => c.StatusName);
                    break;
                case ConstantsRepo.SORT_SERVER_BY_STATUS_NAME_ASCE:
                    list = list.OrderBy(c => c.StatusName);
                    break;
                case ConstantsRepo.SORT_SERVERTYPE_BY_NAME_ASCE:
                    list = list.OrderBy(c => c.ServerTypeName);
                    break;
                case ConstantsRepo.SORT_SERVERTYPE_BY_NAME_DESC:
                    list = list.OrderByDescending(c => c.ServerTypeName);
                    break;
                case ConstantsRepo.SORT_SERVER_BY_DESCRIPTION_ASCE:
                    list = list.OrderBy(c => c.Description);
                    break;
                case ConstantsRepo.SORT_SERVER_BY_DESCRIPTION_DESC:
                    list = list.OrderByDescending(c => c.Description);
                    break;
                case ConstantsRepo.SORT_SERVER_BY_LOCATION_NAME_ASCE:
                    list = list.OrderBy(c => c.LocationName);
                    break;
                case ConstantsRepo.SORT_SERVER_BY_LOCATION_NAME_DESC:
                    list = list.OrderByDescending(c => c.LocationName);
                    break;
                default:
                    list = list.OrderBy(c => c.ServerName);
                    break;
            }

            return list;
        }
        //Create a List for index of servers
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

                page = searchString == null ? page : 1;
                int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
                searchString = searchString ?? currentFilter;
                int pageNumber = (page ?? 1);
                int defaultPageSize = ConstantsRepo.PAGE_SIZE;

                var sorted = Sort(serverList, sortOrder, searchString);
                int totalNumOfServers = sorted.Count();

                // sort by status name default
                sortOrder = sortOrder ?? ConstantsRepo.SORT_SERVER_BY_NAME_DESC;

                ServerIndexVM model = new ServerIndexVM
                {
                    Servers = sorted.ToPagedList(pageNumber, defaultPageSize),
                    CurrentFilter = searchString,
                    CurrentSort = sortOrder,
                    TotalItemCount = totalNumOfServers,
                    ItemStart = currentPageIndex * defaultPageSize + 1,
                    ItemEnd = totalNumOfServers - (defaultPageSize * currentPageIndex) >= defaultPageSize ? defaultPageSize * (currentPageIndex + 1) : totalNumOfServers,

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
        //Gets all the detials involved with the server takes referenceID
        public ServerDetailVM GetServerDetails(string referenceID)
        {
            try
            {
                Server server = _context.Server.Where(a => a.ReferenceID == referenceID).FirstOrDefault();

                IEnumerable<Notification> allServerNotifications = server.Notifications;

                IEnumerable<ServerThreadVM> serverThreads = allServerNotifications.Select(thread => new ServerThreadVM()
                {
                    ReferenceID = thread.ReferenceID,
                    ThreadID = thread.IncidentNumber,
                    ThreadHeading = thread.NotificationHeading,
                    SentDateTime = thread.SentDateTime,
                    ThreadType = thread.NotificationType.NotificationTypeName,
                    LevelOfImpact = thread.LevelOfImpact.LevelName,
                    ThreadStatus = thread.Status.StatusName
                });

                IEnumerable<Application> allServerApplications = server.Applications;

                //.GroupBy(n => n.ClientID)
                //.Select(t => t.OrderBy(i => i.FirstName))
                IEnumerable<ServerApplicationVM> serverApplication = allServerApplications.Select(app => new ServerApplicationVM()
                {
                    ApplicationReferenceID = app.ReferenceID,
                    ApplicationName = app.ApplicationName,
                    Status = app.Status.StatusName,
                    ClientID = app.Client.ClientName,
                    Description = app.Description,
                    URL = app.URL
                });

                var getServerApplications = server.Applications.Select(app => app.ReferenceID).ToArray();

                ServerDetailVM model = new ServerDetailVM
                {
                    ServerName = server.ServerName,
                    ReferenceID = server.ReferenceID,
                    Description = server.Description,
                    StatusID = server.Status.StatusID,
                    Status = server.Status.StatusName,
                    LocationID = server.DataCenterLocation.LocationID,
                    Location = server.DataCenterLocation.Location,
                    ServerTypeID = server.ServerType.ServerTypeID,
                    ServerType = server.ServerType.ServerTypeName,
                    Threads = serverThreads,
                    Applications = serverApplication,
                    ApplicationReferenceIDs = getServerApplications
                };

                model.StatusList = GetStatusList();
                model.ServerTypeList = GetServerTypeList();
                model.LocationList = GetLocationList();
                model.ApplicationList = _selectRepo.GetApplicationList();

                return model;
            }
            catch (Exception e)
            {
                if(e is SqlException)
                { }

                return null;
            }
        }
        ///Creates a new server from ServerVM
        public bool AddServer(ServerVM model, out string msg)
        {
            try
            {
                Server s = _context.Server.Where(a => a.ServerName == model.ServerName).FirstOrDefault();

                if (s != null)
                {
                    msg = "Server name already exist.";
                    return false;
                }

                Server newServer = new Server();
                newServer.ServerName = model.ServerName;
                newServer.StatusID = model.StatusID;
                newServer.LocationID = model.LocationID;
                newServer.Description = model.Description;
                newServer.ServerTypeID = model.ServerTypeID;
                newServer.ReferenceID = Guid.NewGuid().ToString();

                if (model.ApplicationReferenceIDs == null)
                {
                    model.ApplicationReferenceIDs = new string[0];
                }

                var applications = _context.Application.Where(b => model.ApplicationReferenceIDs.Contains(b.ReferenceID));

                newServer.Applications = applications.ToList();
                _context.Server.Add(newServer);

                _context.SaveChanges();

                msg = "Server successfully added";
                return true;
            }
            catch (Exception e)
            {
                if(e is SqlException)
                { }

                msg = "Failed to add server.";
                return false;
            }
        }
        //Edit Server
        public bool EditServer(ServerDetailVM model, out string msg)
        {
            try
            {
                Server s = _context.Server.Where(a => a.ServerName == model.ServerName).FirstOrDefault();

                if (s != null)
                {
                    if(s.ReferenceID != s.ReferenceID)
                    {
                        msg = "Server name already exist.";
                        return false;
                    }
                }

                Server serverUpdated = _context.Server.Where(a => a.ReferenceID == model.ReferenceID).FirstOrDefault();

                serverUpdated.ServerName = model.ServerName;
                serverUpdated.StatusID = model.StatusID;
                serverUpdated.LocationID = model.LocationID;
                serverUpdated.Description = model.Description;
                serverUpdated.ReferenceID = model.ReferenceID;
                serverUpdated.ServerTypeID = model.ServerTypeID;

                if (model.ApplicationReferenceIDs == null)
                {
                    model.ApplicationReferenceIDs = new string[0];
                }

                var apps = _context.Application.Where(a => model.ApplicationReferenceIDs.Contains(a.ReferenceID));

                serverUpdated.Applications.Clear();

                serverUpdated.Applications = apps.ToList();
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
        //Get the Server to Delete 
        public ServerDeleteVM GetDeleteServer(string referenceID)
        {
            try
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
            catch (Exception e)
            {
                if(e is SqlException)
                { }

                return null;
            }
        }
        //Delete the server
        public bool DeleteServer(string referenceID, out string msg)
        {
            // check if server exists
            Server serverToBeDeleted = _context.Server.Where(a => a.ReferenceID == referenceID).FirstOrDefault();

            // check applications associated with server
       
            IEnumerable<Application> allServerApplications = serverToBeDeleted.Applications;

            // check notifications associated with server
            IEnumerable<Notification> allServerNotifications = serverToBeDeleted.Notifications;
            //var serverApplications = _context.Application.Where(s => serverToBeDeleted.ReferenceID.Contains(s.ReferenceID));





            if (serverToBeDeleted == null)
            {
                msg = "Server could not be deleted.";
                return false;
            }

            if (allServerApplications.Count() > 0)
            {
                msg = "Server has application(s) associated, cannot be deleted";
                return false;
            }

            if (allServerNotifications.Count() > 0)
            {
                msg = "Server has notifications(s) associated, cannot be deleted";
                return false;
            }

            try
            {
                _context.Server.Remove(serverToBeDeleted);
                _context.SaveChanges();

                msg = "Server Successfully Deleted";
                return true;
            }
            catch (Exception e)
            {
                if(e is SqlException)
                { }

                msg = "Failed to update server.";
                return false;
            }
        }
        //Creates a list of all the server objects
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
        //Creates a list of all the status objects associated with server
        public SelectList GetStatusList()
        {
            IEnumerable<SelectListItem> statusList = _context.Status.Where(a => a.StatusType.StatusTypeName == Key.STATUS_TYPE_SERVER)
            .Select(sv => new SelectListItem()
            {
                Value = sv.StatusID.ToString(),
                Text = sv.StatusName
            }).OrderBy(s => s.Value);

            return new SelectList(statusList, "Value", "Text");
        }
        //Creates a list of all the location objects associated with server
        public SelectList GetLocationList()
        {

            IEnumerable<SelectListItem> locationList = _context.DataCenterLocation.Select(app => new SelectListItem
            {
                Value = app.LocationID.ToString(),
                Text = app.Location
            });

            return new SelectList(locationList, "Value", "Text");
        }
        //Creates a list of all the status objects associated with server
        public SelectList GetServerTypeList()
        {

            IEnumerable<SelectListItem> serverTypeList = _context.ServerType.Select(server => new SelectListItem
            {
                Value = server.ServerTypeID.ToString(),
                Text = server.ServerTypeName
            });

            return new SelectList(serverTypeList, "Value", "Text");
        }
    }
}


