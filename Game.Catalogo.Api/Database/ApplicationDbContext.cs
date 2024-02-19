using Game.Catalogo.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Game.Catalogo.Api.Database
{
    public class ApplicationDbContext : DbContext
    {

       // public ApplicationDbContext(){}

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

        public DbSet<ItemBatalha> ItensBatalha { get; set; }
    }
}
