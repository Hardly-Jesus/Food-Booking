


using ReservaBook.Core.Domain.Entities;

namespace ReservaBook.Core.Domain.Interfaces
{
    public interface IReservaResporitory : IGenericRepository<Reserva>
    {

        Task<List<Reserva>?> GetAllReservaByIdUsuario(string IdUsuario);
        Task<List<Reserva>?> GetListByRestauranteId(int restauranteId);

    }
}

// Prueba

// Prueba
