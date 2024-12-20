namespace BookStore.DTOs.Author
{
    public class DisplayAuthorDTO
    {
        public int AuthorId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? Age { get; set; }
        public string? Bio { get; set; }
        public int? NumberOfBooks { get; set; }
    }
}
