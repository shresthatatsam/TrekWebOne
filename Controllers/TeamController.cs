using Microsoft.AspNetCore.Mvc;
using UserRoles.Data;
using UserRoles.Models;
using UserRoles.Models.Team;

namespace UserRoles.Controllers
{
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(TeamMember model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageFile != null)
                {
                    var uploads = Path.Combine(_env.WebRootPath, "uploads/team");
                    Directory.CreateDirectory(uploads);

                    var fileName = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
                    var filePath = Path.Combine(uploads, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    model.ImageUrl = "/uploads/team/" + fileName;
                }

                _context.TeamMembers.Add(model);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Team member added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var member = _context.TeamMembers.Find(id);
            if (member == null) return NotFound();
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TeamMember model)
        {
            if (ModelState.IsValid)
            {
                var member = _context.TeamMembers.Find(model.Id);
                if (member == null) return NotFound();

                member.FullName = model.FullName;
                member.Position = model.Position;
                member.FacebookUrl = model.FacebookUrl;
                member.TwitterUrl = model.TwitterUrl;
                member.LinkedInUrl = model.LinkedInUrl;
                member.IsActive = model.IsActive;

                if (model.ImageFile != null)
                {
                    var uploads = Path.Combine(_env.WebRootPath, "uploads/team");
                    Directory.CreateDirectory(uploads);

                    var fileName = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
                    var filePath = Path.Combine(uploads, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    member.ImageUrl = "/uploads/team/" + fileName;
                }

                _context.Update(member);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Team member updated!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
