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
	}
}
