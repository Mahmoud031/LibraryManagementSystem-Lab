using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.ViewModels
{
    public class BookIndexViewModel
    {
        public List<BookListItemViewModel> Books { get; set; } = new();
        public List<SelectListItem> Categories { get; set; } = new();
        public string? SelectedCategory { get; set; }
        public string? CurrentDate { get; set; }
    }
}