using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Entities;

namespace MoviesApi.Context
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }
        public DbSet<MoviesGenres> MoviesGenres { get; set; }
        public DbSet<MovieTheater> MovieTheaters { get; set; }
        public DbSet<MoviesMovieTheaters> MoviesMovieTheaters { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MoviesGenres>()
                .HasKey(mg => new { mg.MovieId, GenereId = mg.GenreId });


            modelBuilder.Entity<MoviesActors>()
                .HasKey(ma => new { ma.MovieId, Actor = ma.ActorId });

            modelBuilder.Entity<MoviesMovieTheaters>()
                .HasKey(m => new { m.MovieId, m.MovieTheaterId });

            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }


        private void SeedData(ModelBuilder modelBuilder)
        {
            //var rolAdminId = Guid.NewGuid().ToString();
            //var userAdminId = Guid.NewGuid().ToString();

            //var rolAdmin = new IdentityRole()
            //{
            //    Id = rolAdminId,
            //    Name = "Admin",
            //    NormalizedName = "Admin"
            //};

            //var passwordHasher = new PasswordHasher<IdentityUser>();
            //var username = "admin@admin.com";

            //var userAdmin = new IdentityUser()
            //{
            //    Id = userAdminId,
            //    UserName = username,
            //    NormalizedUserName = username,
            //    Email = username,
            //    NormalizedEmail = username,
            //    PasswordHash = passwordHasher.HashPassword(null, "Aa123456!")
            //};

            //modelBuilder.Entity<IdentityUser>()
            //    .HasData(userAdmin);

            //modelBuilder.Entity<IdentityRole>()
            //    .HasData(rolAdmin);

            //modelBuilder.Entity<IdentityUserClaim<string>>()
            //    .HasData(new IdentityUserClaim<string>
            //    {
            //        Id = 1,
            //        ClaimType = ClaimTypes.Role,
            //        UserId = userAdminId,
            //        ClaimValue = "Admin"
            //    });
        }
    }
}
