using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorDespacho.Migrations
{
    /// <inheritdoc />
    public partial class CambioCalleNumeroDireccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Calle",
                table: "Direcciones",
                newName: "CalleNumero");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CalleNumero",
                table: "Direcciones",
                newName: "Calle");
        }
    }
}
