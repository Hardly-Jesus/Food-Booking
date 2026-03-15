using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservaBook.Infraestructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMigrationAndEntityPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Platos_Pedidos_IdPedido",
                table: "Platos");

            migrationBuilder.DropForeignKey(
                name: "FK_Platos_Pedidos_PedidoId",
                table: "Platos");

            migrationBuilder.DropIndex(
                name: "IX_Platos_IdPedido",
                table: "Platos");

            migrationBuilder.DropIndex(
                name: "IX_Platos_PedidoId",
                table: "Platos");

            migrationBuilder.DropColumn(
                name: "IdPedido",
                table: "Platos");

            migrationBuilder.DropColumn(
                name: "PedidoId",
                table: "Platos");

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PlatoPedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPedido = table.Column<int>(type: "int", nullable: false),
                    IdPlato = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatoPedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatoPedidos_Pedidos_IdPedido",
                        column: x => x.IdPedido,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlatoPedidos_Platos_IdPlato",
                        column: x => x.IdPlato,
                        principalTable: "Platos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatoPedidos_IdPedido",
                table: "PlatoPedidos",
                column: "IdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_PlatoPedidos_IdPlato",
                table: "PlatoPedidos",
                column: "IdPlato");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatoPedidos");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Pedidos");

            migrationBuilder.AddColumn<int>(
                name: "IdPedido",
                table: "Platos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PedidoId",
                table: "Platos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Platos_IdPedido",
                table: "Platos",
                column: "IdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_Platos_PedidoId",
                table: "Platos",
                column: "PedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Platos_Pedidos_IdPedido",
                table: "Platos",
                column: "IdPedido",
                principalTable: "Pedidos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Platos_Pedidos_PedidoId",
                table: "Platos",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id");
        }
    }
}
