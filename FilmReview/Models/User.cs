using Microsoft.AspNetCore.Identity;

namespace FilmReview.Models
{
    public class User : IdentityUser<long>
    {
        public DateTime CreateTime { get; set; }
    }
}
