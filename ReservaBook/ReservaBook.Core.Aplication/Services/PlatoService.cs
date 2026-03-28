
using AutoMapper;
using ReservaBook.Core.Aplication.Dtos.plato;
using ReservaBook.Core.Aplication.Interfaces;
using ReservaBook.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;

namespace ReservaBook.Core.Aplication.Services
{
    public class PlatoService : GenericService<CreatePlatoRequestDto, CreatePlatoRequestDto, PlatoResponseDto, Plato>, IPlatoService
    {
        private readonly IPlatoRepository _PlatoRepository;
        private readonly IRestauranteRepository _RestauranteRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IPlatoMenuRepository _PatoMenuRepository;
        private readonly IReservaResporitory reservaResporitory;
        private readonly IPagoRepository _pagoRepository;
        private readonly IPedidoRepository pedidoRepository;
        private readonly IReseñaRepository reseniaRepository;
        private readonly IMapper _Mapper;

        public PlatoService(IPlatoRepository _PlatoRepository,
            IRestauranteRepository _RestauranteRepository, 
            IMenuRepository _menuRepository,
            IPlatoMenuRepository _PatoMenuRepository,
            IMapper _mapper,
            IReservaResporitory reservaResporitory,
            IPagoRepository pagoRepository,
            IPedidoRepository pedidoRepository,
            IReseñaRepository reseniaRepository) : base(_PlatoRepository, _mapper)
        {
            this._PlatoRepository = _PlatoRepository;
            this._Mapper = _mapper;
            this._RestauranteRepository = _RestauranteRepository;
            this._menuRepository = _menuRepository;
            this._PatoMenuRepository = _PatoMenuRepository;
            this.reservaResporitory = reservaResporitory;
            this._pagoRepository = pagoRepository;
            this.reseniaRepository = reseniaRepository;
            this.pedidoRepository = pedidoRepository;

        }



        public override async Task<PlatoResponseDto?> AddAsync(CreatePlatoRequestDto? entity)
        {

            try
            {
                if (entity == null)
                    return null;


                return await base.AddAsync(entity);
            }
            catch (Exception ex)
            {


                throw new Exception(ex.Message);

            }

        }



        public override async Task<PlatoResponseDto?> GetByIdAsync(int id)
        {
            try
            {

                var response = new PlatoResponseDto() { HasError = false, Errors = [] };


                var entity = await base.GetByIdAsync(id);

                if (entity == null)
                {
                    response.HasError = true;
                    response.Errors.Add("Message: No se pudo encontrar un restaurante con ese id");
                    return response;

                }

                return entity;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }
        }




        public override async Task<PlatoResponseDto?> UpdateAsync(int id, CreatePlatoRequestDto? entity)
        {

            if (entity == null)
            {
                return null;

            }

            var response = new PlatoResponseDto() { HasError = true, Errors = [] };

            var IsExit = await _PlatoRepository.GetByIdAsync(id);


            if (IsExit == null)
            {
                response.HasError = true;
                response.Errors.Add("No se encontro una plato con ese id, favor verificar");
                return response;

            }


            if (entity.Imagen == null || string.IsNullOrEmpty(entity.Imagen))
            {
                entity.Imagen = IsExit.Imagen;

            }

            entity!.Id = IsExit.Id;
            entity.Estado = IsExit.Estado;
            entity.UsuarioId = IsExit.UsuarioId;

            return await base.UpdateAsync(id, entity);

        }





        public async Task<bool> ChangeStatus(int idPlato, string Status)
        {
            try
            {
                if (idPlato <= 0)
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(Status))
                {
                    return false;
                }



                await _PlatoRepository.ChangeStatus(idPlato, Status);
                return true;

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error la intentar cambiar el estado del plato " + ex.Message);

            }

        }

        public async Task<List<PlatoResponseDto>> GetListPlatoByUsuarioId(string UsuarioId)
        {

            try
            {
                var entiies = await _PlatoRepository.GetListPlatoByUsuarioId(UsuarioId);

                if (entiies == null || entiies.Count == 0)
                {
                    return [];
                }

                var map = _Mapper.Map<List<PlatoResponseDto>>(entiies);
                return map;


            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al obtener tus platos registrado " + ex.Message);


            }

        }






        public async Task<List<PlatoResponseDto>> GetListPlatoNotAddMenu(string UsuarioId)
        {
            try
            {

                var restaurante = await _RestauranteRepository.GetByUserId(UsuarioId);

                if (restaurante == null)
                {
                    return [];
                }

                var menu = await _menuRepository.GetMenuByRestauranteId(restaurante!.Id);


                if (menu == null)
                {
                    return [];
                }



          
                var PlatoMenus = await _PatoMenuRepository.GetByMenuId(menu.Id);


                if (PlatoMenus == null || PlatoMenus.Count == 0)
                {
                    return [];
                }


                var platosUsuario = await _PlatoRepository.GetListPlatoByUsuarioId(UsuarioId);

                if(platosUsuario == null)
                {
                    return [];
                }


                var platosFiltrados = platosUsuario
                    .Where(p => !PlatoMenus.Any(pm => pm!.PlatoId == p.Id))
                    .ToList();

                var listPlato = _Mapper.Map<List<PlatoResponseDto>>(platosFiltrados);

                if(listPlato == null || listPlato.Count == 0)
                {
                    return [];
                }


                return listPlato;


            }
            catch (Exception ex)
            {


                throw new Exception("Ocurrio un error al intentar ver los platos " + ex.Message);


            }

        }










        public async Task<List<PlatoResponseDto>> GetListPlatoMenu(string UsuarioId)
        {

            try
            {
                var restaurante = await _RestauranteRepository.GetByUserId(UsuarioId);

                if (restaurante == null)
                {
                    return [];
                }

                var menu = await _menuRepository.GetMenuByRestauranteId(restaurante!.Id);


                if (menu == null)
                {
                    return [];
                }


                var PlatoMenus = await _PatoMenuRepository.GetByMenuId(menu.Id);



                if (PlatoMenus == null || PlatoMenus.Count == 0)
                {
                    return [];
                }

                var listPlato = new List<PlatoResponseDto>();
                foreach (var entity in PlatoMenus)
                {

                    var plato = await _PlatoRepository.GetByIdAsync(entity!.PlatoId);
                    var map = _Mapper.Map<PlatoResponseDto>(plato);
                    listPlato.Add(map);

                }


                if (listPlato.Count == 0 || listPlato == null)
                {

                    return [];

                }


                return listPlato;

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al obtener tus platos registrado " + ex.Message);

            }

        }

        public async Task<Indicadoresdto?> GetIndicadoresDto(string Usuario)
        {
            try 
            {
                var response = new Indicadoresdto() { TotalPagoProcesado = 0, TotalPedido =  0, TotalResenia = 0, TotalReserva = 0};

                var restauranteUsuario = await _RestauranteRepository.GetByUserId(Usuario);

               

                var reservas = await reservaResporitory.GetListByRestauranteId(restauranteUsuario!.Id);

                response.TotalReserva = reservas!.Count == 0 ? 0 : reservas.Count;


                var resenias = await reseniaRepository.GetAllReseñaByIdRestaurante(restauranteUsuario.Id);

                response.TotalResenia = resenias.Count == 0 ? 0 : resenias.Count;


                var pedidosRestaurante = await pedidoRepository.GetPedidosByRestauranteId(restauranteUsuario.Id);

                response.TotalPedido = pedidosRestaurante.Count == 0 ? 0 : pedidosRestaurante.Count;


                var pagos = await _pagoRepository.GetlAllAsync();

                var pedidosPagados = pedidosRestaurante.Where(p => pagos.Any(x => x.IdPedido == p.Id)).ToList();

                response.TotalPagoProcesado = pedidosPagados.Count == 0 ? 0 : pedidosPagados.Count;

                return response;

            }catch(Exception ex)
            {

                throw new Exception("Ocurrio un error al obtener los indicadores" + ex.Message);


            }
           
        }
    }
}

// Prueba

// Prueba
