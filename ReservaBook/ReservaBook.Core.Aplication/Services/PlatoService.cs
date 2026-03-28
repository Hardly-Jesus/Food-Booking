
using AutoMapper;
using ReservaBook.Core.Aplication.Dtos.plato;
using ReservaBook.Core.Aplication.Interfaces;
using ReservaBook.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;

namespace ReservaBook.Core.Aplication.Services
{
    public class PlatoService : GenericService<CreatePlatoRequestDto, CreatePlatoRequestDto, PlatoResponseDto, Plato>,IPlatoService
    {
        private readonly IPlatoRepository _PlatoRepository;
        private readonly IMapper _Mapper;

        public PlatoService(IPlatoRepository _PlatoRepository, IMapper _mapper) : base(_PlatoRepository, _mapper)
        {
            this._PlatoRepository = _PlatoRepository;
            this._Mapper = _mapper;

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
    }
}

// Prueba

// Prueba
