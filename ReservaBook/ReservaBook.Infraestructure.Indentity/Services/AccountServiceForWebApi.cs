

using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ReservaBook.Core.Aplication.Dtos.email;
using ReservaBook.Core.Aplication.Dtos.User;
using ReservaBook.Core.Aplication.Interfaces;
using ReservaBook.Core.Domain.Settings;
using ReservaBook.Infraestructure.Indentity.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace ReservaBook.Infraestructure.Indentity.Services
{
    public class AccountServiceForWebApi : IAccountServiceForWebApi
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IEmailService emailService;
        private readonly JwtSettings _jwtSettings;




        public AccountServiceForWebApi(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, IEmailService _emailService, IOptions<JwtSettings> JwtSettings)
        {
            emailService = _emailService;
            userManager = _userManager;
            signInManager = _signInManager;
            _jwtSettings = JwtSettings.Value;

        }



        public async Task<LoginResponseDto> Authenticate(LoginDto dto)
        {

            var responseDto = new LoginResponseDto() { Name = "", LastName = "", Errors = [], AccessToken = "" };




            var user = await userManager.FindByNameAsync(dto.UserName);

            if (user != null)
            {
                responseDto.HasError = true;
                responseDto.Errors!.Add($"There is not account registered with this userName: {dto.UserName}");
                return responseDto;

            }



            if (!user!.EmailConfirmed)
            {
                responseDto.HasError = true;
                responseDto.Errors!.Add($"This account {dto.UserName} is not active, you shoul check your email");
                return responseDto;

            }



            var result = await signInManager.PasswordSignInAsync(user.UserName ?? "", dto.Password, false, true);


            if (!result.Succeeded)
            {

                responseDto.HasError = true;
                responseDto.Errors!.Add($"These credentials are invalid for this user: {user.UserName}");
                return responseDto;

            }


            JwtSecurityToken jwtSecurityToken = await GenerateJwtToken(user);



            var rolesList = await userManager.GetRolesAsync(user);

   
            responseDto.Name = user.Name;
            responseDto.LastName = user.LastName;
            responseDto.AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);


            return responseDto;


        }




        public async Task<RegisterResponseDto> RegisterUser(SaveUserDto saveUser)
        {

            var response = new RegisterResponseDto() { Name = "", Email = "", Id = "", LastName = "", UserName = "", Errors = [] };


            var userWithSomeUserName = await userManager.FindByNameAsync(saveUser.UserName);
            if (userWithSomeUserName != null)
            {
                response.HasError = true;
                response.Errors!.Add($"this userName: {saveUser.UserName} is already taken.");
                return response;
            }

            var userWithSomeEmail = await userManager.FindByEmailAsync(saveUser.Email);
            if (userWithSomeEmail != null)
            {
                response.HasError = true;
                response.Errors!.Add($"This email: {saveUser.Email} is already taken.");
                return response;

            }


            AppUser User = new AppUser()
            {

                Name = saveUser.LastName,
                LastName = saveUser.LastName,
                Email = saveUser.Email,
                PhoneNumber = saveUser.Phone,
                UserName = saveUser.UserName,
                EmailConfirmed = false,
                ProfileImage = saveUser.ProfileImage ?? "",
                RNC = saveUser.RNC ?? ""


            };


            var result = await userManager.CreateAsync(User, saveUser.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(User, saveUser.Role);
                string token = await GetVerificationEmailToken(User);
                await emailService.SendAsync(new EmailRequestDto()
                {
                    To = saveUser.Email,
                    Subject = "Confirm registration",
                    HtmlBody = $"Please confirm your acount Use this Token:{token} "

                });


                var CurrentrolesList = await userManager.GetRolesAsync(User);

                response.Id = User.Id;
                response.Name = User.Name;
                response.UserName = User.UserName ?? "";
                response.LastName = User.LastName;
                response.Email = User.Email ?? "";
                response.IsVerified = User.EmailConfirmed;
                response.Roles = CurrentrolesList.ToList();

            }
            else
            {

                response.HasError = true;
                response.Errors!.AddRange(result.Errors.Select(s => s.Description).ToList());
                return response;

            }


            return response;
        }





        public async Task<EditResponseDto> EditUser(SaveUserDto saveUser,bool? IsCreated = false)
        {


            bool IsNotCreated = IsCreated ?? false;
            var response = new EditResponseDto() { Name = "", Email = "", Id = "", LastName = "", UserName = "", Errors = []};


            var userWithSomeUserName = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == saveUser.UserName && u.Id != saveUser.Id);
            if (userWithSomeUserName != null)
            {
                response.HasError = true;
                response.Errors!.Add($"this userName: {saveUser.UserName} is already taken.");
                return response;
            }

            var userWithSomeEmail = await userManager.Users.FirstOrDefaultAsync(u => u.Email == saveUser.Email && u.Id != saveUser.Id);
            if (userWithSomeEmail != null)
            {
                response.HasError = true;
                response.Errors!.Add($"This email: {saveUser.Email} is already taken.");
                return response;

            }


            var user = await userManager.FindByIdAsync(saveUser.Id);
            if (user != null)
            {
                response.HasError = true;
                response.Errors!.Add("There is not account registered with this user");
                return response;
            }



            user!.Name = saveUser.LastName;
            user.LastName = saveUser.LastName;
            user.PhoneNumber = saveUser.Phone;
            user.UserName = saveUser.UserName;
            user.EmailConfirmed = false;
            user.ProfileImage = string.IsNullOrWhiteSpace(saveUser.ProfileImage) ? user.ProfileImage : saveUser.ProfileImage;
            user.RNC = saveUser.RNC ?? "";
            user.EmailConfirmed = user.Email == saveUser.Email;
            user.Email = saveUser.Email;





            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                var rolesList = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, rolesList);
                await userManager.AddToRoleAsync(user, saveUser.Role);

                if (!user.EmailConfirmed && IsNotCreated)
                {
                    string token = await GetVerificationEmailToken(user);
                    await emailService.SendAsync(new EmailRequestDto()
                    {
                        To = saveUser.Email,
                        Subject = "Confirm registration",
                        HtmlBody = $"Please confirm your acount use this URL: {token}"


                    });
                }



                if (!string.IsNullOrWhiteSpace(saveUser.Password) && IsNotCreated)
                {

                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var resultChange = await userManager.ResetPasswordAsync(user, token, saveUser.Password);

                    if (resultChange != null && !resultChange.Succeeded)
                    {

                        response.HasError = true;
                        response.Errors.AddRange(resultChange.Errors.Select(s => s.Description).ToList());
                        return response;

                    }

                }


                var CurrentrolesList = await userManager.GetRolesAsync(user);

                response.Id = user.Id;
                response.Name = user.Name;
                response.UserName = user.UserName ?? "";
                response.LastName = user.LastName;
                response.Email = user.Email ?? "";
                response.IsVerified = user.EmailConfirmed;
                response.Roles = CurrentrolesList.ToList();

                return response;
            }
            else
            {

                response.HasError = true;
                response.Errors!.AddRange(result.Errors.Select(s => s.Description).ToList());
                return response;

            }


        }




        public async Task<UserResponseDto> DeleteAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            var response = new UserResponseDto() { Errors = [], HasError = false };

            if (user == null)
            {

                response.HasError = true;
                response.Errors!.Add($"There is not Account registered with this user");

            }


            await userManager.DeleteAsync(user);


            return response;
        }


        public async Task<List<UserDto>> GetAllUser(bool? IsActive = true)
        {

            var users = userManager.Users;


            List<UserDto> ListUserDto = [];
            if (IsActive!.Value && IsActive != null)
            {
                users = users.Where(u => u.EmailConfirmed);

            }
            else
            {
                users = users.Where(u => !u.EmailConfirmed);
            }


            var ListUser = await users.ToListAsync();

            foreach (var item in ListUser)
            {

                var rolesList = await userManager.GetRolesAsync(item);


                ListUserDto.Add(new UserDto()
                {

                    Id = item.Id,
                    Name = item.Name,
                    UserName = item.UserName ?? "",
                    LastName = item.LastName,
                    Email = item.Email ?? "",
                    IsVerified = item.EmailConfirmed,
                    Phone = item.PhoneNumber ?? "",
                    ProfileImage = item.ProfileImage,
                    Role = rolesList.FirstOrDefault() ?? ""

                });



            }

            return ListUserDto;


        }



        public async Task<UserDto?> GetUserByEmail(string gmail)
        {

            var user = await userManager.FindByEmailAsync(gmail);


            if (user == null)
            {
                return null;

            }


            var rolesList = await userManager.GetRolesAsync(user);

            var userDto = new UserDto()
            {

                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email ?? "",
                UserName = user.UserName ?? "",
                Phone = user.PhoneNumber ?? "",
                ProfileImage = user.ProfileImage,
                IsVerified = user.EmailConfirmed,
                Role = rolesList.FirstOrDefault() ?? ""


            };


            return userDto;
        }



        public async Task<UserDto?> GetUserById(string id)
        {

            var user = await userManager.FindByIdAsync(id);


            if (user == null)
            {
                return null;

            }


            var rolesList = await userManager.GetRolesAsync(user);

            var userDto = new UserDto()
            {

                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email ?? "",
                UserName = user.UserName ?? "",
                Phone = user.PhoneNumber ?? "",
                ProfileImage = user.ProfileImage,
                IsVerified = user.EmailConfirmed,
                Role = rolesList.FirstOrDefault() ?? ""


            };


            return userDto;
        }




        public async Task<UserDto?> GetUserByUserName(string userName)
        {

            var user = await userManager.FindByNameAsync(userName);


            if (user == null)
            {
                return null;

            }

            var rolesList = await userManager.GetRolesAsync(user);

            var userDto = new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email ?? "",
                UserName = user.UserName ?? "",
                Phone = user.PhoneNumber ?? "",
                ProfileImage = user.ProfileImage,
                IsVerified = user.EmailConfirmed,
                Role = rolesList.FirstOrDefault() ?? ""

            };


            return userDto;
        }



        public async Task<UserResponseDto> RessetPassowrd(RessetPasswordRequestDto request)
        {

            var response = new UserResponseDto() { Errors = [], HasError = false };


            var user = await userManager.FindByIdAsync(request.Id);

            if (user == null)
            {
                response.HasError = true;
                response.Errors!.Add($"there is not account registered with this user");
                return response;

            }


            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await userManager.ResetPasswordAsync(user, token, request.password);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Errors!.Add($"An error ocurred while resset password");
                return response;

            }


            user.EmailConfirmed = true;
            await userManager.UpdateAsync(user);

            return response;
        }




        public async Task<UserResponseDto> ForgotPasswordAsync(ForgotPasswordRequest request)
        {

            var response = new UserResponseDto() { Errors = [], HasError = false };


            var user = await userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                response.HasError = true;
                response.Errors!.Add($"there is not account registered with this userName {request.UserName}");
                return response;

            }



            var ressetToken = GetRessetPassworToken(user);
            user.EmailConfirmed = false;


            await userManager.UpdateAsync(user);


            await emailService.SendAsync(new EmailRequestDto()
            {

                To = user.Email ?? "",
                Subject = "RessetPassword",
                HtmlBody = $"please resset your password visiting this URL: {ressetToken}"


            });

            return response;
        }







        public async Task<string> confirmAccounAsync(string UserId, string Token)
        {

            var user = await userManager.FindByIdAsync(UserId);

            if (user == null)
            {

                return "There is not account registered with this user";

            }


            Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Token));
            var result = await userManager.ConfirmEmailAsync(user, Token);
            if (result.Succeeded)
            {
                return $"Account confirmed for {user.Email}. you can now use the app";

            }
            else
            {

                return $"An error ocurred when while confirming this email {user.Email}";

            }


        }





        #region private Method
        private async Task<string> GetVerificationEmailToken(AppUser user)
        {

            string Token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            Token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(Token));
            
            return Token;

        }




        private async Task<string> GetRessetPassworToken(AppUser user)
        {

            string Token = await userManager.GeneratePasswordResetTokenAsync(user);
            Token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(Token));   
            return Token;

        }





        public async Task<JwtSecurityToken> GenerateJwtToken(AppUser user)
        {
        
        
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);



            var rolesClaims = new List<Claim>();

            foreach(var role in roles)
            {
                rolesClaims.Add(new Claim("role",role));
            }


            var claims = new[]
            {

                new Claim(JwtRegisteredClaimNames.Sub,user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email ?? ""),
                new Claim("UId",user.Id),

            }.Union(userClaims).Union(rolesClaims);


            var symmectriSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecreKey));
            var signinCredentials = new SigningCredentials(symmectriSecurityKey,SecurityAlgorithms.Aes128CbcHmacSha256);


            var JwtSecuritytoken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signinCredentials);


            return JwtSecuritytoken;

        }
        #endregion





    }


}
