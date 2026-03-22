using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservaBook.Core.Domain.Entities;


namespace ReservaBook.Infraestructure.Persistence.EntityConfigurations
{
    public class RestauranteEntityConfiguration : IEntityTypeConfiguration<Restaurante>
    {
        public void Configure(EntityTypeBuilder<Restaurante> builder)
        {

            #region basic configuration
            builder.HasKey(e => e.Id);
            builder.ToTable("Restaurantes");
            #endregion



            #region property configuration
            builder.Property(r => r.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(r => r.Telefono).IsRequired().HasMaxLength(50);
            builder.Property(r => r.Direccion).IsRequired().HasMaxLength(150);
            builder.Property(r => r.EspecialidadGastronomica).IsRequired().HasMaxLength(250);
            builder.Property(r => r.UsuarioId).IsRequired().HasMaxLength(1000);
            builder.Property(r => r.Imagen).IsRequired().HasMaxLength(int.MaxValue);
            #endregion



            #region relationship
            builder.HasMany(r => r.Reservas)
                   .WithOne(r => r.Restaurante)
                   .HasForeignKey(r => r.IdRestaurante)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.Mesas)
                   .WithOne(r => r.Restaurante)
                   .HasForeignKey(r => r.IdRestaurante)
                   .OnDelete(DeleteBehavior.NoAction);


            builder.HasMany(r => r.Pedidos)
                   .WithOne(r => r.Restaurante)
                   .HasForeignKey(r => r.IdRestaurante)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.Reseñas)
                   .WithOne(r => r.Restaurante)
                   .HasForeignKey(r => r.IdRestaurante)
                    .OnDelete(DeleteBehavior.Cascade);
            #endregion

        }
    }
}

// Prueba

// Prueba
