using NotificationPortal.Models;
using NotificationPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace NotificationPortal.Repositories
{
    public class ProfileRepo
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public ProfileVM GetUserDetail(IPrincipal User)
        {
            if (User != null)
            {
                var username = User.Identity.Name;
                var user = _context.UserDetail
                            .Where(u => u.User.UserName == username)
                            .Select(a => new ProfileVM
                            {
                                ReferenceID = a.ReferenceID,
                                BusinessPhone = a.BusinessPhone,
                                BusinessTitle = a.BusinessTitle,
                                FirstName = a.FirstName,
                                LastName = a.LastName,
                                HomePhone = a.HomePhone,
                                MobilePhone = a.MobilePhone,
                            }).FirstOrDefault();
                return user;
            }
            else
            {
                return null;
            }
        }

        public bool EditProfile(ProfileVM model, out string msg)
        {
            UserDetail original = _context.UserDetail.Where(a => a.ReferenceID == model.ReferenceID).FirstOrDefault();
            bool changed = original.BusinessPhone != model.BusinessPhone
                            || original.BusinessTitle != model.BusinessTitle
                            || original.MobilePhone != model.MobilePhone
                            || original.HomePhone != model.HomePhone
                            || original.FirstName != model.FirstName
                            || original.LastName != model.LastName;
            if (changed)
            {
                try
                {
                    UserDetail userUpdated = _context.UserDetail
                                            .Where(a => a.ReferenceID == model.ReferenceID)
                                            .FirstOrDefault();
                    userUpdated.FirstName = model.FirstName;
                    userUpdated.LastName = model.LastName;
                    userUpdated.BusinessTitle = model.BusinessTitle;
                    userUpdated.BusinessPhone = model.BusinessPhone;
                    userUpdated.HomePhone = model.HomePhone;
                    userUpdated.MobilePhone = model.MobilePhone;

                    _context.SaveChanges();
                    msg = "User profile updated.";
                    return true;
                }
                catch
                {
                    msg = "Failed to update user profile.";
                    return false;
                }
            }
            else
            {
                msg = "No actual change to be updated.";
                return false;
            }
        }
    }
}