namespace FilmReview.Models
{
    public class Rating
    {
        public int RatingId { get; set; }
        public User? User { get; set; }
        public Film? Film { get; set; }
        public int Score { get; set; }
    }
}
