

using AutoMapper;
using ReservaBook.Core.Aplication.Dtos.pago;
using ReservaBook.Core.Aplication.Interfaces;
using ReservaBook.Core.Domain.Common.Enums;
using ReservaBook.Core.Domain.Entities;
using ReservaBook.Core.Domain.Interfaces;
using System.Net.Http.Headers;

namespace ReservaBook.Core.Aplication.Services
{
    public class PagoService : GenericService<CreatePagoRequesDto, CreatePagoRequesDto,PagoResponseDto,Pago>, IPagoService
    {

        private readonly IMapper _mapper;
        private readonly IPagoRepository _pago;




        public PagoService(IMapper _mapper, IPagoRepository _pago) : base(_pago,_mapper)
        {
            this._mapper = _mapper;
            this._pago = _pago; 
        }




        public override async Task<PagoResponseDto?> AddAsync(CreatePagoRequesDto? entity)
        {

            if (entity == null)
            {

                return null;

            }

            entity.Id = 0;
            entity.Estado = EstadoPago.pendiente.ToString();
            entity.Fecha = DateTime.Now;        
           

           
             

            return await base.AddAsync(entity);

        }





        public async Task<PagoResponseDto?> GetBypedidoId(int pedidoId)
        {
               var response = new PagoResponseDto() { HasError = false, Errors = []};

            try
            {
                if(pedidoId == 0)
                {
                    return null;   

                }


                var entity = await _pago.GetByIdPedido(pedidoId);


                if(entity == null)
                {
                    response.HasError = true;
                    response.Errors.Add("No se encontro el pago especificado");
                    return response;

                }


                var map = _mapper.Map<PagoResponseDto>(entity);
                return map;

            }
            catch (Exception ex) 
            {
                throw new Exception("Ocurrio un error al obtener el pago por el id del pedido" + ex.Message);
            
            
            }
        }

       
    }
}
