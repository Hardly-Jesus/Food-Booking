

namespace ReservaBook.Core.Aplication.Dtos.User
{
    public class RessetPasswordRequestDto
    {

        public required string Id { get; set; }
        public required string password { get; set; }
        public required string Token { get; set; }


    }
}
