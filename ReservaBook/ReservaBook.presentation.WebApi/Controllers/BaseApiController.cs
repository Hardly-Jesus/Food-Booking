using Microsoft.AspNetCore.Mvc;

namespace ReservaBook.presentation.WebApi.Controllers
{
    [Route("Api/v{version:apiversion}/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
     
        


    }
}



