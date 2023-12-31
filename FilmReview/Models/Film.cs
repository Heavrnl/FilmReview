﻿namespace FilmReview.Models
{
    public class Film
    {
        public long FilmId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Director { get; set; }        
        public Category? Category { get; set; }
        public DateTime PubDate { get; set; }

        public int? Score { get; set; }

        public ICollection<Rating>? Ratings { get; set; }
        public ICollection<Review>? Reviews { get; set; }

        

    }
}
