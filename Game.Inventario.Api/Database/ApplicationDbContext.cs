using Game.Inventario.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Game.Inventario.Api.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ItemBatalha> ItensBatalha { get; set; }
        public DbSet<ItemInventario> ItensInventario { get; set; }

        protected override void OnModelCreating(ModelBuilder model) 
        {
            model.Entity<ItemInventario>()
                .HasKey(x => x.ItemIventId);

            model.Entity<ItemInventario>()
                .HasOne(item => item.ItemBatalha)
                .WithMany()
                .HasForeignKey(item => item.ItemId);
        }
    }
}
