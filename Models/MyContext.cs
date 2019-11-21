using Microsoft.EntityFrameworkCore;
using Bright_Ideas.Models;
using System;

namespace Bright_Ideas.Models {
    public class MyContext : DbContext {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<Comment> Comments {get;set;}
        public DbSet<User> Users {get;set;}
        public DbSet<Conversation> Conversations {get;set;}
    }
}