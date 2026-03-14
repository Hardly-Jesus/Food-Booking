using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservaBook.Infraestructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntityPlato_Y_Menu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Platos_Menus_IdMenu",
                table: "Platos");

            migrationBuilder.DropIndex(
                name: "IX_Platos_IdMenu",
                table: "Platos");

            migrationBuilder.DropColumn(
                name: "IdMenu",
                table: "Platos");

            migrationBuilder.CreateTable(
                name: "PlatoMenus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatoId = table.Column<int>(type: "int", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatoMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatoMenus_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatoMenus_Platos_PlatoId",
                        column: x => x.PlatoId,
                        principalTable: "Platos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatoMenus_MenuId",
                table: "PlatoMenus",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_PlatoMenus_PlatoId",
                table: "PlatoMenus",
                column: "PlatoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatoMenus");

            migrationBuilder.AddColumn<int>(
                name: "IdMenu",
                table: "Platos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Platos_IdMenu",
                table: "Platos",
                column: "IdMenu");

            migrationBuilder.AddForeignKey(
                name: "FK_Platos_Menus_IdMenu",
                table: "Platos",
                column: "IdMenu",
                principalTable: "Menus",
                principalColumn: "Id");
        }
    }
}
