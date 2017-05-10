using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NotificationPortal.Models
{
    public class Application
    {
        [Key]
        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public virtual Client Client { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<UserDetail> UserDetails { get; set; }
        public virtual ICollection<Server> Servers { get; set; }
        public virtual Status Status { get; set; }
        [ForeignKey("Client")]
        public int ClientID { get; set; }
        [ForeignKey("Status")]
        public int StatusID { get; set; }
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string ReferenceID { get; set; }
    }

    public class Client
    {
        [Key]
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<UserDetail> UserDetails { get; set; }
        [ForeignKey("Status")]
        public int StatusID { get; set; }
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string ReferenceID { get; set; }
    }

    public class DataCenterLocation
    {
        [Key]
        public int LocationID { get; set; }
        public string Location { get; set; }
        public virtual ICollection<Server> Servers { get; set; }
    }

    public class Group
    {
        [Key]
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public virtual ICollection<RoleDetail> RoleDetails { get; set; }
    }

    public class LevelOfImpact
    {
        [Key]
        public int LevelOfImpactID { get; set; }
        public string LevelName { get; set; }
        [Index(IsUnique = true)]
        public int LevelValue { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }

    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }
        public string IncidentNumber { get; set; }
        public string NotificationHeading { get; set; }
        public string NotificationDescription { get; set; }
        public DateTime SentDateTime { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual LevelOfImpact LevelOfImpact { get; set; }
        public virtual NotificationType NotificationType { get; set; }
        public virtual Priority Priority { get; set; }
        public virtual SendMethod SendMethod { get; set; }
        public virtual ICollection<Server> Servers { get; set; }
        public virtual Status Status { get; set; }
        public virtual UserDetail UserDetail { get; set; }
        [ForeignKey("Status")]
        public int StatusID { get; set; }
        [ForeignKey("SendMethod")]
        public int SendMethodID { get; set; }
        [ForeignKey("LevelOfImpact")]
        public int LevelOfImpactID { get; set; }
        [ForeignKey("NotificationType")]
        public int NotificationTypeID { get; set; }
        [ForeignKey("Priority")]
        public int PriorityID { get; set; }
        [ForeignKey("UserDetail")]
        public string UserID { get; set; }
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string ReferenceID { get; set; }
    }

    public class NotificationType
    {
        [Key]
        public int NotificationTypeID { get; set; }
        public string NotificationTypeName { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }

    public class Priority
    {
        [Key]
        public int PriorityID { get; set; }
        public string PriorityName { get; set; }
        [Index(IsUnique = true)]
        public int PriorityValue { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }

    public class RoleDetail
    {
        [Key, ForeignKey("Role")]
        public string RoleID { get; set; }
        public string RoleDescription { get; set; }
        public virtual Group Group { get; set; }
        public virtual ApplicationRole Role { get; set; }
        [ForeignKey("Group")]
        public int GroupID { get; set; }
    }

    public class SendMethod
    {
        [Key]
        public int SendMethodID { get; set; }
        public string SendMethodName { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }

    public class Server
    {
        [Key]
        public int ServerID { get; set; }
        public string ServerName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual DataCenterLocation DataCenterLocation { get; set; }
        public virtual Status Status { get; set; }
        public virtual ServerType ServerType { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        [ForeignKey("Status")]
        public int StatusID { get; set; }
        [ForeignKey("DataCenterLocation")]
        public int LocationID { get; set; }
        [ForeignKey("ServerType")]
        public int ServerTypeID { get; set; }
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string ReferenceID { get; set; }
    }

    public class ServerType
    {
        [Key]
        public int ServerTypeID { get; set; }
        public string ServerTypeName { get; set; }
        public virtual ICollection<Server> Servers { get; set; }
    }

    public class Status
    {
        [Key]
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Server> Servers { get; set; }
        public virtual StatusType StatusType { get; set; }
        public virtual ICollection<UserDetail> UserDetails { get; set; }
        [ForeignKey("StatusType")]
        public int StatusTypeID { get; set; }
    }

    public class StatusType
    {
        [Key]
        public int StatusTypeID { get; set; }
        public string StatusTypeName { get; set; }
        public virtual ICollection<Status> Statuses { get; set; }
    }

    public class UserDetail
    {
        [Key, ForeignKey("User")]
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessTitle { get; set; }
        public string BusinessPhone { get; set; }
        public string MobilePhone { get; set; }
        public string HomePhone { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual Client Client { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual Status Status { get; set; }
        public virtual ApplicationUser User { get; set; }
        [ForeignKey("Client")]
        public int? ClientID { get; set; }
        [ForeignKey("Status")]
        public int StatusID { get; set; }
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string ReferenceID { get; set; }
    }
}