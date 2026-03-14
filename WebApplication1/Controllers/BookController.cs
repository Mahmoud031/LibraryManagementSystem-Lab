using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class BookController : Controller
    {
        private readonly AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            TempData["CurrentDate"] = DateTime.Now.ToString("dddd, MMMM dd, yyyy");

            var books = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .AsNoTracking()
                .ToList();

            var categories = _context.Categories
                .AsNoTracking()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();

            categories.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "All columns"
            });

            var vm = new BookIndexViewModel
            {
                Books = books,
                Categories = categories,
                CurrentDate = TempData["CurrentDate"]?.ToString()
            };

            TempData.Keep("CurrentDate");

            return View(vm);
        }

        public IActionResult Details(int id)
        {
            var book = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .AsNoTracking()
                .FirstOrDefault(b => b.Id == id);

            if (book == null)
                return NotFound();

            var vm = new BookDetailsViewModel
            {
                Book = book,
                CurrentDate = TempData["CurrentDate"]?.ToString()
            };

            TempData.Keep("CurrentDate");

            return View(vm);
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            return View();
        }
    }
}