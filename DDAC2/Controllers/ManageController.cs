using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DDAC2.Data;
using DDAC2.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Http;
using DDAC2.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace DDAC2.Controllers
{
 [Authorize(Roles = "Admin")]
    public class ManageController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ManageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Flowers
        public async Task<IActionResult> Index(string searchString, string PostTag)
        {
            // Use LINQ to get list of genres.
            IQueryable<string> TypeQuery = from m in _context.Post
                                           orderby m.Tag
                                           select m.Tag;
            //Flower = await _context.Flower.ToListAsync();
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
            ViewBag.Posts = await posts.ToListAsync();
            return View();
        }

        // GET: Flowers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.ID == id);
            if (post == null)
            {
                return NotFound();
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfigurationRoot Configuration = builder.Build();

            ViewBag.ImgUri = Configuration["ConnectionStrings:BlobStorage"] + post.CoverPhoto;
            return View(post);
        }

        // GET: Flowers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flowers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, int ID, string Title, IFormFile CoverPhoto, string Content, string Tag)
        {
            var post = new Post
            {
                ID = ID,
                Title = Title,
                CoverPhoto = Title,
                Content = Content,
                PublishedDate = DateTime.Now,
                EditedDate = DateTime.Now,
                Tag = Tag
            };

            if (!ModelState.IsValid)
            {
                return View(post);
            }

            Post existingPost = await _context.Post.SingleOrDefaultAsync(
                m => m.Title == post.Title
            );

            if (existingPost != null)
            {
                ModelState.AddModelError(string.Empty, "This blog title is already exists.");
                return View(post);
            }

            BlobsController blobsController = new BlobsController();

            if (!blobsController.UploadBlobFunction(post.Title, CoverPhoto.OpenReadStream()))
            {
                ModelState.AddModelError(string.Empty, "The image file failed to upload.");
                return View(post);
            }
            post.CoverPhoto = post.Title;

            _context.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Flowers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Flowers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int ID, string Title, IFormFile CoverPhoto, string Content, DateTime PublishedDate, string Tag)
        {
            var post = new Post
            {
                ID = ID,
                Title = Title,
                CoverPhoto = Title,
                Content = Content,
                PublishedDate = PublishedDate,
                EditedDate = DateTime.Now,
                Tag = Tag
            };

            if (id != post.ID)
            {
                return NotFound();
            }

            if (CoverPhoto != null)
            {
                BlobsController blobsController = new BlobsController();
                if (!blobsController.UploadBlobFunction(post.Title, CoverPhoto.OpenReadStream()))
                {
                    ModelState.AddModelError(string.Empty, "The image file failed to upload.");
                    return View(post);
                }
                post.CoverPhoto = post.Title;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Flowers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.ID == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Flowers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);

            BlobsController blobsController = new BlobsController();

            if (!blobsController.DeleteBlobFunction(post.CoverPhoto))
            {

            }

            _context.Post.Remove(post);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.ID == id);
        }
    }
}