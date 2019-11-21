using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bright_Ideas.Models
{
    public class Comment
    {
        [Key]
        public int CommentId {get;set;}

        [Required(ErrorMessage = "Comment is Required")]
        [MinLength(5, ErrorMessage = "Comment must be 5 characters or more!")]
        public string Message {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public int UserId {get;set;}
        public User Creator {get;set;}

        public List<Conversation> CommentLikers {get;set;}//Navigation Property//Ask What do you get access to?
    }
}