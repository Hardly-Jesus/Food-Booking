

using AutoMapper;
using ReservaBook.Core.Aplication.Dtos.menu;
using ReservaBook.Core.Aplication.Interfaces;
using ReservaBook.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;

namespace ReservaBook.Core.Aplication.Services
{
    public class MenuService : GenericService<CreateMenuDto, CreateMenuDto, MenuResponseDto, Menu>,IMenuService
    {

        private readonly IMenuRepository menuRepository;
        private readonly IRestauranteRepository repoRestaurante;
        private readonly IMapper _mapper;


        public MenuService(IMenuRepository menuRepository, IMapper _mapper, IRestauranteRepository repoRestaurante) : base(menuRepository, _mapper)
        {
            this.menuRepository = menuRepository;   
            this.repoRestaurante = repoRestaurante;
            this._mapper = _mapper;
        }







        public override async Task<MenuResponseDto?> AddAsync(CreateMenuDto? entity)
        {

            if (entity == null)
            {
                return null;
            }


            var restaurante = await repoRestaurante.GetByUserId(entity.IdUsuario);

            entity.IdRestaurante = restaurante!.Id;
            return await base.AddAsync(entity);

        }



        public async Task<DeleteMenuResponseDto?> DeleteMenuAsync(int id)
        {
            var response = new DeleteMenuResponseDto() { Success = false, IsCreated = true };

            try
            {

                bool result = await base.DeleteAsync(id);

                if (result)
                {
                    response.Success = true;
                    response.IsCreated = true;
                    return response;
                }

                return response;

            }
            catch (Exception ex)
            {


                throw new Exception("Ocurrio un error al intentar eliminar el menu " + ex.Message);


            }
        }



        public async  Task<MenuResponseDto?> GetByPropietario(string usuarioId)
        {
            try
            {     var response = new MenuResponseDto() { HasErrors = false, Errors = [] };

                var restaurante = await repoRestaurante.GetByUserId(usuarioId);

                if(restaurante == null)
                {
                    response.HasErrors = true;
                    response.Errors.Add("ocurrio un error al intentar obtener el menu, favor verificar que tu restaurante esta registrado");
                    return response;

                }

                var menu = await menuRepository.GetMenuByRestauranteId(restaurante.Id);

                if(menu == null)
                {
                    response.HasErrors = true;
                    response.Errors.Add("ocurrio un error al intentar obtener el menu, favor verificar que tu restaurante esta registrado, id no encontrado");
                    return response;

                }

                var map = _mapper.Map<MenuResponseDto>(menu);       
                return map;
            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar obtener el menu" + ex.Message);
            
            }

        }




        public override async Task<MenuResponseDto?> UpdateAsync(int id, CreateMenuDto? entity)
        {

            if (entity == null)
            {
                return null;

            }

            var restaurante = await repoRestaurante.GetByUserId(entity.IdUsuario);
            var response = new MenuResponseDto() { HasErrors = true, Errors = [] };

            var IsExit = await menuRepository.GetByIdAsync(id);


            if (IsExit == null)
            {
                response.HasErrors = true;
                response.Errors.Add("No se encontro una plato con ese id, favor verificar");
                return response;

            }


            entity!.Id = IsExit.Id;
            entity.IdRestaurante = IsExit.IdRestaurante;
            return await base.UpdateAsync(id, entity);

        }




    }
}

// Prueba

// Prueba
