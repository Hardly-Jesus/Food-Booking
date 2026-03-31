using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ReservaBook.Core.Aplication.Dtos.User;
using ReservaBook.Core.Aplication.Interfaces;
using ReservaBook.presentation.WebApi.Handlers;
using System.Security.Claims;

namespace ReservaBook.presentation.WebApi.Controllers.v1
{

   
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class ManagerAccountController : BaseApiController
    {

        private readonly IAccountServiceForWebApi _accountService;
        private readonly IMapper _mapper;



        public ManagerAccountController(IAccountServiceForWebApi _accountService, IMapper _mapper)
        {

            this._accountService = _accountService;
            this._mapper = _mapper;

        }


        [HttpGet("Get-Users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsers()
       {
            try
            {


                var users = await _accountService.GetAllUser();

                if (users == null) 
                {
                    return Ok(new List<UserDto>());
                }

                return Ok(users);


            }
            catch (Exception ex)
            {
                 
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            
     
            }
        
       
        }





    
        [HttpPut("EditUser/{usuarioId}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EditResponseDto))]
        public async Task<IActionResult> EditUser(string usuarioId,[FromForm] EditUserRequestMinDto dto)
        {

            try
            {
                
               
                var result = await _accountService.EditUser(
                    new SaveUserDto()
                    {   Id = usuarioId,
                        Name = dto.Name,
                        LastName = dto.LastName,
                        Email = dto.Email ?? "",
                        Password = dto.Password ?? "",
                        ProfileImage = FileHandler.Upload(dto.ProfileImage, usuarioId!, null!, true, ""),
                        Phone = dto.Phone,
                        UserName = dto.UserName ?? "",
                        Role = ""
                    });


                if (result == null || result.HasError)
                {
                    return BadRequest(result!.Errors);
                }

              
                return Created("", result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }








        [HttpGet("GetById/{usuarioId}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EditResponseDto))]
        public async Task<IActionResult> GetUserById(string usuarioId)
        {

            try
            {


                var result = await _accountService.GetUserById(usuarioId);


                if (result == null)
                {
                    return NotFound("Usuario no encontrado, favor verificar");
                }


                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }





        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var userDeleted = await _accountService.DeleteAsync(id);

                if (userDeleted.HasError)
                {
                      return NotFound(userDeleted.Errors);  
                }
                
                return NoContent();

            }
            catch (Exception ex)
            {
            

                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message); 
            
            }
       
        }




    }
}

// Prueba

// Prueba
