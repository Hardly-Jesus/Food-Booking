using ReservaBook.Core.Domain.Common.Enums;
using ReservaBook.Core.Domain.Entities;


namespace ReservaBook.Core.Domain.Interfaces
{
    public interface IPedidoRepository : IGenericRepository<Pedido>
    {

        Task<bool> ChangeStatus(int IdPedido, EstadoPedido pedido);
        Task<List<Pedido>> GetPedidosByRestauranteId(int restauranteId);
        Task<List<Pedido>> GetPedidosByUsuarioId(string usuarioId);
    }
}

// Prueba

// Prueba
