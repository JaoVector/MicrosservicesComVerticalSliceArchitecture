using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game.Inventario.Api.Migrations
{
    /// <inheritdoc />
    public partial class QuartaMigracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensInventario_ItensBatalha_ItemBatalhaItemId",
                table: "ItensInventario");

            migrationBuilder.DropIndex(
                name: "IX_ItensInventario_ItemBatalhaItemId",
                table: "ItensInventario");

            migrationBuilder.DropColumn(
                name: "ItemBatalhaItemId",
                table: "ItensInventario");

            migrationBuilder.CreateIndex(
                name: "IX_ItensInventario_ItemId",
                table: "ItensInventario",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensInventario_ItensBatalha_ItemId",
                table: "ItensInventario",
                column: "ItemId",
                principalTable: "ItensBatalha",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensInventario_ItensBatalha_ItemId",
                table: "ItensInventario");

            migrationBuilder.DropIndex(
                name: "IX_ItensInventario_ItemId",
                table: "ItensInventario");

            migrationBuilder.AddColumn<Guid>(
                name: "ItemBatalhaItemId",
                table: "ItensInventario",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ItensInventario_ItemBatalhaItemId",
                table: "ItensInventario",
                column: "ItemBatalhaItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensInventario_ItensBatalha_ItemBatalhaItemId",
                table: "ItensInventario",
                column: "ItemBatalhaItemId",
                principalTable: "ItensBatalha",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
