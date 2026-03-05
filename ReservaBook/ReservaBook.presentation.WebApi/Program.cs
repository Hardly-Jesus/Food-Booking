using ReservaBook.Infraestructure.Shared;
using ReservaBook.Infraestructure.Indentity;
using ReservaBook.presentation.WebApi.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container...



// Add services to the container.



builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEmailServicesIOC(builder.Configuration);
builder.Services.AddIdentityLayerIOCForWebApi(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning();
builder.Services.AddSwaggerExtension();
builder.Services.AddVersioningExtensions();




var app = builder.Build();
await app.Services.RunIdentitySeed();

// Configure the HTTP request pipeline.
// Hola prueba de comit--Andris
if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerExtension(app);
        app.MapOpenApi();
    }

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecks("/health");
app.MapControllers();

app.Run();
