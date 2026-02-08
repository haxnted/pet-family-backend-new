using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using VolunteerManagement.Domain.Aggregates.Volunteers.ValueObjects.Properties;

#nullable disable

namespace VolunteerManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DetectPendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photos",
                table: "Volunteers");

            migrationBuilder.AddColumn<Guid>(
                name: "Photo",
                table: "Volunteers",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Volunteers");

            migrationBuilder.AddColumn<IReadOnlyList<Photo>>(
                name: "Photos",
                table: "Volunteers",
                type: "jsonb",
                nullable: false);
        }
    }
}
