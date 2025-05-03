using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedievalGame.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWeaponAuditTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeaponAuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WeaponId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponAuditLogs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeaponAuditLogs");
        }
    }
}
