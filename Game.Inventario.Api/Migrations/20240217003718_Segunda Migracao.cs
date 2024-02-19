using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game.Inventario.Api.Migrations
{
    /// <inheritdoc />
    public partial class SegundaMigracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensIventario_ItensBatalha_ItemBatalhaItemId",
                table: "ItensIventario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensIventario",
                table: "ItensIventario");

            migrationBuilder.RenameTable(
                name: "ItensIventario",
                newName: "ItensInventario");

            migrationBuilder.RenameIndex(
                name: "IX_ItensIventario_ItemBatalhaItemId",
                table: "ItensInventario",
                newName: "IX_ItensInventario_ItemBatalhaItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensInventario",
                table: "ItensInventario",
                column: "ItemIventId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensInventario_ItensBatalha_ItemBatalhaItemId",
                table: "ItensInventario",
                column: "ItemBatalhaItemId",
                principalTable: "ItensBatalha",
                principalColumn: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensInventario_ItensBatalha_ItemBatalhaItemId",
                table: "ItensInventario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensInventario",
                table: "ItensInventario");

            migrationBuilder.RenameTable(
                name: "ItensInventario",
                newName: "ItensIventario");

            migrationBuilder.RenameIndex(
                name: "IX_ItensInventario_ItemBatalhaItemId",
                table: "ItensIventario",
                newName: "IX_ItensIventario_ItemBatalhaItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensIventario",
                table: "ItensIventario",
                column: "ItemIventId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensIventario_ItensBatalha_ItemBatalhaItemId",
                table: "ItensIventario",
                column: "ItemBatalhaItemId",
                principalTable: "ItensBatalha",
                principalColumn: "ItemId");
        }
    }
}
