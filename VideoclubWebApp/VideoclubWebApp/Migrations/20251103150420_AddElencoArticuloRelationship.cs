using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoclubWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddElencoArticuloRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ElencoArticulos",
                columns: table => new
                {
                    ArticuloId = table.Column<int>(type: "int", nullable: false),
                    ElencoId = table.Column<int>(type: "int", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElencoArticulos", x => new { x.ArticuloId, x.ElencoId });
                    table.ForeignKey(
                        name: "FK_ElencoArticulos_Articulos_ArticuloId",
                        column: x => x.ArticuloId,
                        principalTable: "Articulos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElencoArticulos_Elencos_ElencoId",
                        column: x => x.ElencoId,
                        principalTable: "Elencos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ElencoArticulos_ElencoId",
                table: "ElencoArticulos",
                column: "ElencoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElencoArticulos");
        }
    }
}
