using LibraryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Middlewires
{
    public class BookDetailsMiddleware
    {
        private readonly RequestDelegate _next;

        public BookDetailsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext db)
        {
            var path = context.Request.Path.Value?.ToLower();

            // old middleware path
            if (path == "/book-info-old")
            {
                var isbn = context.Request.Query["isbn"].ToString();

                var book = await db.Books
                    .Include(b => b.Author)
                    .FirstOrDefaultAsync(b => b.ISBN == isbn);

                context.Response.ContentType = "text/html";

                if (book == null)
                {
                    await context.Response.WriteAsync("<h2>Book Not Found</h2>");
                    return;
                }

                var html = $@"
                <html>
                    <body style='font-family:Arial;padding:30px;'>
                        <h1>Book Details (Old Middleware)</h1>
                        <p>Title: {book.Title}</p>
                        <p>Author: {book.Author?.Name}</p>
                        <p>ISBN: {book.ISBN}</p>
                    </body>
                </html>";

                await context.Response.WriteAsync(html);
                return;
            }

            await _next(context);
        }
    }
}