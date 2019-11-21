using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Bright_Ideas.Models 
{
    public class Conversation
    {
        [Key]
        public int ConversationId {get;set;}
        public int UserId {get;set;}
        public User ThatUser {get;set;}//Navigation Property
        public int CommentId {get;set;}
        public Comment ThatComment {get;set;}//Navigation Property
    }
}