using LibraryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Middlewires;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        "Server=.;Database=LibraryManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<RequestLogMiddleware>();
app.UseMiddleware<BookListMiddleware>();
app.UseMiddleware<BookDetailsMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapFallback(async context =>
{
    context.Response.ContentType = "text/html";

    await context.Response.WriteAsync(@"
        <html>
            <body style='font-family:Arial;padding:40px;background:#f8f9fa'>
                <h1 style='color:red'>Library System - Resource Not Found</h1>
                <p>The requested resource does not match any route or middleware path.</p>
            </body>
        </html>");
});

app.Run();