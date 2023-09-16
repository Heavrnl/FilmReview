namespace FilmReview.Models
{
    public class Country
    {
        public int CountryId { get; set; }
        public string? Name { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}
