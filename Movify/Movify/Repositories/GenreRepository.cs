using Movify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movify.Repositories
{
    public class GenreRepository: Repository<Genre>, IGenreRepository
    {
        public GenreRepository(MovifyContext context): base(context)
        {
        }

        public void Remove(Genre genre)
        {
            genre.status = false;
        }
    }
}
