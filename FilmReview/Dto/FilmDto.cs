using FilmReview.Models;

namespace FilmReview.Dto
{
    public class FilmDto
    {
        public long FilmId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Director { get; set; }        
        public string? CategoryName { get; set; }
        public DateTime PubDate { get; set; }

        public int? Score { get; set; }
        
        

    }
}
