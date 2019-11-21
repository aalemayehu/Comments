using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bright_Ideas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Bright_Ideas.Controllers
{
    public class HomeController : Controller
    {
        private MyContext context;
        
        public HomeController(MyContext mc)
        {
            context = mc;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                User userMatchingEmail = context.Users
                    .FirstOrDefault(u => u.Email == user.Email);
                if (userMatchingEmail != null)
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                }
                else 
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    user.Password = Hasher.HashPassword(user, user.Password);
                    context.Users.Add(user);
                    context.SaveChanges();
                    HttpContext.Session.SetInt32("userid", user.UserId);
                return Redirect("/Bright_Ideas");
                }
            }
              return View("Index");
        }
        [HttpPost("login")]
        public IActionResult Login(LoginUser user)
        {
            if(ModelState.IsValid)
            {
                User userMatchingEmail = context.Users
                    .FirstOrDefault(u => u.Email == user.LoginEmail);
                if (userMatchingEmail == null)
                {
                    ModelState.AddModelError("LoginEmail", "Unknown Email!");
                }
                else 
                {
                    PasswordHasher<LoginUser> Hasher = new PasswordHasher<LoginUser>();
                    var result = Hasher.VerifyHashedPassword(user, userMatchingEmail.Password, user.LoginPassword);
                    if (result == 0)
                    {
                        ModelState.AddModelError("LoginPassword", "Incorrect Password!");
                    }
                    else
                    {
                    HttpContext.Session.SetInt32("userid", userMatchingEmail.UserId);
                    return Redirect ("/Bright_Ideas");
                    }
                }
            }
                return View ("Index");
        }
        [HttpGet("Bright_Ideas")]
        public IActionResult Bright_Ideas()
        {
            int? userid = HttpContext.Session.GetInt32("userid");//Since int can't be null, need to use int? to allow it

            User loggedIn = context.Users
                .FirstOrDefault(u => u.UserId == (int) userid);

            if ( userid == null)
            {
                return Redirect("/");
            }
            else
            {
                ViewBag.User = loggedIn;
                ViewBag.loggedIn = userid;
                ViewBag.commentsWithUser = context.Comments
                    .Include(n => n.Creator);
                ViewBag.AllCommentswithLikes = context.Comments
                    .Include(l => l.CommentLikers)
                    .OrderByDescending(a => a.CommentLikers.Count);
                return View("Bright_Ideas");
            }
        }
        [HttpPost("Add")]
        public IActionResult Add(Comment c)
        {
            int? userid = HttpContext.Session.GetInt32("userid");//Since int can't be null, need to use int? to allow it
            User loggedIn = context.Users
                .FirstOrDefault(u => u.UserId == (int) userid);
            if(ModelState.IsValid)
            {
                context.Add(c);
                context.SaveChanges();
                return Redirect("Bright_Ideas");
            } else
            {
                ViewBag.User = loggedIn;
                ViewBag.loggedIn = userid;
                ViewBag.commentsWithUser = context.Comments
                    .Include(n => n.Creator);
                ViewBag.AllCommentswithLikes = context.Comments
                    .Include(l => l.CommentLikers);
                return View("Bright_Ideas");
            }
        }
        [HttpGet("OneUser/{UserId}")]
        public IActionResult OneUser( int UserId)
        {
            int? userid = HttpContext.Session.GetInt32("userid");//Since int can't be null, need to use int? to allow it
            User loggedIn = context.Users.FirstOrDefault(u => u.UserId == (int) userid);
            if ( userid == null)
            {
                return Redirect("/");
            }
            User UserProfile = context.Users
                .Include(c => c.Mycomments)
                .ThenInclude(c => c.CommentLikers)
                .FirstOrDefault(u => u.UserId == (int)UserId);
            int numPosts = context.Users
                .Include(c => c.Mycomments)
                .FirstOrDefault(u => u.UserId == (int)UserId)
                .Mycomments.Count;
            int numLikes = context.Comments
                .Include(c => c.CommentLikers)
                .ThenInclude(c => c.ThatUser)
                .FirstOrDefault(u => u.UserId == (int)UserId)
                .CommentLikers.Count;
            User numeLikes = context.Users
                .Include(u => u.LikedComments)
                .FirstOrDefault(u => u.UserId == (int)UserId);
            ViewBag.numeLikes = numeLikes;
            ViewBag.AllCommentswithLikes = context.Comments
                .Include(l => l.CommentLikers);
            ViewBag.numPosts = numPosts;
            ViewBag.UserProfile = UserProfile;
            return View("OneUser");
        }
        [HttpGet("Like/{UserId}/{CommentId}")]
        public IActionResult Like(int UserId, int CommentId)
        {
            Conversation LikedComment = new Conversation();
            LikedComment.UserId = UserId;
            LikedComment.CommentId = CommentId;
            context.Add(LikedComment);
            context.SaveChanges();
            return RedirectToAction("Bright_Ideas");
        }
        [HttpGet("LikedBy/{CommentId}")]
         public IActionResult LikedBy(int CommentId)
         {
            List<Comment> CommentProfiles = context.Comments
            .Where(c => c.CommentId == CommentId)
            .ToList();
            ViewBag.CommentProfiles = CommentProfiles;
           Comment ACommentwithLikes = context.Comments
                .Include(l => l.CommentLikers)
                .ThenInclude(u => u.ThatUser)
                .FirstOrDefault(u => u.CommentId == (int)CommentId);
            ViewBag.ACommentwithLikes = ACommentwithLikes;

            return View("OneComment");
         }
         [HttpGet("LikedBy/OneUser/{UserId}")]
        public IActionResult LikedByOneUser( int UserId)
        {
            int? userid = HttpContext.Session.GetInt32("userid");//Since int can't be null, need to use int? to allow it
            User loggedIn = context.Users.FirstOrDefault(u => u.UserId == (int) userid);
            if ( userid == null)
            {
                return Redirect("/");
            }
            User UserProfile = context.Users
                .Include(c => c.Mycomments)
                .ThenInclude(c => c.CommentLikers)
                .FirstOrDefault(u => u.UserId == (int)UserId);
            int numPosts = context.Users
                .Include(c => c.Mycomments)
                .FirstOrDefault(u => u.UserId == (int)UserId)
                .Mycomments.Count;
            ViewBag.numPosts = numPosts;
            ViewBag.UserProfile = UserProfile;
            return View("OneUser");
        }
        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
        [HttpGet("Delete/{CommentId}")]
        public IActionResult DeleteComment(int CommentId)
        {
            Comment CommentToDelete = context.Comments.FirstOrDefault(p => p.CommentId == CommentId);
            context.Remove(CommentToDelete);
            context.SaveChanges();
            return RedirectToAction("Bright_Ideas");
        }
    }
}
