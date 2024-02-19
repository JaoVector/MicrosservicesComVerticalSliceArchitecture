using Game.Common;
using Game.Inventario.Api.Entities;
using System.Linq.Expressions;

namespace Game.Inventario.Api.Features.ItensInventario.Repository
{
    public interface IItemInventarioRepository : IBaseRepository<ItemInventario>
    {
        Task<List<ItemInventario>> ConsultaItensInventario(Guid? PersonagemId, CancellationToken cancellationToken);
        Task<bool> VerificaItemBatalha(Guid ItemId, CancellationToken cancellationToken);
    }
}
