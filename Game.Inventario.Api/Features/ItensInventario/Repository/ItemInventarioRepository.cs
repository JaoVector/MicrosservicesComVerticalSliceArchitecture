using Game.Common.Exceptions;
using Game.Inventario.Api.Database;
using Game.Inventario.Api.Entities;
using Game.Inventario.Api.Repository;
using Microsoft.EntityFrameworkCore;

namespace Game.Inventario.Api.Features.ItensInventario.Repository
{
    public class ItemInventarioRepository : BaseRepository<ItemInventario>, IItemInventarioRepository
    {

        private readonly ApplicationDbContext _context;

        public ItemInventarioRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<List<ItemInventario>> ConsultaItensInventario(Guid? PersonagemId, CancellationToken cancellationToken)
        {
            try
            {
                var itens = await _context.ItensInventario.
                    Where(p => p.PersonagemId == PersonagemId)
                    .Select(itens => new ItemInventario
                    {
                        ItemIventId = itens.ItemIventId,
                        ItemBatalha = _context.ItensBatalha
                                    .Where(b => b.ItemId == itens.ItemId)
                                    .Select(batalha => new ItemBatalha
                                    {
                                        ItemId = batalha.ItemId,
                                        Nome = batalha.Nome,
                                        Descricao = batalha.Descricao,
                                        Ataque = batalha.Ataque,
                                        Defesa = batalha.Defesa,
                                        ItemCategoria = batalha.ItemCategoria,
                                    }).FirstOrDefault()

                    }).ToListAsync(cancellationToken);

                return itens;
            }
            catch (Exception ex)
            {

                throw new DatabaseException($"Erro ao consultar Itens no Inventario: {ex}");
            }
            
        }

        public async Task<bool> VerificaItemBatalha(Guid ItemId, CancellationToken cancellationToken)
        {
            try
            {
                var item = await _context.ItensBatalha
                       .Where(i => i.ItemId == ItemId)
                       .CountAsync(cancellationToken);

                if (item < 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {

                throw new DatabaseException($"Erro ao consultar Item no Inventario");
            }
        }
    }
}

/*
 * 
 * 
 * public Task<ItemInventario> ConsultaItemInventario(Guid? PersonagemId, CancellationToken cancellationToken)
        {
            var item = _context.ItensIventario
                .Where(p => p.PersonagemId == PersonagemId)
                .Select(item => new ItemInventario 
                {
                    ItemId = item.ItemId,
                    
                });
        }
 * 
 * 
 * 
 */