using Game.Common;
using Game.Common.Events.ItensBatalha;
using Game.Inventario.Api.Features.ItensBatalha.Repository;
using MassTransit;

namespace Game.Inventario.Api.Features.ItensBatalha
{
    public sealed class ItemBatalhaExcluido : IConsumer<ItemBatalhaExcluidoEvent>
    {
        private readonly IItemBatalhaRepository _batalhaRepository;
        private readonly IUnitOfWork _uof;

        public ItemBatalhaExcluido(IItemBatalhaRepository batalhaRepository, IUnitOfWork uof)
        {
            _batalhaRepository = batalhaRepository;
            _uof = uof;
        }

        public async Task Consume(ConsumeContext<ItemBatalhaExcluidoEvent> context)
        {
            var item = await _batalhaRepository.Consulta(item => item.ItemId == context.Message.ItemId, context.CancellationToken);

            _batalhaRepository.Exclui(item);

            await _uof.Commit(context.CancellationToken);
        }
    }
}
