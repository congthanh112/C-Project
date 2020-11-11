using Movify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movify.Repositories
{
    public interface IGenreRepository: IRepository<Genre>
    {
        void Remove(Genre genre);
    }
}
