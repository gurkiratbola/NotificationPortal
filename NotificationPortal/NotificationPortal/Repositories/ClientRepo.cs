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
        const string APP_STATUS_TYPE_NAME = "Client";
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public IEnumerable<ClientVM> GetClientList(){
            IEnumerable<ClientVM> clientList = _context.Client
                                                .Select(c => new ClientVM
                                                {
                                                    ClientID = c.ClientID,
                                                    ClientName = c.ClientName,
                                                    StatusID = c.StatusID,
                                                    StatusName = c.Status.StatusName
                                                });
            return clientList;
        }

        public SelectList GetStatusList() {
            IEnumerable<SelectListItem> statusList = _context.Status
                                    .Where(a=>a.StatusType.StatusTypeName == APP_STATUS_TYPE_NAME)
                                    .Select(sv => new SelectListItem()
                                    {
                                        Value = sv.StatusID.ToString(),
                                        Text = sv.StatusName
                                    });

            return new SelectList(statusList, "Value", "Text");
        }

        public bool AddClient(ClientVM client, out string msg)
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

        public ClientVM GetClient(int clientID) {
            ClientVM client = _context.Client
                            .Where(a => a.ClientID == clientID)
                            .Select(b => new ClientVM
                            {
                                ClientID = b.ClientID,
                                ClientName = b.ClientName,
                                StatusID = b.StatusID,
                                StatusName =b.Status.StatusName
                            }).FirstOrDefault();
            return client;
        }

        public bool EditClient(ClientVM client, out string msg)
        {
            Client c = _context.Client.Where(a => a.ClientName == client.ClientName).FirstOrDefault();
            if (c != null)
            {
                msg = "Client name already exist.";
                return false;
            }
            try
            {
                Client clientUpdated= _context.Client
                                        .Where(a => a.ClientID == client.ClientID)
                                        .FirstOrDefault();
                clientUpdated.ClientName = client.ClientName;
                clientUpdated.StatusID = client.StatusID;
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

        public bool DeleteClient(int clientID, out string msg) {
            // check if client exists
            Client clientToBeDeleted = _context.Client
                                    .Where(a => a.ClientID == clientID)
                                    .FirstOrDefault();
            // check applications associated with client
            var clientApplications = _context.Application
                                       .Where(a => a.ClientID == clientID)
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

            _context.Client.Remove(clientToBeDeleted);
            _context.SaveChanges();
            msg = "Client Successfully Deleted";
            return true;
        }
    }
        //public string FindUserID()
        //{
        //    string name = User.Identity.Name;
        //    UserDetail user = _context.UserDetail
        //            .Where(u => u.User.UserName == name).FirstOrDefault();
        //    string userId = user.UserID;
        //    return userId;
        //}
}