using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetBrima.Migrations
{
    /// <inheritdoc />
    public partial class tablesnotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomEleve",
                table: "Notes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PrenomEleve",
                table: "Notes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomEleve",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "PrenomEleve",
                table: "Notes");
        }
    }
}
