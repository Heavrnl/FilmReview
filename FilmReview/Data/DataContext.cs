using Microsoft.EntityFrameworkCore;
using FilmReview.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FilmReview
{
    public class DataContext: IdentityDbContext<User, Role, long>
    {
        public DbSet<Category> categories { get; set; }
        public DbSet<Country> countries { get; set; }
        public DbSet<Film> films { get; set; }
        public DbSet<Rating> ratings { get; set; }
        public DbSet<Review> reviews { get; set; }
        public DataContext(DbContextOptions<DataContext> options):base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //约束评分大于0小于5
            builder.Entity<Rating>().Property(p=>p.Score).HasAnnotation("CK_Rating_Score", "Score > 0 AND Score <= 5");
            //影评最多1k字
            builder.Entity<Review>().Property(p=>p.Content).HasMaxLength(1000);

           

            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
