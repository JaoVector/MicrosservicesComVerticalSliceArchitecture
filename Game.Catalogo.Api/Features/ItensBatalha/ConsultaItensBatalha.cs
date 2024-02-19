using Carter;
using Game.Catalogo.Api.Contracts.ItemBatalha;
using Game.Catalogo.Api.Features.ItensBatalha.Repository;
using Game.Common.Exceptions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Game.Catalogo.Api.Features.ItensBatalha
{
    public static class ConsultaItensBatalha
    {
        public sealed record Query(int skip, int take) : IRequest<List<ItemBatalhaResponse>>;

        internal sealed class Handler : IRequestHandler<Query, List<ItemBatalhaResponse>>
        {

            private readonly IItemBatalhaRepository _batalhaRepository;

            public Handler(IItemBatalhaRepository batalhaRepository)
            {
                _batalhaRepository = batalhaRepository;
            }

            public async Task<List<ItemBatalhaResponse>> Handle(Query request, CancellationToken cancellationToken)
            {

                var itens = await _batalhaRepository.ConsultaTodos(request.skip, request.take, cancellationToken);

                if (itens is null) throw new NotFoundException($"Nenhum Item foi Encontrado"); ;
                
                var itensResp = itens.Adapt<List<ItemBatalhaResponse>>();

                return itensResp;
            }
        }
    }

    public class ConsultaItensBatalhaEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/ItemBatalha", async ([FromQuery] int skip, [FromQuery] int take, ISender sender) => 
            {
                var request = new ConsultaItensBatalha.Query(skip, take);

                var result = await sender.Send(request);

                if (result is null) return Results.NotFound("Não foi encontrado nenhum Item de Batalha");

                return Results.Ok(result);
            });
        }
    }
}
