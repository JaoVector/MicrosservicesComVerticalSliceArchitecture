using Game.Catalogo.Api.Database;
using Game.Catalogo.Api.Entities;
using Game.Catalogo.Api.Repository;
using System.Linq.Expressions;

namespace Game.Catalogo.Api.Features.ItensBatalha.Repository
{
    public class ItemBatalhaRepository : BaseRepository<ItemBatalha>, IItemBatalhaRepository
    {
        public ItemBatalhaRepository(ApplicationDbContext context) : base(context) { }
    }
}
