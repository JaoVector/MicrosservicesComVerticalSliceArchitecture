using Carter;
using Game.Catalogo.Api.Contracts.ItemBatalha;
using Game.Catalogo.Api.Database;
using Game.Catalogo.Api.Entities;
using Game.Catalogo.Api.Features.ItensBatalha.Repository;
using Game.Common;
using Game.Common.Enum;
using Game.Common.Events.ItensBatalha;
using Game.Common.Exceptions;
using Mapster;
using MassTransit;
using MediatR;

namespace Game.Catalogo.Api.Features.ItensBatalha
{
    public static class CriaItemBatalha
    {
        public sealed record Command : IRequest<Guid> 
        {
            public string? Nome { get; set; } = string.Empty;
            public string? Descricao { get; set; } = string.Empty;
            public int Ataque { get; set; } 
            public int Defesa { get; set; }
            public ItemCategoriaEnum ItemCategoria { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Command, Guid>
        {
            private readonly IUnitOfWork _uof;
            private readonly IItemBatalhaRepository _itemBatalhaRepository;
            private readonly IPublishEndpoint _publishEndpoint;

            public Handler(IUnitOfWork ofWork, IItemBatalhaRepository batalhaRepository, IPublishEndpoint publish)
            {
                _uof = ofWork;
                _itemBatalhaRepository = batalhaRepository;
                _publishEndpoint = publish;
            }

            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var itemBa = new ItemBatalha
                    {
                        ItemId = Guid.NewGuid(),
                        Nome = request.Nome,
                        Descricao = request.Descricao,
                        Ataque = request.Ataque,
                        Defesa = request.Defesa,
                        ItemCategoria = request.ItemCategoria
                    };

                    _itemBatalhaRepository.Cria(itemBa);

                    await _uof.Commit(cancellationToken);

                    await _publishEndpoint.Publish(new ItemBatalhaCriadoEvent
                    {
                        ItemId = itemBa.ItemId,
                        Nome = itemBa.Nome,
                        Descricao = itemBa.Descricao,
                        Ataque = itemBa.Ataque,
                        Defesa = itemBa.Defesa,
                        ItemCategoria = itemBa.ItemCategoria

                    }, cancellationToken);

                    return itemBa.ItemId;

                }
                catch (Exception ex)
                {
                    throw new PublishError($"Erro ao publicar a Mensagem: {ex}");
                }
            }
        }
    }

    public class CriaItemBatalhaEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/ItemBatalha", async (CriaItemBatalhaRequest request, ISender sender) =>
            {
                var command = request.Adapt<CriaItemBatalha.Command>();

                var result = await sender.Send(command);

                return Results.Ok(result);
            });
        }
    }
}
