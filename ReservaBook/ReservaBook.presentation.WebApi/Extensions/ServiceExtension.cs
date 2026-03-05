using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ReservaBook.presentation.WebApi.Extensions
{
    public static class ServiceExtension
    {

        public static void AddSwaggerExtension(this IServiceCollection services) 
        {


            services.AddSwaggerGen(Opt =>
            {
                List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", searchOption: SearchOption.TopDirectoryOnly).ToList();
                xmlFiles.ForEach(xmlFiles => Opt.IncludeXmlComments(xmlFiles));


                Opt.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1.0",
                    Title = "Reserva food Api",
                    Description = "this api will be resposible for overall data distribution",
                    Contact = new OpenApiContact()
                    {
                        Name = "Kelvin Diaz Ramirez",
                        Email = "Kelvindiazramirez@gmail.com"
                       
                    }

                });


                Opt.SwaggerDoc("v2", new OpenApiInfo()
                {
                    Version = "v2.0",
                    Title = "Reserva food Api",
                    Description = "this api will be resposible for overall data distribution",
                    Contact = new OpenApiContact()
                    {
                        Name = "Kelvin Diaz Ramirez",
                        Email = "KelvindiazNoReply@gmail.com"

                    }

                });


                Opt.DescribeAllParametersInCamelCase();

            });
        
            
        }



        public static void AddVersioningExtensions(this IServiceCollection services)
        {


            services.AddApiVersioning(Opt =>
            {
                Opt.DefaultApiVersion = new ApiVersion(1, 0);
                Opt.AssumeDefaultVersionWhenUnspecified = true;
                Opt.ReportApiVersions = true;
                Opt.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("x-api-version")
                    );

            }).AddApiExplorer(opt => {

                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;



            });



        }







    }
}
