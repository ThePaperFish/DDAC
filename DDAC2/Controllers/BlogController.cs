using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDAC2.Data;
using DDAC2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HeyRed.MarkdownSharp;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace DDAC2.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context; 
        private readonly UserManager<IdentityUser> _userManager;
        public BlogController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string searchString, string PostTag)
        {
            IQueryable<string> TypeQuery = from m in _context.Post
                                           orderby m.Tag
                                           select m.Tag;

            var posts = from m in _context.Post
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(s => s.Title.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(PostTag))
            {
                posts = posts.Where(x => x.Tag == PostTag);
            }

            IEnumerable<SelectListItem> items = new SelectList(await TypeQuery.Distinct().ToListAsync());
            ViewBag.PostTag = items;

            IList<Post> Posts = await posts.ToListAsync();
            ViewBag.Posts = Posts;

            return View(Posts);
        }

        public async Task<IActionResult> Detail(int? id)
        {

            if (id == null)
                return NotFound();

            Post post = await _context.Post.FirstOrDefaultAsync(m => m.ID == id);
            if (post == null)
                return NotFound();

            Markdown mark = new Markdown();
            string text = mark.Transform(post.Content);
            post.Content = text;

            var comments = from m in _context.Comment
                           where m.PostId == id
                           orderby m.PublishedDate descending
                           select m;
            IList<Comment> Comments = await comments.ToListAsync();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfigurationRoot Configuration = builder.Build();
            ViewBag.ImgUri = Configuration["ConnectionStrings:BlobStorage"] + post.CoverPhoto;

            ViewBag.Username = this.User.Identity.Name;
            ViewBag.post = post;
            ViewBag.Comments = Comments;
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detail([Bind("PostId,AuthorId,Content")] Comment comment)
        {
            comment.EditedDate = DateTime.Now;
            comment.PublishedDate = DateTime.Now;

            _context.Comment.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Detail");
        }

    }
}