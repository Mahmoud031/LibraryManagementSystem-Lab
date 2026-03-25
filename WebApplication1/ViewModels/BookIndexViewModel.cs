using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.ViewModels
{
    public class BookIndexViewModel
    {
        public List<BookListItemViewModel> Books { get; set; } = new();
        public List<SelectListItem> Categories { get; set; } = new();

        public string? SearchText { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? SelectedCategoryId { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public string? CurrentDate { get; set; }
    }
}