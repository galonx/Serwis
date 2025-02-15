using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Serwis.Data.Migrations
{
    /// <inheritdoc />
    public partial class pola : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsServiceCompleted",
                table: "Appointments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ServiceNotes",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsServiceCompleted",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ServiceNotes",
                table: "Appointments");
        }
    }
}
