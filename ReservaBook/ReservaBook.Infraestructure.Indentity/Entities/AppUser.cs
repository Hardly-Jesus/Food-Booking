using Microsoft.AspNetCore.Identity;


namespace ReservaBook.Infraestructure.Indentity.Entities
{
    public class AppUser : IdentityUser
    {

        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string ProfileImage { get; set; }
        public string RNC { get; set; } = string.Empty;


    }
}
