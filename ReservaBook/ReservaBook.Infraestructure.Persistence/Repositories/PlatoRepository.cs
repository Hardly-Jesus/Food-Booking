using ReservaBook.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;
using ReservaBook.Infraestructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservaBook.Infraestructure.Persistence.Repositories
{
    public class PlatoRepository : GenericRepository<Plato>, IPlatoRepository
    {

        public PlatoRepository(ReservaBookContext appContext) : base(appContext)
        {
        }
    }
}
