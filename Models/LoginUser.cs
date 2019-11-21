using System.ComponentModel.DataAnnotations;

namespace Bright_Ideas.Models
{
    public class LoginUser
    {
        [Required (ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage="Please enter a valid email")]
        public string LoginEmail {get;set;}

        [Required (ErrorMessage="Password is Required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        public string LoginPassword {get;set;}
    }
}