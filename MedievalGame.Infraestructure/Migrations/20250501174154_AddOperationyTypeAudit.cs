using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedievalGame.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOperationyTypeAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "CharacterAuditLogs",
                newName: "Date");

            migrationBuilder.AddColumn<string>(
                name: "OperationType",
                table: "CharacterAuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperationType",
                table: "CharacterAuditLogs");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "CharacterAuditLogs",
                newName: "CreatedAt");
        }
    }
}
