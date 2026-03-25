namespace WebApplication1.Dtos.Books
{
    public class BookCardDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int PublishYear { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsRestricted { get; set; }
        public int AttachmentsCount { get; set; }
    }
}