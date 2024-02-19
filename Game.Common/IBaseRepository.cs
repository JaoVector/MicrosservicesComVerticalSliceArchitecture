using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Game.Common
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        void Cria(T entity);
        void Atualiza(T entity);
        void Exclui(T entity);
        Task<T> Consulta(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);
        Task<List<T>> ConsultaTodos(int skip, int take, CancellationToken cancellationToken);
    }
}
