using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Serviço.Estoque.Migrations
{
    /// <inheritdoc />
    public partial class FixPrimaryKeyName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UniqueI",
                table: "Produtos",
                newName: "UniqueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UniqueId",
                table: "Produtos",
                newName: "UniqueI");
        }
    }
}
