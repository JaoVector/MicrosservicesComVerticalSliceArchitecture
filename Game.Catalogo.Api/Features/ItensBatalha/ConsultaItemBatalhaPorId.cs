using Carter;
using Game.Catalogo.Api.Contracts.ItemBatalha;
using Game.Catalogo.Api.Features.ItensBatalha.Repository;
using Game.Common.Exceptions;
using Mapster;
using MediatR;

namespace Game.Catalogo.Api.Features.ItensBatalha
{
    public static class ConsultaItemBatalhaPorId
    {
        public sealed record Query(Guid ItemId) : IRequest<ItemBatalhaResponse>;

        internal sealed class Handler : IRequestHandler<Query, ItemBatalhaResponse>
        {

            private readonly IItemBatalhaRepository _batalhaRepository;

            public Handler(IItemBatalhaRepository itemBatalha)
            {
                _batalhaRepository = itemBatalha;
            }

            public async Task<ItemBatalhaResponse> Handle(Query request, CancellationToken cancellationToken)
            {

                var result = await _batalhaRepository.Consulta(item => item.ItemId == request.ItemId, cancellationToken);

                if (result is null) throw new NotFoundException($"Nenhum Item de ID: {request.ItemId} foi Encontrado");

                var item = result.Adapt<ItemBatalhaResponse>();

                return item;
            }
        }
    }

    public class ConsultaItemEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/ItemBatalha/{ItemId}", async (Guid? itemId, ISender sender) => 
            {

                if (itemId is null) return Results.BadRequest("O Id esta vazio");

                var query = new ConsultaItemBatalhaPorId.Query(itemId.Value);

                var result = await sender.Send(query);

                if (result is null) return Results.NotFound("Item Não encontrado");

                return Results.Ok(result);
            });
        }
    }
}
