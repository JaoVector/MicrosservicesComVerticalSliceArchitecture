using Game.Common;
using Game.Common.Events.ItensBatalha;
using Game.Inventario.Api.Entities;
using Game.Inventario.Api.Features.ItensBatalha.Repository;
using MassTransit;

namespace Game.Inventario.Api.Features.ItensBatalha
{
    public sealed class ItemBatalhaCriado : IConsumer<ItemBatalhaCriadoEvent>
    {
        private readonly IItemBatalhaRepository _batalhaRepository; 
        private readonly IUnitOfWork _uof;

        public ItemBatalhaCriado(IItemBatalhaRepository batalhaRepository, IUnitOfWork work)
        {
            _batalhaRepository = batalhaRepository;
            _uof = work;
        }

        public async Task Consume(ConsumeContext<ItemBatalhaCriadoEvent> context)
        {
            var item = new ItemBatalha 
            {
                ItemId = context.Message.ItemId,
                Nome= context.Message.Nome,
                Descricao= context.Message.Descricao,
                Ataque= context.Message.Ataque,
                Defesa= context.Message.Defesa,
                ItemCategoria= context.Message.ItemCategoria,
            };

            _batalhaRepository.Cria(item);

            await _uof.Commit(context.CancellationToken);
        }
    }
}
