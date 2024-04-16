using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? EmailId { get; set; }
        public string? Password { get; set; }
        public int OrganisationId { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public Organisation? Organisation { get; set; } // Navigation property for Organisation
        public Role? Role { get; set; } // Navigation property for Role
    }
}
