

using AutoMapper;
using ReservaBook.Core.Aplication.Dtos.Reseña;
using ReservaBook.Core.Aplication.Interfaces;
using ReservaBook.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;

namespace ReservaBook.Core.Aplication.Services
{
    public class ReseñaService : GenericService<CreateReseñaDto, CreateReseñaDto, ReseñaResponseDto, Reseña>, IReseñaService
    {
        private readonly IMapper _mapper;
        private readonly IReseñaRepository _reseñaRepository;


        public ReseñaService(IReseñaRepository _reseñaRepository, IMapper _mapper) : base(_reseñaRepository, _mapper)
        {
            this._mapper = _mapper;
            this._reseñaRepository = _reseñaRepository;

        }


        public override async Task<ReseñaResponseDto?> AddAsync(CreateReseñaDto? entity)
        {
            var response = new ReseñaResponseDto() { HasErrors = false, Errors = [] };


            try
            {

                if (entity == null)
                {
                    return null;
                }

                if (string.IsNullOrWhiteSpace(entity.Descripcion))
                {
                    response.HasErrors = true;
                    response.Errors.Add("Debes agregarle una descripcion para realizar la reseña");
                    return response;

                }

                if (entity.CantidadEstrella <= 0)
                {
                    response.HasErrors = true;
                    response.Errors.Add("Debes agregarle una descripcion para realizar la reseña");
                    return response;
                }

                return await base.AddAsync(entity);

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar " + ex.Message);

            }

        }

        public async Task<List<ReseñaResponseDto>> GetAllByIdRestaurnteAsync(int idRestaurante)
        {
            try
            {
                var response = new ReseñaResponseDto() { HasErrors = false, Errors = [] };
                var listResponse = new List<ReseñaResponseDto>();

                if (idRestaurante <= 0)
                {
                    return [];
                }


                var entities = await _reseñaRepository.GetAllReseñaByIdRestaurante(idRestaurante);

                if(entities == null || entities.Count <= 0)
                {
                    response.HasErrors = true;
                    response.Errors.Add("No se encontraron las reseña del restaurante indicado");
                    listResponse.Add(response);
                    return listResponse;

                }


                var map = _mapper.Map<List<ReseñaResponseDto>>(entities);
                return map;

            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar tener las reseña " + ex.Message); 

            }
        }



        public override async Task<ReseñaResponseDto?> UpdateAsync(int id, CreateReseñaDto? entity)
        {
            var response = new ReseñaResponseDto() { HasErrors = false, Errors = [] };


            try
            {

                if (entity == null)
                {
                    return null;
                }


                if (id <= 0)
                {
                    response.HasErrors = true;
                    response.Errors.Add("Debes una reseña valida para actualizar");
                    return response;
                }


                var IsExit = await _reseñaRepository.GetByIdAsync(id);

                if (IsExit == null)
                {

                    response.HasErrors = true;
                    response.Errors.Add("No se encontro un reseña con ese id, favor verificar");
                    return response;

                }

                entity.Id = IsExit.Id;
                entity.IdRestaurante = IsExit.IdRestaurante;


                return await base.UpdateAsync(entity.Id, entity);
            }
            catch (Exception ex)
            {

                throw new Exception("Ocurrio un error al intentar actualizar la reseña" + ex.Message);

            }

        }

    }
}
