using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game.Inventario.Api.Migrations
{
    /// <inheritdoc />
    public partial class TerceiraMigracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensInventario_ItensBatalha_ItemBatalhaItemId",
                table: "ItensInventario");

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemBatalhaItemId",
                table: "ItensInventario",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensInventario_ItensBatalha_ItemBatalhaItemId",
                table: "ItensInventario",
                column: "ItemBatalhaItemId",
                principalTable: "ItensBatalha",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensInventario_ItensBatalha_ItemBatalhaItemId",
                table: "ItensInventario");

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemBatalhaItemId",
                table: "ItensInventario",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensInventario_ItensBatalha_ItemBatalhaItemId",
                table: "ItensInventario",
                column: "ItemBatalhaItemId",
                principalTable: "ItensBatalha",
                principalColumn: "ItemId");
        }
    }
}
