using NotificationPortal.Models;
using NotificationPortal.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static NotificationPortal.ViewModels.ValidationVM;

namespace NotificationPortal.Repositories
{
    public class ClientRepo
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public IEnumerable<ClientVM> Sort(IEnumerable<ClientVM> list, string sortOrder, string searchString = null) {

            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(c => c.ClientName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case ConstantsRepo.SORT_CLIENT_BY_NAME_DESC:
                    list = list.OrderByDescending(c => c.ClientName);
                    break;

                case ConstantsRepo.SORT_STATUS_BY_NAME_DESC:
                    list = list.OrderByDescending(c => c.StatusName);
                    break;

                case ConstantsRepo.SORT_STATUS_BY_NAME_ASCE:
                    list = list.OrderBy(c => c.StatusName);
                    break;

                default:
                    list = list.OrderBy(c => c.ClientName);
                    break;
            }
            return list;
        }

        public ClientIndexVM GetClientList(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                IEnumerable<ClientVM> clientList = _context.Client
                                                .Select(c => new ClientVM
                                                {
                                                    ClientName = c.ClientName,
                                                    ClientID = c.ClientID,
                                                    StatusID = c.StatusID,
                                                    StatusName = c.Status.StatusName,
                                                    ReferenceID = c.ReferenceID,
                                                    NumOfApps = c.Applications.Count()
                                                });
                int totalNumOfClients = clientList.Count();
                page = searchString == null ? page : 1;
                int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
                searchString = searchString ?? currentFilter;
                int pageNumber = (page ?? 1);
                int defaultPageSize = ConstantsRepo.PAGE_SIZE;
                ClientIndexVM model = new ClientIndexVM
                {
                    Clients = Sort(clientList, sortOrder, searchString).ToPagedList(pageNumber, defaultPageSize),
                    CurrentFilter = searchString,
                    CurrentSort = sortOrder,
                    TotalItemCount = totalNumOfClients,
                    ItemStart = currentPageIndex * 10 + 1,
                    ItemEnd = totalNumOfClients - (10 * currentPageIndex) >= 10? 10 * (currentPageIndex + 1): totalNumOfClients,
                    ClientHeadingSort = sortOrder == ConstantsRepo.SORT_CLIENT_BY_NAME_DESC ? ConstantsRepo.SORT_CLIENT_BY_NAME_ASCE : ConstantsRepo.SORT_CLIENT_BY_NAME_DESC,
                    StatusSort = sortOrder == ConstantsRepo.SORT_STATUS_BY_NAME_DESC ? ConstantsRepo.SORT_STATUS_BY_NAME_ASCE : ConstantsRepo.SORT_STATUS_BY_NAME_DESC,
                };
                return model;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<ClientApplicationVM> GetClientApplications(int clientID)
        {

            IEnumerable<ClientApplicationVM> applications = _context.Application
                                                           .Where(a => a.ClientID == clientID)
                                                           .Select(c => new ClientApplicationVM
                                                           {
                                                               ReferenceID = c.ReferenceID,
                                                               ApplicationName = c.ApplicationName,
                                                               URL = c.URL,
                                                               Description = c.Description
                                                           }).ToList();
            return applications;
        }

        public bool AddClient(ClientCreateVM client, out string msg)
        {
            Client c = _context.Client.Where(a => a.ClientName == client.ClientName)
                            .FirstOrDefault();
            if (c != null) {
                msg = "Client name already exist.";
                return false;
            }
            try
            {
                Client newClient = new Client()
                {
                    ClientName = client.ClientName,
                    StatusID = client.StatusID,
                    ReferenceID = Guid.NewGuid().ToString()
                };
                _context.Client.Add(newClient);
                _context.SaveChanges();
                msg = "Client successfully added";
                return true;
            }
            catch
            {
                msg = "Failed to add client.";
                return false;
            }
        }

        public ClientVM GetClient(string referenceID) {
            try {
                ClientVM client = _context.Client
                .Where(a => a.ReferenceID == referenceID)
                .Select(b => new ClientVM
                {
                    ClientName = b.ClientName,
                    ClientID = b.ClientID,
                    StatusID = b.StatusID,
                    StatusName = b.Status.StatusName,
                    ReferenceID = b.ReferenceID
                }).FirstOrDefault();

                client.Applications = GetClientApplications(client.ClientID);

                return client;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool EditClient(ClientVM client, out string msg)
        {
            Client c = _context.Client.Where(a => a.ClientName == client.ClientName).FirstOrDefault();
            if (c != null) {
                if (c.ReferenceID != client.ReferenceID)
                {
                    msg = "Client name already exist.";
                    return false;
                }
            }

            Client original = _context.Client.Where(a => a.ReferenceID == client.ReferenceID).FirstOrDefault();
            bool changed = original.ClientName != client.ClientName || original.StatusID != client.StatusID;
            if (changed)
            {
                try
                {
                    Client clientUpdated = _context.Client
                                            .Where(a => a.ReferenceID == client.ReferenceID)
                                            .FirstOrDefault();
                    clientUpdated.ClientName = client.ClientName;
                    clientUpdated.StatusID = client.StatusID;
                    clientUpdated.ReferenceID = client.ReferenceID;
                    _context.SaveChanges();
                    msg = "Client information succesfully updated.";
                    return true;
                }
                catch
                {
                    msg = "Failed to update client.";
                    return false;
                }
            }
            else {
                msg = "Information is identical, no update performed.";
                return false;
            }
        }

        public bool DeleteClient(string referenceID, out string msg) {
            Client clientToBeDeleted = _context.Client
                                    .Where(a => a.ReferenceID == referenceID)
                                    .FirstOrDefault();
            // check applications associated with client
            var clientApplications = _context.Application
                                       .Where(a => a.ReferenceID == referenceID)
                                       .FirstOrDefault();
            if (clientToBeDeleted == null)
            {
                msg = "Client could not be deleted.";
                return false;
            }
            if (clientApplications != null)
            {
                msg = "Client has application(s) associated, cannot be deleted";
                return false;
            }

            try {
                _context.Client.Remove(clientToBeDeleted);
                _context.SaveChanges();
                msg = "Client Successfully Deleted";
                return true;
            } catch {
                msg = "Failed to update client.";
                return false;
            }

        }
    }

}