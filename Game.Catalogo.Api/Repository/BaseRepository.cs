using Game.Catalogo.Api.Database;
using Game.Common;
using Game.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Game.Catalogo.Api.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {

        private readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Atualiza(T entity)
        {
            try
            {
                entity.DataAtualizacao = DateTimeOffset.Now;
                _context.Update(entity);
            }
            catch (Exception ex)
            {

                throw new DatabaseException($"Erro ao tentar atualizar a Entidade: {ex}");
            }
            
        }

        public async Task<T> Consulta(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Set<T>().SingleOrDefaultAsync(expression, cancellationToken);
            }
            catch (Exception ex)
            {

                throw new DatabaseException($"Erro na consulta: {ex}");
            }
           
        }

        public async Task<List<T>> ConsultaTodos(int skip, int take, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Set<T>().Skip(skip).Take(take).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {

                throw new DatabaseException($"Erro na Consulta: {ex}");
            }
        }

        public void Cria(T entity)
        {
            try
            {
                entity.DataCriacao = DateTimeOffset.Now;
                _context.Set<T>().Add(entity);
            }
            catch (Exception ex)
            {

                throw new DatabaseException($"Erro ao tentar criar uma entidade no banco: {ex}");
            }
        }

        public void Exclui(T entity)
        {
            try
            {
                entity.DataExclusao = DateTimeOffset.Now;
                _context.Set<T>().Remove(entity);
            }
            catch (Exception ex)
            {

                throw new DatabaseException($"Erro ao tentar excluir uma entidade do banco: {ex}");
            }
        }
    }
}
