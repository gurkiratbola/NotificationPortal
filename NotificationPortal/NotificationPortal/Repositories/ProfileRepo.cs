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
        public const string USERNAME_UPDATED = "Username changed";

        // get user profile detail to display for "GET" method
        public ProfileVM GetUserDetail(IPrincipal User)
        {
            if (User != null)
            {
                var username = User.Identity.Name;
                string referenceID = _context.UserDetail.Where(u => u.User.UserName == username).SingleOrDefault().ReferenceID;
                ProfileVM user = _context.UserDetail
                            .Where(u => u.ReferenceID == referenceID)
                            .Select(a => new ProfileVM
                            {
                                ReferenceID = a.ReferenceID,
                                BusinessPhone = a.BusinessPhone,
                                BusinessTitle = a.BusinessTitle,
                                FirstName = a.FirstName,
                                Email = a.User.Email,
                                LastName = a.LastName,
                                HomePhone = a.HomePhone,
                                MobilePhone = a.MobilePhone,
                                SendMethodID = a.SendMethodID
                            }).FirstOrDefault();

                return user;
            }
            else
            {
                return null;
            }
        }
        // save user profile changes to display for "POST" method
        public bool EditProfile(ProfileVM model, out string msg)
        {
            UserDetail original = _context.UserDetail.Where(a => a.ReferenceID == model.ReferenceID).FirstOrDefault();
            var email = original.User.Email;
            bool changed = original.BusinessPhone != model.BusinessPhone
                            || original.BusinessTitle != model.BusinessTitle
                            || original.MobilePhone != model.MobilePhone
                            || original.HomePhone != model.HomePhone
                            || original.FirstName != model.FirstName
                            || original.LastName != model.LastName
                            || original.SendMethodID != model.SendMethodID
                            || original.User.Email != model.Email;
            if (changed)
            {
                bool emailAvailable = true;
                //check if the email already exist in the database
                if (original.User.Email != model.Email)
                {
                    var user = _context.Users.Where(a => a.Email == model.Email).FirstOrDefault();
                    if (user != null && user.UserDetail.ReferenceID != model.ReferenceID)
                    {
                        emailAvailable = false;
                    }
                }

                // if SendMethod is not email then mobile phone must be defined
                int sendMethodEmail = _context.SendMethod.Where(m => m.SendMethodName == Key.SEND_METHOD_EMAIL).FirstOrDefault().SendMethodID;
                if (model.SendMethodID != sendMethodEmail && model.MobilePhone == null)
                {
                    msg = "You current preference requires mobile number.";
                    return false;
                }

                if (emailAvailable)
                {
                    try
                    {
                        UserDetail userUpdated = _context.UserDetail
                                                .Where(a => a.ReferenceID == model.ReferenceID)
                                                .FirstOrDefault();
                        userUpdated.FirstName = model.FirstName;
                        userUpdated.LastName = model.LastName;
                        userUpdated.User.Email = model.Email;
                        userUpdated.User.UserName = model.Email;
                        userUpdated.BusinessTitle = model.BusinessTitle;
                        userUpdated.BusinessPhone = model.BusinessPhone;
                        userUpdated.HomePhone = model.HomePhone;
                        userUpdated.MobilePhone = model.MobilePhone;
                        userUpdated.SendMethodID = model.SendMethodID;

                        _context.SaveChanges();

                        if (email != model.Email)
                        {
                            // if the username/email has been updated successfully
                            msg = USERNAME_UPDATED;
                        }
                        else
                        {
                            msg = "User profile updated.";
                        }
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
                    msg = "This email is associated with another user.";
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