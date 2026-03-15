using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservaBook.Infraestructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigrationAndEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CantidadPlatos",
                table: "PlatoPedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioUnitario",
                table: "PlatoPedidos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadPlatos",
                table: "PlatoPedidos");

            migrationBuilder.DropColumn(
                name: "PrecioUnitario",
                table: "PlatoPedidos");
        }
    }
}
