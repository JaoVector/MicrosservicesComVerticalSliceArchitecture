using Carter;
using Game.Common.Exceptions;
using Game.Inventario.Api.Contracts.ItemInventario;
using Game.Inventario.Api.Contracts.ItensBatalha;
using Game.Inventario.Api.Features.ItensInventario.Repository;
using MediatR;

namespace Game.Inventario.Api.Features.ItensInventario
{
    public static class ConsutaItensInventario
    {
        public sealed record Query(Guid PersonagemId) : IRequest<List<ItensInventarioResponse>>;
        
        internal sealed class Handler : IRequestHandler<Query, List<ItensInventarioResponse>>
        {
            private readonly IItemInventarioRepository _inventarioRepository;

            public Handler(IItemInventarioRepository inventarioRepository)
            {
                _inventarioRepository = inventarioRepository;
            }

            public async Task<List<ItensInventarioResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var itens = await _inventarioRepository.ConsultaItensInventario(request.PersonagemId, cancellationToken);

                if (itens is null) throw new NotFoundException($"Não foram encontrados itens para o Personagem de ID: {request.PersonagemId}");

                return (from item in itens select new ItensInventarioResponse 
                {
                    ItemIventId = item.ItemIventId,
                    ItemBatalha = new ItemBatalhaResponse 
                    {
                        Nome = item.ItemBatalha.Nome,
                        Descricao = item.ItemBatalha.Descricao,
                        Ataque = item.ItemBatalha.Ataque,
                        Defesa = item.ItemBatalha.Defesa,
                        ItemCategoria = item.ItemBatalha.ItemCategoria
                    }
                }).ToList();
            }
        }
    }

    public class ConsultaItensInventarioEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/ItemInventario/{PersonagemId}", async (Guid? personagemId, ISender sender) =>
            {

                if (personagemId is null) return Results.BadRequest("O Id esta vazio");

                var request = new ConsutaItensInventario.Query(personagemId.Value);

                var result = await sender.Send(request);

                if (result is null) return Results.NotFound("Não Foi Possível Encontrar o PersonagemId");

                return Results.Ok(result);
            });
        }
    }
}
