using LibraryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace WebApplication1.Middlewires
{
    public class BookListMiddleware
    {
        private readonly RequestDelegate _next;

        public BookListMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext db)
        {
            var path = context.Request.Path.Value?.ToLower();

            // old middleware path
            if (path == "/book-list-old")
            {
                var books = await db.Books
                    .Include(b => b.Author)
                    .ToListAsync();

                var html = new StringBuilder();
                html.Append("<html><body style='font-family:Arial;padding:30px;'>");
                html.Append("<h1>Book List (Old Middleware)</h1>");

                foreach (var book in books)
                {
                    html.Append($"<p>{book.Title} - {book.Author?.Name}</p>");
                }

                html.Append("</body></html>");

                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(html.ToString());
                return;
            }

            await _next(context);
        }
    }
}