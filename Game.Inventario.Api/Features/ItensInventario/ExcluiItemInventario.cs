using Amazon.Runtime.Internal;
using Carter;
using Game.Common;
using Game.Common.Exceptions;
using Game.Inventario.Api.Features.ItensInventario.Repository;
using MediatR;

namespace Game.Inventario.Api.Features.ItensInventario
{
    public static class ExcluiItemInventario
    {
        public sealed record Query(Guid IdItemIvent) : IRequest<Guid>;

        internal sealed class Handler : IRequestHandler<Query, Guid>
        {
            private readonly IItemInventarioRepository _repository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IItemInventarioRepository repository, IUnitOfWork unitOfWork)
            {
                _repository = repository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Guid> Handle(Query request, CancellationToken cancellationToken)
            {
                var item = await _repository.Consulta(item => item.ItemIventId == request.IdItemIvent, cancellationToken);

                if (item is null) throw new NotFoundException($"Não foi encontrado nenhum item com ID: {request.IdItemIvent}");

                _repository.Exclui(item);

                await _unitOfWork.Commit(cancellationToken);

                return item.ItemIventId;
            }
        }
    }

    public class ExluiItemInventarioEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/v1/ItemInventario/{ItemIventId}", async (Guid? itemIventId, ISender sender) =>
            {
                if (itemIventId is null) return Results.BadRequest("Campo Id precisa ser preenchido");

                var request = new ExcluiItemInventario.Query(itemIventId.Value);

                var result = await sender.Send(request);

                return Results.Ok(result);
            });
        }
    }
}
