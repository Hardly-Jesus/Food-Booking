

namespace ReservaBook.Core.Domain.Entities
{
    public class Menu
    {

        public  int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; } 


        //navigation property
        public ICollection<PlatoMenu> PlatoMenus { get; set; } = new List<PlatoMenu>();  
        
       
    }

}
