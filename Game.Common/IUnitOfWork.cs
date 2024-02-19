using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Common
{
    public interface IUnitOfWork
    {
        Task Commit(CancellationToken cancellation);
    }
}
