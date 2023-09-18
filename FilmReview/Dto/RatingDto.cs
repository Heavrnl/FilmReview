namespace FilmReview.Dto
{
    public class RatingDto
    {
        public int RatingId { get; set; }
        public long FilmId { get; set; }
        public long UserId { get; set; }

        
        public int Score { get; set; }
    }
}
