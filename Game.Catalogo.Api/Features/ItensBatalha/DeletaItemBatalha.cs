using Carter;
using Game.Catalogo.Api.Features.ItensBatalha.Repository;
using Game.Common;
using Game.Common.Events.ItensBatalha;
using Game.Common.Exceptions;
using MassTransit;
using MediatR;

namespace Game.Catalogo.Api.Features.ItensBatalha
{
    public static class DeletaItemBatalha
    {
        public sealed record Command(Guid ItemId) : IRequest<Guid>;
        
        internal sealed class Handler : IRequestHandler<Command, Guid>
        {

            private readonly IUnitOfWork _unitOfWork;
            private readonly IItemBatalhaRepository _itemBatalhaRepository;
            private readonly IPublishEndpoint _publishEndpoint;

            public Handler(IUnitOfWork unitOfWork, IItemBatalhaRepository itemBatalhaRepository, IPublishEndpoint publish)
            {
                _unitOfWork = unitOfWork;
                _itemBatalhaRepository = itemBatalhaRepository;
                _publishEndpoint = publish;
            }

            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var item = await _itemBatalhaRepository.Consulta(item => item.ItemId == request.ItemId, cancellationToken);

                    if (item is null) throw new NotFoundException($"Nenhum Item de ID: {request.ItemId} foi Encontrado");

                    _itemBatalhaRepository.Exclui(item);

                    await _unitOfWork.Commit(cancellationToken);

                    await _publishEndpoint.Publish(new ItemBatalhaExcluidoEvent
                    {
                        ItemId = item.ItemId,
                    }, cancellationToken);

                    return item.ItemId;
                }
                catch (Exception ex)
                {

                    throw new PublishError($"Erro ao tentar publicar a Mensagem: {ex}");
                }
                
            }
        }
    }

    public class DeletaItemBatalhaEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/v1/ItemBatalha/{ItemId}", async (Guid? itemId, ISender sender) => 
            {
                if(itemId is null) return Results.BadRequest("O Id esta vazio");

                var request = new DeletaItemBatalha.Command(itemId.Value);

                var result = await sender.Send(request);

                return Results.Ok(result);
            });
        }
    }
}
