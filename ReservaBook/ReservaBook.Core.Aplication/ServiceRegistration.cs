using Microsoft.Extensions.DependencyInjection;
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

        public static void AddServcesLayerIOC(this IServiceCollection services)
        {

            #region generalConfiguration
            services.AddAutoMapper(opt => { }, Assembly.GetExecutingAssembly());
            #endregion



            #region services Registration IOC
            #endregion


        }



    }
}
