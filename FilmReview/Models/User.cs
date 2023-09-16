using Microsoft.AspNetCore.Identity;

namespace FilmReview.Models
{
    public class User : IdentityUser<long>
    {
        public DateTime CreateTime { get; set; }
        public Country? Country { get; set; }
        public ICollection<Rating>? Ratings { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}