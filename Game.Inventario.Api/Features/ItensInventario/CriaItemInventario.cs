using Carter;
using Game.Common;
using Game.Common.Exceptions;
using Game.Inventario.Api.Entities;
using Game.Inventario.Api.Features.ItensInventario.Repository;
using MediatR;

namespace Game.Inventario.Api.Features.ItensInventario
{
    public static class CriaItemInventario
    {
        public sealed record Command : IRequest<Guid> 
        {
            public Guid ItemId { get; set; }
            public Guid PersonagemId { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Command, Guid>
        {
            private readonly IItemInventarioRepository _itemInventarioRepository;
            private readonly IUnitOfWork _uof;

            public Handler(IItemInventarioRepository itemInventarioRepository, IUnitOfWork uof)
            {
                _itemInventarioRepository = itemInventarioRepository;
                _uof = uof;
            }

            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {

                if (!await _itemInventarioRepository.VerificaItemBatalha(request.ItemId, cancellationToken)) 
                {
                    throw new NotFoundException($"Não foi econtrado nenhum Item com ID: {request.ItemId}");
                }

                var itemInventario = new ItemInventario
                {
                    ItemIventId= Guid.NewGuid(),
                    ItemId = request.ItemId,
                    PersonagemId = request.PersonagemId,
                };

                _itemInventarioRepository.Cria(itemInventario);

                await _uof.Commit(cancellationToken);

                return itemInventario.ItemIventId;
            }
        }
    }

    public class CriaItemInventarioEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/ItemInventario", async (CriaItemInventario.Command request, ISender sender) => 
            {
                var result = await sender.Send(request);

                return Results.Ok(result);
            });
        }
    }
}
