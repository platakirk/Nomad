using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nomad.Models
{
    public class UserModel
    {
        [Key]
        [Display(Name = "User Identification")]
        public int UserID { get; set; }

        [Display(Name = "Type of User")]
        [Required(ErrorMessage = "Please enter your user type")]
        public int TypeID { get; set; }

        public string Type { get; set; }

        public List<UserTypeModel> Types { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter your First name")]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [Required(ErrorMessage = "Please enter your Middle name")]
        [MaxLength(30)]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter your Last name")]
        [MaxLength(30)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please create a username")]
        [MaxLength(40)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [RegularExpression(@"^09(73|74|05|06|15|16|17|26|27|35|36|37|79|38|07|08|09|10|12|18|19|20|21|28|29|30|38|39|89|99|22|23|32|33)\d{3}\s?\d{4}", ErrorMessage = "Invalid Format")]
        [MaxLength(30)]
        public string Mobile { get; set; }

        [MaxLength(20)]
        public string Status { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Last Login Date")]
        public DateTime? LastLogin { get; set; }

    }

    public class UserTypeModel
    {
        [Key]
        [Display(Name = "User Identification")]
        public int TypeID { get; set; }

        [Display(Name = "Type of User")]
        public string Type { get; set; }
    }
}