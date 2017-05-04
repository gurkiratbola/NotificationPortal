using NotificationPortal.Models;
using NotificationPortal.ViewModels;
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

        public IEnumerable<ClientVM> GetClientList()
        {
            try
            {
                IEnumerable<ClientVM> clientList = _context.Client
                                                .Select(c => new ClientVM
                                                {
                                                    ClientName = c.ClientName,
                                                    StatusID = c.StatusID,
                                                    StatusName = c.Status.StatusName,
                                                    ReferenceID = c.ReferenceID
                                                });
                return clientList;
            }
            catch (Exception ex)
            {
                return null;
            }
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
                Client newClient = new Client();
                newClient.ClientName = client.ClientName;
                newClient.StatusID = client.StatusID;
                newClient.ReferenceID = Guid.NewGuid().ToString();
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
                    StatusID = b.StatusID,
                    StatusName = b.Status.StatusName,
                    ReferenceID = b.ReferenceID
                }).FirstOrDefault();
                return client;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool EditClient(ClientVM client, out string msg)
        {
            Client c = _context.Client.Where(a => a.ClientName == client.ClientName).FirstOrDefault();
            if (c.ReferenceID != client.ReferenceID)
            {
                msg = "Client name already exist.";
                return false;
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