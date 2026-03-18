namespace WebApplication1.ViewModels
{
    public class BookListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? ISBN { get; set; }
        public int PublishYear { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsRestricted { get; set; }
        public string Status { get; set; } = string.Empty;
        public int AttachmentsCount { get; set; }
    }
}