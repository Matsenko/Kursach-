using Kursach.Models;
using Microsoft.EntityFrameworkCore;

namespace Kursach.Data
{
    public class KursachContext: DbContext
    {
        public KursachContext()
        {

        }
        public KursachContext(DbContextOptions<KursachContext> options) : base(options)
        {
        }
        public DbSet<MbModel> Currencies { get; set; }
        public DbSet<UserModel> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseSqlServer("Data Source=LAPTOP-C1HR1LSD\\ZENON_2012;Initial Catalog=Kursach4;Trusted_Connection=True;TrustServerCertificate=True;");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MbModel>().HasKey(x => x.Id);
            modelBuilder.Entity<UserModel>().HasKey(x => x.Id);
        }
    }
}
