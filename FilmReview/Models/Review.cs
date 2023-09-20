﻿namespace FilmReview.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string? Content { get; set; }
        public User? User { get; set; }
        public long UserId { get; set; }


        public long FilmId { get; set; }
        public Film? Film { get; set; } 
    }
}

