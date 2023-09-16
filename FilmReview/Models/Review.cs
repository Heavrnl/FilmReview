﻿namespace FilmReview.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public User? User { get; set; } 

        public Film? Film { get; set; } 
    }
}

