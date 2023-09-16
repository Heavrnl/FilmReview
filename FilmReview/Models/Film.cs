namespace FilmReview.Models
{
    public class Film
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Director { get; set; }

        public int Rating { get; set; }

        public int VotesNumber { get; set; }
        public string[]? Actors   { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
