

using ReservaBook.Core.Aplication.Dtos.plato;
using ReservaBook.Core.Domain.Entities;

namespace ReservaBook.Core.Aplication.Interfaces
{
    public interface IPlatoService : IGenericService<CreatePlatoRequestDto, CreatePlatoRequestDto,PlatoResponseDto,Plato>
    {

        Task<bool> ChangeStatus(int idPlato, string Status);
        Task<List<PlatoResponseDto>> GetListPlatoByUsuarioId(string UsuarioId);
        Task<List<PlatoResponseDto>> GetListPlatoMenu(string UsuarioId);
        Task<List<PlatoResponseDto>> GetListPlatoNotAddMenu(string UsuarioId);
        Task<Indicadoresdto?> GetIndicadoresDto(string Usuario);
        Task<List<PlatoResponseDto>> GetListPlatoAddMenuNotAddPedidoAsync(string UsuarioId, int idPedido);
        Task<List<PlatoResponseDto>> GetListPlatoByPedidoId(int pedidoId);
    }
}

// Prueba

// Prueba
