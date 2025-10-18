using Microsoft.AspNetCore.Mvc;
using UserRoles.Data;
using UserRoles.Models.Contact;
using UserRoles.Services.Interface;

namespace UserRoles.Controllers
{
	public class ContactController : Controller
	{
		private readonly AppDbContext _context;

		public ContactController(AppDbContext context)
		{
			_context = context;
		}
		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Submit(Contact model)
		{
			_context.Contacts.Add(model);
			await _context.SaveChangesAsync();

			// TODO: Save to database or send email
			TempData["Success"] = "Thank you for contacting us! We will get back to you soon.";
			if (!string.IsNullOrEmpty(model.Type))
			{
				// Suppose slug comes from the model or some logic
				string slug = model.Type; // or assign the appropriate value
				return RedirectToAction("Details", "TrekPackage", new { slug = slug });
			}

			else
				return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult Details(string status = "active")
		{
			IQueryable<Contact> query = _context.Contacts;

			switch (status.ToLower())
			{
				case "active":
					query = query.Where(x => x.IsActive);
					break;
				case "inactive":
					query = query.Where(x => !x.IsActive);
					break;
				case "all":
				default:
					// No filter, show all
					break;
			}

			ViewBag.SelectedStatus = status;
			var data = query.OrderByDescending(x => x.Id).ToList();
			return View(data);
		}

		[HttpPost]
		public async Task<IActionResult> ToggleActive(int id)
		{
			var contact = await _context.Contacts.FindAsync(id);
			if (contact == null)
				return NotFound();

			contact.IsActive = !contact.IsActive;
			_context.Update(contact);
			await _context.SaveChangesAsync();

			TempData["Success"] = "Status updated successfully.";
			return RedirectToAction(nameof(Details));
		}
	}
}
