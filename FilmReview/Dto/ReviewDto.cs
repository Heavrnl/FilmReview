namespace FilmReview.Dto
{
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public string? Content { get; set; }
        public long UserId { get; set; }

        public long FilmId { get; set; } 
    }
}

