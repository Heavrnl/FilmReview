namespace FilmReview.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int UserId { get; set;}
        public User? User { get; set; } // 用户与评论之间的导航属性
        public int FilmId { get; set; }
        public Film? Film { get; set; } // 电影与评论之间的导航属性
    }
}

