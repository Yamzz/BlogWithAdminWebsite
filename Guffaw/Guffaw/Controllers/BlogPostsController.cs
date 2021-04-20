using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Guffaw.Entities.Entities;
using Guffaw.Models;
using Guffaw.Utilities;
using PagedList;

namespace Guffaw.Controllers
{
    public class BlogPostsController : Controller
    {
        private guffawEntities db;

        public BlogPostsController()
        {
            db = new guffawEntities();
        }


        protected string StripHtml(string Txt)
        {
            return Regex.Replace(Txt, "<(.|\\n)*?>", string.Empty);
        }


        // GET: BlogPosts
        public ActionResult Index(int? page)
        {
            //var pageSize = 7;
            var pageNumber = (page ?? 1);
            return View(db.BlogPosts.OrderBy(m => m.DateTime).ToPagedList(pageNumber, Constants.PageSize));
            //return View(db.BlogPosts.ToList());
        }

        // GET: BlogPosts/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BlogPost blogPost = db.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }

            blogPost.Post = HttpUtility.HtmlDecode(StripHtml(blogPost.Post));
            return View(blogPost);
        }

        // GET: BlogPosts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Post,DateTime,PostedBy,Image,Files")] BlogUploadViewModel blogPost)
        {
            if (ModelState.IsValid)
            {
                if (blogPost.Files[0] != null && blogPost.Files[0].ContentLength > 0)
                {
                    var folder = Server.MapPath("~/Content/Images/blog");
                    var path = Path.Combine(folder, Path.GetFileName(blogPost.Files[0].FileName));
                    blogPost.DateTime = DateTime.Now;

                    //new blog blog entity 
                    var newBlogPost = new BlogPost
                    {
                        Id = blogPost.Id,
                        Title = blogPost.Title,
                        Post = blogPost.Post,
                        PostedBy = blogPost.PostedBy,
                        Description = blogPost.Description,
                        DateTime = blogPost.DateTime,
                        Image = path,
                    };

                    db.BlogPosts.Add(newBlogPost);
                    blogPost.Files[0].SaveAs(path);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }


            var blogPostVm = new BlogUploadViewModel
            {
                Id = blogPost.Id,
                DateTime = blogPost.DateTime,
                ImageURL = blogPost.Image,
                PostedBy = blogPost.PostedBy,
                Post = blogPost.Post,
                Title = blogPost.Title,
                Description = blogPost.Description,
            };

            return View(blogPostVm);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Post,DateTime,PostedBy,ImageURL,Files")] BlogUploadViewModel blogPost)
        {
            if (ModelState.IsValid)
            {
                var folder = Server.MapPath("~/Content/images/blog");
                string path;

                if (blogPost.Files[0] != null && blogPost.Files[0].ContentLength > 0)
                {
                    path = Path.Combine(folder, Path.GetFileName(blogPost.Files[0].FileName));
                    blogPost.Files[0].SaveAs(path);

                    //remove old image maybe 
                }
                else
                {
                    path = blogPost.ImageURL;
                }

                //new blog blog entity 
                var blogPostToUpdate = new BlogPost
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    Post = blogPost.Post,
                    PostedBy = blogPost.PostedBy,
                    Description = blogPost.Description,
                    DateTime = DateTime.Now,
                    Image = path,
                };

                db.Entry(blogPostToUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            blogPost.Post = HttpUtility.HtmlDecode(StripHtml(blogPost.Post));

            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            BlogPost blogPost = db.BlogPosts.Find(id);
            db.BlogPosts.Remove(blogPost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
