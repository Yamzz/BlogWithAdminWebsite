using Guffaw.Entities.Entities;
using Guffaw.Models;
using Guffaw.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Guffaw.Controllers
{
    public class HomeController : Controller
    {
        private readonly guffawEntities db;

        public HomeController()
        {
            db = new guffawEntities();
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin");
            }

            var homeVM = new HomeViewModel();

            try
            {
                homeVM = new HomeViewModel
                {
                    BlogPostVM = db.BlogPosts.ToList(),
                    ContactVM = new Contact()
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                // use dummy data for testing purposes
                homeVM = GetDummyBlogData();
            }

            return View(homeVM);
        }

        public ActionResult DownloadFile()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "App_Data/";
            var fileBytes = System.IO.File.ReadAllBytes(path + Constants.Leaflet);
            var fileName = Constants.Leaflet;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }


        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(HomeViewModel _homeviewModel)
        {
            if (ModelState.IsValid)
            {
                var newContact = new Contact
                {
                    Email = _homeviewModel.ContactVM.Email,
                    Message = _homeviewModel.ContactVM.Message,
                    Name = _homeviewModel.ContactVM.Name,
                    Phonenumber = _homeviewModel.ContactVM.Phonenumber,
                    DateTime = DateTime.Now
                };
                db.Contacts.Add(newContact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        protected string StripHtml(string Txt)
        {
            return Regex.Replace(Txt, "<(.|\\n)*?>", string.Empty);
        }


        //GET: Blog
        //[Route("Home/BlogPost/{postId?}")] //Route: /Users/Index
        public ActionResult BlogPost(int? postId)
        {
            // use for real database only 
            // var blogPost = db.BlogPosts.Find(postId);

            // test blog object 
            var blogPost = new BlogPost()
            {
                Id = 1,
                Title = "Test first blog post",
                Description = "Blog Post Description 1",
                Post = "I am writing about the blog here",
                PostedBy = "Developer",
                DateTime = DateTime.Now,
                Image = AppDomain.CurrentDomain.BaseDirectory + "Content/Images/blog/Endocannabiniod system .jpg"
            };

            if (blogPost == null)
            {
                return RedirectToAction("Index");
            }
            blogPost.Post = blogPost.Post;
            return View(blogPost);
        }


        public HomeViewModel GetDummyBlogData()
        {
            var homeVM = new HomeViewModel
            {
                BlogPostVM = new List<BlogPost>
                {
                    new BlogPost()
                    {
                        Id = 1,
                        Title = "Test first blog post",
                        Description = "Blog Post Description 1",
                        Post = "I am writing about the blog here",
                        PostedBy = "Developer",
                        DateTime = DateTime.Now,
                        Image = AppDomain.CurrentDomain.BaseDirectory + "Content/Images/blog/Endocannabiniod system .jpg"
                    },
                },
                ContactVM = new Contact()
            };

            return homeVM;
        }

    }
}