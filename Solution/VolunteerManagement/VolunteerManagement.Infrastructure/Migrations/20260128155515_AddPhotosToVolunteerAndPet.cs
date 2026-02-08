using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;

#nullable disable

namespace VolunteerManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotosToVolunteerAndPet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<IReadOnlyList<Photo>>(
                name: "Photos",
                table: "Volunteers",
                type: "jsonb",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photos",
                table: "Volunteers");
        }
    }
}
