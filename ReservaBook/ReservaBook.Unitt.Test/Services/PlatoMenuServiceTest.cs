

using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReservaBook.Core.Aplication.Dtos.menu;
using ReservaBook.Core.Aplication.Dtos.plato;
using ReservaBook.Core.Aplication.Dtos.platoMenu;
using ReservaBook.Core.Aplication.Mappings.EntitiesToDto;
using ReservaBook.Core.Aplication.Services;
using ReservaBook.Core.Domain.Common.Enums;
using ReservaBook.Core.Domain.Entities;
using ReservaBook.Infraestructure.Persistence.Contexts;
using ReservaBook.Infraestructure.Persistence.Repositories;

namespace ReservaBook.Unitt.Test.Services
{
    public class PlatoMenuServiceTest
    {

        private readonly IMapper _mapper;
        private readonly DbContextOptions<ReservaBookContext> dbContextOptions;



        public PlatoMenuServiceTest()
        {
            dbContextOptions = new DbContextOptionsBuilder<ReservaBookContext>()
                                     .UseInMemoryDatabase(databaseName: $"InMemoryDbPlatoMenu_{Guid.NewGuid}")
                                      .Options;



            var loggerFactory = LoggerFactory.Create(cf =>
            {

                cf.AddConsole();
            });



            var config = new MapperConfiguration(cfg =>
            {
                //Add your automapper profile here
                cfg.AddProfile<PlatoMenuEntityToDtosMappingProfile>();


            }, loggerFactory);



            _mapper = config.CreateMapper();


        }



        #region private method
        public PlatoMenuService CreateService()
        {
            var context = new ReservaBookContext(dbContextOptions);
            var repo = new PlatoMenuRepository(context);
            var menuRepo = new MenuRepository(context);
            var PlatoRepo = new PlatoRepository(context);

            var service = new PlatoMenuService(repo,menuRepo,PlatoRepo,_mapper);
            return service;
        }
        #endregion



        #region test
        [Fact]
        public async Task AddPlatoAlMenu_should_return_ListResponseDto_when_Added()
        {

            //arrange
            var service =  CreateService();

            var context = new ReservaBookContext(dbContextOptions);
            var menuRepo = new MenuRepository(context);
            var PlatoRepo = new PlatoRepository(context);
         
            var plato = new Plato()
            {

                Id  = 0,
                Nombre = "Mangu con salami",
                Descripcion = "Es un plato de entrada muy ...",
                Categoria = PlatoCategoria.Entradas.ToString(),
                Estado = Estado.Disponible.ToString(),
                Precio = 120.50m,
                Imagen = "https://i.pinimg.com/originals/ae/63/38/ae633823e1c5d0c395263bc05ec4da36.jpg"
            };



            var menu = new Menu()
            {
                Id = 0,
                Nombre = "Comida china",
                Descripcion = "Es un menu orientada a comida china ......"


            };

            var PlatoAdded = PlatoRepo.AddAsync(plato);
            var menuAdded = menuRepo.AddAsync(menu);

            List<int> PlatosIdList = new List<int>();
            PlatosIdList.Add(PlatoAdded.Id);

            //act
            var result = await service.AddPlatoAlMenu(menuAdded.Id,PlatosIdList);





            //assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(1);
            result.Should().HaveCountGreaterThan(0);

        }




        [Fact]
        public async Task DeletePlatoDelMenu_should_return_ResponseDto_when_Deleted()
        {

            //arrange
            var service = CreateService();

            var context = new ReservaBookContext(dbContextOptions);
            var menuRepo = new MenuRepository(context);
            var PlatoRepo = new PlatoRepository(context);

            var plato = new Plato()
            {

                Id = 0,
                Nombre = "Mangu con salami",
                Descripcion = "Es un plato de entrada muy ...",
                Categoria = PlatoCategoria.Entradas.ToString(),
                Estado = Estado.Disponible.ToString(),
                Precio = 120.50m,
                Imagen = "https://i.pinimg.com/originals/ae/63/38/ae633823e1c5d0c395263bc05ec4da36.jpg"
            };



            var menu = new Menu()
            {
                Id = 0,
                Nombre = "Comida china",
                Descripcion = "Es un menu orientada a comida china ......"


            };

            var PlatoAdded = PlatoRepo.AddAsync(plato);
            var menuAdded = menuRepo.AddAsync(menu);

            List<int> PlatosIdList = new List<int>();
            PlatosIdList.Add(PlatoAdded.Id);

            //act
            var PlatoMenuAdded = await service.AddPlatoAlMenu(menuAdded.Id, PlatosIdList);
            var result = await service.DeletePlatoDelMenu(menuAdded.Id,PlatoAdded.Id);



            //assert
            result.Should().NotBeNull();
       
        }
        #endregion


    }
}
