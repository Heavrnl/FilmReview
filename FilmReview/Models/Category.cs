namespace FilmReview.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }

        public ICollection<Film>? Films { get; set; }
    }
}
