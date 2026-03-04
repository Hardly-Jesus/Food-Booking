

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReservaBook.Infraestructure.Indentity.Contexts;
using ReservaBook.Infraestructure.Indentity.Entities;
using ReservaBook.Infraestructure.Indentity.Seeds;

namespace ReservaBook.Infraestructure.Indentity
{
    public static class ServicesRegistration
    {

     
        public static void AddIdentityLayerIOCForWebApi(this IServiceCollection service, IConfiguration config)
        {

            #region context configuration
            GenerateConfiguration(service, config);

            #endregion



            #region Idenitity configuration

            //configuraciones generales
            service.Configure<IdentityOptions>(opt =>
            {
                //configuracion de la contrseña
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;



                
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts = 5;



                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
                
            });



            //Configuracion para gestion de usuarios
            service.AddIdentityCore<AppUser>()
                .AddRoles<IdentityRole>()
                .AddSignInManager()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);




            service.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(12); //tiempo de duracion del token
               


            });



            service.AddAuthentication(opt =>
            {

                opt.DefaultScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultSignInScheme = IdentityConstants.ApplicationScheme;  

            }).AddCookie(IdentityConstants.ApplicationScheme,opt =>
            {
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                opt.LoginPath = "/Login";
                opt.AccessDeniedPath = "/Login/AccessDenied";


            });


            #endregion

        }



        #region privated methods
        private static void GenerateConfiguration(IServiceCollection Service, IConfiguration config)
        {

            if (config.GetValue<bool>("useInMemoryDatabase"))
            {
                Service.AddDbContext<IdentityContext>(opt => opt.UseInMemoryDatabase("ReservaMemoryDb"));

            }
            else
            {


                var connectionStrings = config.GetConnectionString("DefaultConnection");
                Service.AddDbContext<IdentityContext>(


                    (ServiceProvider, opt) =>
                    {

                        opt.EnableSensitiveDataLogging();
                        opt.UseSqlServer(connectionStrings,
                         m => m.MigrationsAssembly(typeof(IdentityContext)
                       .Assembly.FullName));

                    },
                    contextLifetime: ServiceLifetime.Scoped,
                    optionsLifetime: ServiceLifetime.Scoped

                );


            }

        }
        #endregion




        #region identity Seed
        public static async Task RunIdentitySeed(this IServiceProvider Service)
        {

            using var scope = Service.CreateScope();
            
            var serviceProvider = scope.ServiceProvider;

            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();


            await DefaultRoles.seedAsync(roleManager);
            await DefaultPropietarioUser.SeedAsync(userManager);
            await DefaultAdminUser.SeedAsync(userManager);
            await DefaultClienteUser.SeedAsync(userManager);

        }
        #endregion



    }
}
