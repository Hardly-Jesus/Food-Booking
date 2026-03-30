using Microsoft.EntityFrameworkCore;
using ReservaBook.Core.Domain.Common.Enums;
using ReservaBook.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;
using ReservaBook.Infraestructure.Persistence.Contexts;

namespace ReservaBook.Infraestructure.Persistence.Repositories
{
    public class PedidoRepository : GenericRepository<Pedido>, IPedidoRepository
    {

        private readonly ReservaBookContext _context;

        public PedidoRepository(ReservaBookContext appContext) : base(appContext)
        {
            this._context = appContext;
        }


        public async Task<bool> ChangeStatus(int idMesa, EstadoPedido estado)
        {
            var entity = await _context.Set<Pedido>().FindAsync(idMesa);

            if (entity == null)
            {
                return false;
            }

            entity.Estado = estado;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Pedido>> GetPedidosByRestauranteId(int restauranteId)
        {

            var entities = await _context.Set<Pedido>().Where(p => p.IdRestaurante == restauranteId).ToListAsync();

            if (entities == null) return [];


            return entities;

        }




        public async Task<List<Pedido>> GetPedidosByUsuarioId(string usuarioId)
        {

            var entities = await _context.Set<Pedido>().Where(p => p.UsuarioId == usuarioId).ToListAsync();

            if (entities == null) return [];


            return entities;

        }








    }
}

// Prueba

// Prueba
