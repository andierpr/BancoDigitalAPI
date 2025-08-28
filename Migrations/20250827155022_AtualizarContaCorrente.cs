using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancoDigitalAPI.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarContaCorrente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumeroConta",
                table: "Contas",
                newName: "Conta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Conta",
                table: "Contas",
                newName: "NumeroConta");
        }
    }
}
