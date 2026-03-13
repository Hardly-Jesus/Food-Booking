using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservaBook.Infraestructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedPropertyInEntityPlato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Precio",
                table: "Platos",
                type: "decimal(22,2)",
                precision: 22,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(22)",
                oldPrecision: 22,
                oldScale: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Precio",
                table: "Platos",
                type: "float(22)",
                precision: 22,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(22,2)",
                oldPrecision: 22,
                oldScale: 2);
        }
    }
}
