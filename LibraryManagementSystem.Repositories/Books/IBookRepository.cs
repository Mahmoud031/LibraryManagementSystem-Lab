using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManagementSystem.Repositories.Books
{
    public interface IBookRepository
    {
        List<Book> GetAll(
            string? searchText = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int? categoryId = null,
            int page = 1,
            int pageSize = 5);

        int GetCount(
            string? searchText = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int? categoryId = null);

        Book? GetDetails(int id);
        Book? GetById(int id);

        void Add(Book book);
        void Update(Book book);
        void Delete(Book book);
        void Save();

        List<SelectListItem> GetCategorySelectList();
        List<SelectListItem> GetAuthorSelectList();
        List<SelectListItem> GetStatusSelectList();
    }
}