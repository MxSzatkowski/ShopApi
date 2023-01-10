using Microsoft.EntityFrameworkCore;

namespace ShopsApi.Entities
{
    public class ShopDbContext : DbContext
    {
        private string _connectionString = "Server=Michal;Database=ShopDb;Trusted_Connection=True;";
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Adress> Adresses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(x => x.Email)
                .IsRequired();
            modelBuilder.Entity<Role>()
                .Property(x => x.Name)
                .IsRequired();
            modelBuilder.Entity<Shop>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(25);
            modelBuilder.Entity<Shop>()
                .Property(x => x.Description)
                .IsRequired();
            modelBuilder.Entity<Adress>()
               .Property(x => x.City)
               .IsRequired();
            modelBuilder.Entity<Adress>()
               .Property(x => x.Street)
               .IsRequired();
            modelBuilder.Entity<Product>()
               .Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(25);
            modelBuilder.Entity<Product>()
               .Property(x => x.Description)
               .IsRequired();
            modelBuilder.Entity<Product>()
               .Property(x => x.Price)
               .IsRequired();

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }

}
