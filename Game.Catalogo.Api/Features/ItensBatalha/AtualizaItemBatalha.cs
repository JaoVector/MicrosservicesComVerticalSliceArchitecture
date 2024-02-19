using Carter;
using Game.Catalogo.Api.Contracts.ItemBatalha;
using Game.Catalogo.Api.Entities;
using Game.Catalogo.Api.Features.ItensBatalha.Repository;
using Game.Common;
using Game.Common.Events.ItensBatalha;
using Game.Common.Exceptions;
using Mapster;
using MassTransit;
using MediatR;

namespace Game.Catalogo.Api.Features.ItensBatalha
{
    public static class AtualizaItemBatalha
    {
        public sealed record Command : IRequest<ItemBatalhaResponse> 
        {
            public Guid Id { get; set; }
            public string? Nome { get; set; }
            public string? Descricao { get; set; }
            public int Ataque { get; set; }
            public int Defesa { get; set; }
            
        }

        internal sealed class Handler : IRequestHandler<Command, ItemBatalhaResponse>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IItemBatalhaRepository _itemBatalha;
            private readonly IPublishEndpoint _publishEndpoint;

            public Handler(IUnitOfWork unitOfWork, IItemBatalhaRepository itemBatalha, IPublishEndpoint endpoint)
            {
                _unitOfWork = unitOfWork;
                _itemBatalha = itemBatalha;
                _publishEndpoint = endpoint;
            }

            public async Task<ItemBatalhaResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var item = await _itemBatalha.Consulta(item => item.ItemId == request.Id, cancellationToken);

                    if (item is null) throw new NotFoundException($"Não foi encontrado nenhum Item com ID: {request.Id}");

                    item.AtualizaItemExtension(request);

                    _itemBatalha.Atualiza(item);

                    await _unitOfWork.Commit(cancellationToken);

                    await _publishEndpoint.Publish(new ItemBatalhaAtualizadoEvent
                    {
                        ItemId = item.ItemId,
                        Nome = item.Nome,
                        Descricao = item.Descricao,
                        Ataque = item.Ataque,
                        Defesa = item.Defesa,

                    }, cancellationToken);

                    return item.Adapt<ItemBatalhaResponse>();
                }
                catch (Exception ex)
                {

                    throw new PublishError($"Erro ao Publicar a Mensagem: {ex}");
                }
            } 
        }

        public static void AtualizaItemExtension(this ItemBatalha item, Command request) 
        {
            item.Nome = request.Nome;
            item.Descricao = request.Descricao;
            item.Ataque = request.Ataque;
            item.Defesa = request.Defesa;
        }
    }

    public class AtualizaEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("api/v1/ItemBatalha/{ItemId}", async (AtualizaItemBatalha.Command request, Guid? ItemId, ISender sender) => 
            {
                if(request.Id != ItemId) return Results.BadRequest("Id não confere");

                var result = await sender.Send(request);

                return Results.Ok(result);
            });
        }
    }
}
