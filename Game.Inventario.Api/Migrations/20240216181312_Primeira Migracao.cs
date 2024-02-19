using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Game.Inventario.Api.Migrations
{
    /// <inheritdoc />
    public partial class PrimeiraMigracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItensBatalha",
                columns: table => new
                {
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ataque = table.Column<int>(type: "int", nullable: false),
                    Defesa = table.Column<int>(type: "int", nullable: false),
                    ItemCategoria = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DataAtualizacao = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DataExclusao = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensBatalha", x => x.ItemId);
                });

            migrationBuilder.CreateTable(
                name: "ItensIventario",
                columns: table => new
                {
                    ItemIventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonagemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemBatalhaItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataCriacao = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DataAtualizacao = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DataExclusao = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensIventario", x => x.ItemIventId);
                    table.ForeignKey(
                        name: "FK_ItensIventario_ItensBatalha_ItemBatalhaItemId",
                        column: x => x.ItemBatalhaItemId,
                        principalTable: "ItensBatalha",
                        principalColumn: "ItemId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItensIventario_ItemBatalhaItemId",
                table: "ItensIventario",
                column: "ItemBatalhaItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItensIventario");

            migrationBuilder.DropTable(
                name: "ItensBatalha");
        }
    }
}
