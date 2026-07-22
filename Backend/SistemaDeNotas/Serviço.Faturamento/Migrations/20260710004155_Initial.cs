using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Serviço.Faturamento.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notas_Fiscais",
                columns: table => new
                {
                    UniqueId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notas_Fiscais", x => x.UniqueId);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProdutoId = table.Column<string>(type: "TEXT", nullable: false),
                    Quantidade = table.Column<int>(type: "INTEGER", nullable: false),
                    NotaFiscalId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Notas_Fiscais_NotaFiscalId",
                        column: x => x.NotaFiscalId,
                        principalTable: "Notas_Fiscais",
                        principalColumn: "UniqueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_NotaFiscalId",
                table: "Items",
                column: "NotaFiscalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Notas_Fiscais");
        }
    }
}
