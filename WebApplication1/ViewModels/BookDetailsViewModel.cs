namespace WebApplication1.ViewModels
{
    public class BookDetailsViewModel
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ISBN { get; set; }
        public int PublishYear { get; set; }
        public decimal Price { get; set; }
        public bool IsRestricted { get; set; }
        public string Status { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string? CurrentDate { get; set; }
        public List<BookAttachmentViewModel> Attachments { get; set; } = new();
    }
}