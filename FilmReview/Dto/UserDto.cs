using Microsoft.AspNetCore.Identity;

namespace FilmReview.Dto
{
    public class UserDto
    {
        public string? UserName { get; set; }

        public long UserId { get; set; }
        public string? Email { get; set; }
        public DateTime CreateTime { get; set; }
        public int CountryId { get; set; }

    }
}