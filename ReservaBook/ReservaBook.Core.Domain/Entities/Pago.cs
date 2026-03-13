using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservaBook.Core.Domain.Entities
{
    public class Pago
    {

        public int Id { get; set; }
        public required DateTime Fecha { get; set; }
        public required decimal Monto { get; set; }
        public required string Estado { get; set; }


        //foreing key y navigation property
   
        public int IdPedido { get; set; }
        public Pedido? Pedido { get; set; }
    }
}
