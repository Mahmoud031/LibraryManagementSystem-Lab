using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories.Base;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories.Books
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(AppDbContext context) : base(context)
        {
        }

        public List<Book> GetAll(
            string? searchText = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int? categoryId = null,
            int page = 1,
            int pageSize = 5)
        {
            var query = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Attachments)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(b =>
                    b.Title.Contains(searchText) ||
                    (b.ISBN != null && b.ISBN.Contains(searchText)));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(b => b.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(b => b.Price <= maxPrice.Value);
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(b => b.CategoryId == categoryId.Value);
            }

            return query
                .OrderBy(b => b.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetCount(
            string? searchText = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int? categoryId = null)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(b =>
                    b.Title.Contains(searchText) ||
                    (b.ISBN != null && b.ISBN.Contains(searchText)));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(b => b.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(b => b.Price <= maxPrice.Value);
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(b => b.CategoryId == categoryId.Value);
            }

            return query.Count();
        }

        public Book? GetDetails(int id)
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Attachments)
                .FirstOrDefault(b => b.Id == id);
        }

        public override Book? GetById(int id)
        {
            return _context.Books
                .Include(b => b.Attachments)
                .FirstOrDefault(b => b.Id == id);
        }

        public List<SelectListItem> GetCategorySelectList()
        {
            return _context.Categories
                .AsNoTracking()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();
        }

        public List<SelectListItem> GetAuthorSelectList()
        {
            return _context.Authors
                .AsNoTracking()
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                })
                .ToList();
        }

        public List<SelectListItem> GetStatusSelectList()
        {
            return Enum.GetValues(typeof(BookStatus))
                .Cast<BookStatus>()
                .Select(s => new SelectListItem
                {
                    Value = s.ToString(),
                    Text = s.ToString()
                })
                .ToList();
        }
    }
}