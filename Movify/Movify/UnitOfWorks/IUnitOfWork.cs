using Movify.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movify.UnitOfWorks
{
    public interface IUnitOfWork: IDisposable
    {
        IGenreRepository Genres { get; }
        int Complete();
    }
}
