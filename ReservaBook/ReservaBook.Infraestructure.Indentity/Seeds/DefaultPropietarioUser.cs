using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReservaBook.Core.Domain.Common.Enums;
using ReservaBook.Infraestructure.Indentity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservaBook.Infraestructure.Indentity.Seeds
{
    public static class DefaultPropietarioUser
    {

        public static async Task SeedAsync(UserManager<AppUser> userManager)
        {

            AppUser user = new()
            {
                Name = "Homer",
                LastName = "Propietario DEV",
                UserName = "UserPropietario",
                EmailConfirmed = true,
                PhoneNumber = "8291210020",
                Email = "PropietarioNoReply@gmail.com",
                PhoneNumberConfirmed = true,
                ProfileImage = "",
            };



            if(await userManager.Users.AllAsync(u => u.Email == user.Email))
            {
            
                var userEntity =  await userManager.FindByEmailAsync(user.Email);  

                if(userEntity != null)
                {
                    await userManager.CreateAsync(user,"123pass@");
                    await userManager.AddToRoleAsync(user,UserRoles.Propietario.ToString());
                }
                
            
            }

        }



    }
}
