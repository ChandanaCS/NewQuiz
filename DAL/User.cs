using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Name must contain only letters and spaces")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? EmailId { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long and not more than 15")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$",
        ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character")]
        public string? Password { get; set; }
        public int OrganisationId { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        public string? PhoneNumber { get; set; }
        [DefaultValue("false")]
        public bool IsApproved { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public Organisation? Organisation { get; set; } // Navigation property for Organisation
        public Role? Role { get; set; } // Navigation property for Role
    }
}
