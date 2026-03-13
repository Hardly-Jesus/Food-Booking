

namespace ReservaBook.Core.Domain.Entities
{
    public class Menu
    {

        public  int Id { get; set; }
        public required string Nombre { get; set; }
        public required List<Plato> PlatosList { get; set; } = new List<Plato>();




        //navigation property
        public ICollection<Plato> Platos { get; set; } = new List<Plato>();  
        
       
    }

}
