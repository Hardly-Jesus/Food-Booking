

namespace ReservaBook.Core.Domain.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public required DateOnly Fecha { get; set; }
        public required int  Hora { get; set; }
        public required List<Plato> Platos { get; set; }

         
        //foreign key y navigation property
        public ICollection<Plato> _Platos { get; set; } = new List<Plato>();

        public int IdRestaurante { get; set; } 
        public Restaurante? Restaurante { get; set; }    

        public Pago? Pago { get; set; }

    }
}
