using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserDAL
    {
        private QuizDBContext context;
        public UserDAL(QuizDBContext context)
        {
            this.context = context;
        }
        public bool RegisterNewUser(User user)
        {
            context.Users.Add(user);
            int result = context.SaveChanges();
            if (result != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Organisation> GetOrganisation()
        {
            return context.Organisations.ToList();
        }
        public int AddOrganisationName(string OrganisationName)
        {
            Organisation organisation = new Organisation
            {
                Name = OrganisationName
            };
            context.Organisations.Add(organisation);
            context.SaveChanges();
            return organisation.Id;
        }
        public bool UpdatePassword(string email, string newPassword)
        {
            var user = context.Users.FirstOrDefault(u => u.EmailId == email);
            if (user != null)
            {
                user.Password = newPassword;
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool DoesUserExist(string email)
        {
            var user = context.Users.FirstOrDefault(u => u.EmailId == email);
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<string> GetAdminEmailsByOrganisation(int id)
        {
            Role role = context.Roles.FirstOrDefault(r => r.Name == "Admin");
            var adminEmails = (from user in context.Users
                               where user.OrganisationId == id && user.RoleId == role.Id
                               select user.EmailId).ToList();

            return adminEmails;
        }
    }
}
