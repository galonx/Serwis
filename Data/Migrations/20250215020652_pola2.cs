using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Serwis.Data.Migrations
{
    /// <inheritdoc />
    public partial class pola2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ServiceCompletionDate",
                table: "Appointments",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceCompletionDate",
                table: "Appointments");
        }
    }
}
