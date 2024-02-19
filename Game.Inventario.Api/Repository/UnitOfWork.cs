using Game.Common;
using Game.Common.Exceptions;
using Game.Inventario.Api.Database;

namespace Game.Inventario.Api.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Commit(CancellationToken cancellation)
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Erro ao tentar efetuar o Commit: {ex}");
            }
        }
    }
}
