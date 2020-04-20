using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDAC2.Data;
using DDAC2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DDAC2.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BlogController(ApplicationDbContext context)
        {
            _context = context;
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

            var comments = from m in _context.Comment
                           where m.PostId == id
                           orderby m.PublishedDate descending
                           select m;
            IList<Comment> Comments = await comments.ToListAsync();

            ViewBag.post = post;
            ViewBag.Comments = Comments;
            return View();
        }

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