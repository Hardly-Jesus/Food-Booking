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
    public class MesaRepository : GenericRepository<Mesa>, IMesaRepository
    {
        public MesaRepository(ReservaBookContext appContext) : base(appContext)
        {
        }
    }
}
