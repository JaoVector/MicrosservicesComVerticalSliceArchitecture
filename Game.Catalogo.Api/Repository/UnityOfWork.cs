using Game.Catalogo.Api.Database;
using Game.Common;
using Game.Common.Exceptions;

namespace Game.Catalogo.Api.Repository
{
    public class UnityOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _context;

        public UnityOfWork(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task Commit(CancellationToken cancellationToken)
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
