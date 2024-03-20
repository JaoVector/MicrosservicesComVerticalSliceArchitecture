using Game.Common;
using Game.Common.Events.ItensBatalha;
using Game.Inventario.Api.Features.ItensBatalha.Repository;
using MassTransit;

namespace Game.Inventario.Api.Features.ItensBatalha
{
    public sealed class ItemBatalhaAtualizado : IConsumer<ItemBatalhaAtualizadoEvent>
    {

        private readonly IItemBatalhaRepository _batalhaRepository;
        private readonly IUnitOfWork _uof;

        public ItemBatalhaAtualizado(IItemBatalhaRepository batalhaRepository, IUnitOfWork uof)
        {
            _batalhaRepository = batalhaRepository;
            _uof = uof;
        }

        public async Task Consume(ConsumeContext<ItemBatalhaAtualizadoEvent> context)
        {
            var item = await _batalhaRepository.Consulta(item => item.ItemId == context.Message.ItemId, context.CancellationToken);

            item.Nome = context.Message.Nome;
            item.Descricao = context.Message.Descricao;
            item.Ataque = context.Message.Ataque;
            item.Defesa = context.Message.Defesa;

            _batalhaRepository.Atualiza(item);

            await _uof.Commit(context.CancellationToken);
        }
    }
}
