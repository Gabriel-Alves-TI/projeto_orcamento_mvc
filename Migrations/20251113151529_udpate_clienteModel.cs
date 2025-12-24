using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projeto_orcamento_mvc.Migrations
{
    /// <inheritdoc />
    public partial class udpate_clienteModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Endereco",
                table: "Clientes",
                newName: "Logradouro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Logradouro",
                table: "Clientes",
                newName: "Endereco");
        }
    }
}
