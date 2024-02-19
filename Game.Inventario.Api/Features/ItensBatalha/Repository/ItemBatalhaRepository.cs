using Game.Inventario.Api.Database;
using Game.Inventario.Api.Entities;
using Game.Inventario.Api.Repository;

namespace Game.Inventario.Api.Features.ItensBatalha.Repository
{
    public class ItemBatalhaRepository : BaseRepository<ItemBatalha>, IItemBatalhaRepository
    {
        public ItemBatalhaRepository(ApplicationDbContext context) : base(context) { }
    }
}
