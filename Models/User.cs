using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bright_Ideas.Models 
{
     public class User
    {
        [Key]
        public int UserId {get;set;}

        [Required (ErrorMessage = "Name is required!")]
        [MinLength(2, ErrorMessage = "Name must be 2 characters or more!")]
        [RegularExpression(@"^[a-zA-Z""'\s]*$", ErrorMessage = "Name Must be in correct format!:)")]
        public string Name {get;set;}

        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$", ErrorMessage = "Alias Must be in the correct format!:)")]
        [Required (ErrorMessage = "Alias is required!")]
        public string Alias {get;set;}

        [Required (ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage="Please enter a valid email")]
        public string Email {get;set;}

        [Required (ErrorMessage="Password is Required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        public string Password {get;set;}

        public List<Conversation> LikedComments {get;set;} //Ask What do you get access to?
        public List<Comment> Mycomments {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        [NotMapped]
        [Compare("Password", ErrorMessage="Confirm Password must match Password")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}
    }
}