

using ReservaBook.Core.Domain.Common.Enums;

namespace ReservaBook.Core.Domain.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public required DateOnly Fecha { get; set; }
        public required int  Hora { get; set; }
        public required  EstadoPedido Estado { get; set; }

         
        //foreign key y navigation property
        public ICollection<PedidoPlato> PedidoPlatos { get; set; } = new List<PedidoPlato>();

        public int IdRestaurante { get; set; } 
        public Restaurante? Restaurante { get; set; }    


        public Pago? Pago { get; set; }

    }
}
