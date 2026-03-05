using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReservaBook.Infraestructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservaBook.Infraestructure.Persistence
{
    public static class ServicesRegistration
    {
        public static void AddPersistencesLayerIOC(this IServiceCollection Service, IConfiguration config) 
        {

            #region context configuration
            if (config.GetValue<bool>("useInMemoryDatabase"))
            {

                Service.AddDbContext<ReservaBookContextc>
                    (opt => opt.UseInMemoryDatabase("IdentityAppMemory"));
            }
            else
            {

                var connectionStrings = config.GetConnectionString("IdentityConnection");
                Service.AddDbContext<ReservaBookContextc>(
                   (ServiceProvider, opt) =>
                   {

                       opt.EnableSensitiveDataLogging();
                       opt.UseSqlServer(connectionStrings,
                        m => m.MigrationsAssembly(typeof(ReservaBookContextc)
                      .Assembly.FullName));

                   },
                    contextLifetime: ServiceLifetime.Scoped,
                    optionsLifetime: ServiceLifetime.Scoped


                );

            }

        }

        #endregion


        #region repositories IOC
        #endregion

    }

}

