using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Dtos.Books;

namespace WebApplication1.ViewComponents
{
    public class RelatedBooksViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public RelatedBooksViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int currentBookId, int categoryId, int authorId, int publishYear)
        {
            var relatedBooks = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .AsNoTracking()
                .Where(b =>
                    b.Id != currentBookId &&
                    (
                        b.CategoryId == categoryId ||
                        b.AuthorId == authorId ||
                        b.PublishYear == publishYear
                    ))
                .OrderByDescending(b => b.CategoryId == categoryId)
                .ThenByDescending(b => b.AuthorId == authorId)
                .ThenByDescending(b => b.PublishYear == publishYear)
                .Take(4)
                .Select(b => new RelatedBookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = b.Author != null ? b.Author.Name : string.Empty,
                    CategoryName = b.Category != null ? b.Category.Name : string.Empty,
                    PublishYear = b.PublishYear,
                    Price = b.Price,
                    Status = b.Status.ToString(),
                    IsRestricted = b.IsRestricted
                })
                .ToList();
            return View(relatedBooks);
        }
    }
}