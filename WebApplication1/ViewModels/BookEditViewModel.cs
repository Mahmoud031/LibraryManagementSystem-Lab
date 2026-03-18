using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class BookEditViewModel
    {
        public int Id { get; set; }

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

        public byte[] Version { get; set; } = Array.Empty<byte>();

        public List<IFormFile>? Files { get; set; }

        public List<BookAttachmentViewModel> ExistingAttachments { get; set; } = new();

        public List<SelectListItem> Authors { get; set; } = new();
        public List<SelectListItem> Categories { get; set; } = new();
        public List<SelectListItem> Statuses { get; set; } = new();
    }
}