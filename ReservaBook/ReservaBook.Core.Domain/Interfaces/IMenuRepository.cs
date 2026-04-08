

using ReservaBook.Core.Domain.Entities;
using System.ComponentModel.Design;

namespace ReservaBook.Core.Domain.Interfaces
{
    public interface IMenuRepository : IGenericRepository<Menu>
    {

        Task<Menu?> GetMenuByRestauranteId(int RestaurateId);

    }
}

// Prueba

// Prueba
