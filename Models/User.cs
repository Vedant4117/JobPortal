using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter your name")]
        public string? Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Enter a valid email Id")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        public string? Role { get; set; }

        public DateTime CreatedOn  { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }

        public string? Description { get; set; }
        public string? Skills { get; set; }
        public string? Experience { get; set; }
        public string? DOB { get; set; }
        public string? JobRole { get; set; }
        public string? CName { get; set; }
        public string? IType { get; set; }
        public string? Address { get; set; }
        public string? CSize { get; set; }
        public string? ContactNo { get; set; }  

        public string? Status { get; set; }

    }
}
