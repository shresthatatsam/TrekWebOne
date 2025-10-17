using Microsoft.AspNetCore.Mvc;
using UserRoles.Models.Blog;
using UserRoles.Services.Interface;

namespace UserRoles.Controllers
{
    public class BlogController : Controller
    {

        public readonly IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await _blogService.GetAllAsync();
            return View(data);
        }

        // GET: Blog/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var blog = await _blogService.GetByIdAsync(id);
            if (blog == null)
                return NotFound();

            return View(blog);
        }


        public IActionResult Create()
        {
            return View();
        }

        // POST: Blog/Create
        [HttpPost]
        public IActionResult Create(Blog model)
        {
           
                model.CreatedAt = DateTime.UtcNow;
                model.UpdatedAt = DateTime.UtcNow;

                // TODO: Save to DB (e.g. _context.Blogs.Add(model); _context.SaveChanges();)
                _blogService.AddAsync(model);
                TempData["Success"] = "Blog created successfully!";
                return RedirectToAction("Index");
            
            return View(model);
        }
    }
}
