using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservaBook.Core.Domain.Entities
{
    public class Plato
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public string? Imagen {  get; set; }
        public required double Precio { get; set; }
        public required string Categoria { get; set; }
        public required string Estado { get; set; } 


        //foreign key y navigation property
        public  int? IdMenu { get; set; }
        public Menu? Menu { get; set; }  

        public int? IdPedido { get; set; }
        public Pedido? Pedido { get; set; }  



    }
}
