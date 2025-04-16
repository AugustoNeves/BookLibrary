namespace BookLibrary.Domain.Entities
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int TotalCopies { get; set; }
        public int CopiesInUse { get; set; }
        public string? Type { get; set; }
        public string? ISBN { get; set; }
        public string? Category { get; set; }
    }
}