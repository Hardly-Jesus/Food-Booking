

using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using ReservaBook.Core.Aplication.Dtos.email;
using ReservaBook.Core.Aplication.Dtos.User;
using ReservaBook.Core.Aplication.Interfaces;
using ReservaBook.Infraestructure.Indentity.Entities;
using System.Collections.Immutable;
using System.Text;



namespace ReservaBook.Infraestructure.Indentity.Services
{
    public class AccountServiceForWebApi : Core.Aplication.Interfaces.IAccountServiceForWebApi
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IEmailService emailService;





        public AccountServiceForWebApi(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, IEmailService _emailService)
        {
            emailService = _emailService;
            userManager = _userManager;
            signInManager = _signInManager;

        }



        public async Task<LoginResponseDto> Authenticate(LoginDto dto)
        {

            var responseDto = new LoginResponseDto() { Name = "", Email = "", Id = "", LastName = "", UserName = "", Errors = [] };




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


            var rolesList = await userManager.GetRolesAsync(user);

            responseDto.Id = user.Id;
            responseDto.Name = user.Name;
            responseDto.UserName = user.UserName ?? "";
            responseDto.LastName = user.LastName;
            responseDto.Email = user.Email ?? "";
            responseDto.IsVerified = user.EmailConfirmed;
            responseDto.Roles = rolesList.ToList();



            return responseDto;


        }




        public async Task SignOutAsync()
        {

            await signInManager.SignOutAsync();

        }



        public async Task<RegisterResponseDto> RegisterUser(SaveUserDto saveUser, string origin)
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
                string UrlVerification = await GetVerificationEmailUri(User, origin);
                await emailService.SendAsync(new EmailRequestDto()
                {
                    To = saveUser.Email,
                    Subject = "Confirm registration",
                    HtmlBody = "Please confirm your acount visiting this URL: "

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





        public async Task<EditResponseDto> EditUser(SaveUserDto saveUser, string origin)
        {

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

                if (!user.EmailConfirmed)
                {
                    string UrlVerification = await GetVerificationEmailUri(user, origin);
                    await emailService.SendAsync(new EmailRequestDto()
                    {
                        To = saveUser.Email,
                        Subject = "Confirm registration",
                        HtmlBody = "Please confirm your acount visiting this URL: "


                    });
                }


                if (!string.IsNullOrEmpty(saveUser.Password))
                {

                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    await userManager.ResetPasswordAsync(user, token, saveUser.Password);

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



            var ressetUri = GetRessetPasswordUri(user, request.Origin);
            user.EmailConfirmed = false;


            await userManager.UpdateAsync(user);


            await emailService.SendAsync(new EmailRequestDto()
            {

                To = user.Email ?? "",
                Subject = "RessetPassword",
                HtmlBody = $"please resset your password visiting this URL: {ressetUri}"


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
        public async Task<string> GetVerificationEmailUri(AppUser user, string origin)
        {

            string Token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            Token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(Token));
            var route = "Login/ConfirmEmail";
            var ulrComplete = new Uri(string.Concat(origin, "/", route));
            var verificationUri = QueryHelpers.AddQueryString(ulrComplete.ToString(), "UserId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri.ToString(), "token", Token);

            return verificationUri;

        }




        public async Task<string> GetRessetPasswordUri(AppUser user, string origin)
        {

            string Token = await userManager.GeneratePasswordResetTokenAsync(user);

            Token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(Token));
            var route = "Login/RessetPassword";
            var ulrComplete = new Uri(string.Concat(origin, "/", route));
            var ressetPassword = QueryHelpers.AddQueryString(ulrComplete.ToString(), "UserId", user.Id);
            ressetPassword = QueryHelpers.AddQueryString(ressetPassword.ToString(), "token", Token);

            return ressetPassword;

        }
        #endregion



















    }


}
