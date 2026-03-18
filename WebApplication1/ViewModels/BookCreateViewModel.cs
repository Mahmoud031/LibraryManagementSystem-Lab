using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class BookCreateViewModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? ISBN { get; set; }

        [Required]
        public int PublishYear { get; set; }

        [Required]
        public decimal Price { get; set; }

        public bool IsRestricted { get; set; }

        [Required]
        public BookStatus Status { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<IFormFile>? Files { get; set; }

        public List<SelectListItem> Authors { get; set; } = new();
        public List<SelectListItem> Categories { get; set; } = new();
        public List<SelectListItem> Statuses { get; set; } = new();
    }
}