using Microsoft.EntityFrameworkCore;
using ReservaBook.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;
using ReservaBook.Infraestructure.Persistence.Contexts;

namespace ReservaBook.Infraestructure.Persistence.Repositories
{
    public class MenuRepository : GenericRepository<Menu>, IMenuRepository
    {

        private readonly ReservaBookContext context;   

        public MenuRepository(ReservaBookContext appContext) : base(appContext)
        {
            context = appContext;   


        }




        public async Task<Menu?> GetMenuByRestauranteId(int RestaurateId) 
        {
            var entity = await context.Set<Menu>().FirstOrDefaultAsync(x => x.IdRestaurante == RestaurateId);

            if (entity == null)
            {

                return null;
            
            }



            return entity;
        
        
        }




    }
}

// Prueba

// Prueba
