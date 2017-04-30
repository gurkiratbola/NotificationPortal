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
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<ClientVM> GetClientList(){
            IEnumerable<ClientVM> clientList = db.Client
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
            IEnumerable<SelectListItem> statusList = db.Status
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

            try
            {
                Client newClient = new Client();
                newClient.ClientName = client.ClientName;
                newClient.StatusID = client.StatusID;
                db.Client.Add(newClient);
                db.SaveChanges();
                msg = "Client added.";
                return true;
            }
            catch
            {
                msg = "Failed to add client.";
                return false;
            }
        }

        public ClientVM GetClient(int clientID, string userID) {
            ClientVM client = db.Client
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
            try
            {
                Client clientUpdated= db.Client
                                        .Where(a => a.ClientID == client.ClientID)
                                        .FirstOrDefault();
                clientUpdated.ClientName = client.ClientName;
                clientUpdated.StatusID = client.StatusID;
                db.SaveChanges();
                msg = "Client information succesfully updated.";
                return true;
            }
            catch
            {
                msg = "Failed to update client.";
                return false;
            }
        }

        public void DeleteClient(int clientID, out string msg) {
            // check client
            Client clientToBeDeleted = db.Client
                                    .Where(a => a.ClientID == clientID)
                                    .FirstOrDefault();
            // check applications
            var clientApplications = db.Application
                                       .Where(a => a.ClientID == clientID)
                                       .FirstOrDefault();
            // check notifications
            var clientNotifications = db.Notification
                                 .Where(a => a.Application.ClientID == clientID)
                                 .FirstOrDefault();
            if (clientNotifications != null)
            {
                msg = "Client associated with notification, cannot be deleted";
            }
            else
            {
                if (clientToBeDeleted != null)
                {
                    // check if there are application with this client
                    if (clientNotifications != null)
                    {
                        msg = "Client has application(s) associated, cannot be deleted";
                    }
                    else
                    {
                        db.Client.Remove(clientToBeDeleted);
                        db.SaveChanges();
                        msg = "Client Successfully Deleted";
                    }
                }
                else
                {
                    msg = "Client could not be deleted.";
                }
            }
        }
    }
}