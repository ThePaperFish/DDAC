using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DDAC2.Data;
using DDAC2.Models;
using DDAC2.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace DDAC2.Views.ManageProjects
{
    [Authorize(Roles="Admin")]
    public class ManageProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManageProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ManageProjects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Project.ToListAsync());
        }

        // GET: ManageProjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: ManageProjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ManageProjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int ID,string Title,string Description, IFormFile CoverPhoto)
        {
            var project = new Project
            {
                ID = ID,
                Title = Title,
                Description = Description,
                CoverPhoto = "",
                EditedDate = DateTime.Now,
            };

            if (!ModelState.IsValid)
            {
                return View();
            }

            BlobsController blobsController = new BlobsController();
            if (!blobsController.UploadBlobFunction("Content-" + project.Title, CoverPhoto.OpenReadStream()))
            {
                ModelState.AddModelError(string.Empty, "The image file failed to upload.");
                return View();
            }
            project.CoverPhoto = "Content-" + project.Title;

            project.EditedDate = DateTime.Now;
            _context.Add(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ManageProjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: ManageProjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int ID, string Title, string Description, IFormFile CoverPhoto)
        {
            if (id != ID)
            {
                return NotFound();
            }

            var project = new Project
            {
                ID = ID,
                Title = Title,
                EditedDate = DateTime.Now,
            };

            if (!ModelState.IsValid)
            {
                return View(project);
            }

            if (CoverPhoto != null)
            {
                BlobsController blobsController = new BlobsController();
                if (!blobsController.UploadBlobFunction("Content-" + project.Title, CoverPhoto.OpenReadStream()))
                {
                    ModelState.AddModelError(string.Empty, "The image file failed to upload.");
                    return View();
                }
                project.CoverPhoto = "Content-" + project.Title;
            }

            try
            {
                _context.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(project.ID))
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

        // GET: ManageProjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project
                .FirstOrDefaultAsync(m => m.ID == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: ManageProjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Project.FindAsync(id);
            _context.Project.Remove(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ID == id);
        }
    }
}
