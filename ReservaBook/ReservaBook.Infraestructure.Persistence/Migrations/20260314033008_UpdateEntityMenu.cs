using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservaBook.Infraestructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntityMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Platos_Menus_MenuId",
                table: "Platos");

            migrationBuilder.DropIndex(
                name: "IX_Platos_MenuId",
                table: "Platos");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Platos");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Menus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Menus");

            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "Platos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Platos_MenuId",
                table: "Platos",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Platos_Menus_MenuId",
                table: "Platos",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id");
        }
    }
}
