using Movify.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movify.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MovifyContext context;
        public UnitOfWork(MovifyContext context)
        {
            this.context = context;
        }
        public IGenreRepository Genres => new GenreRepository(context);
        public int Complete()
        {
            return context.SaveChanges();
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
