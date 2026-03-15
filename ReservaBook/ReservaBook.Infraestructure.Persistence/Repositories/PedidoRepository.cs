using ReservaBook.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;
using ReservaBook.Infraestructure.Persistence.Contexts;



namespace ReservaBook.Infraestructure.Persistence.Repositories
{
    public class PedidoRepository : GenericRepository<Pedido>, IPedidoRepository
    {


        public PedidoRepository(ReservaBookContext appContext) : base(appContext)
        {
        }
    }
}
