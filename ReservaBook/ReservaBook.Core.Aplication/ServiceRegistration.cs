using Microsoft.Extensions.DependencyInjection;
using ReservaBook.Core.Aplication.Interfaces;
using ReservaBook.Core.Aplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReservaBook.Core.Aplication
{
    public static class ServiceRegistration
    {

        public static void AddServicesLayerIOC(this IServiceCollection services)
        {

            #region generalConfiguration
            services.AddAutoMapper(opt => { }, Assembly.GetExecutingAssembly());
            #endregion



            #region services Registration IOC
            services.AddScoped(typeof(IGenericService<,,,>),typeof(GenericService<,,,>));
            services.AddScoped<IRestauranteServices, RestauranteService>();
            services.AddScoped<IMesaService, MesaService>();
            services.AddScoped<IPlatoService, PlatoService>();
            services.AddScoped<IMenuService, MenuService>();
            #endregion


        }



    }
}
