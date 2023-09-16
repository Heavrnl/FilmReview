using Microsoft.EntityFrameworkCore;

using Microsoft.AspNet.Identity.EntityFramework;

namespace FilmReview
{
    public class DataContext: IdentityDbContext<User, Role, long>
    {
        public DataContext(DbContextOptions<DataContext> options):base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
