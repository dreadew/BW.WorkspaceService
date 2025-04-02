using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkspaceService.Infrastructure.src.WorkspaceService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration2025032601 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                schema: "workspace",
                table: "Workspaces",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "workspace",
                table: "Workspaces",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "workspace",
                table: "Workspaces",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                schema: "workspace",
                table: "WorkspaceRoles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "workspace",
                table: "WorkspaceRoles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                schema: "workspace",
                table: "WorkspacePositions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "workspace",
                table: "WorkspacePositions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                schema: "workspace",
                table: "WorkspaceDirectoryArtifact",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                schema: "workspace",
                table: "WorkspaceDirectory",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "workspace",
                table: "WorkspaceDirectory",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    EventType = table.Column<int>(type: "integer", nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    IsSent = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                schema: "workspace",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "workspace",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "workspace",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                schema: "workspace",
                table: "WorkspaceRoles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "workspace",
                table: "WorkspaceRoles");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                schema: "workspace",
                table: "WorkspacePositions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "workspace",
                table: "WorkspacePositions");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                schema: "workspace",
                table: "WorkspaceDirectoryArtifact");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                schema: "workspace",
                table: "WorkspaceDirectory");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "workspace",
                table: "WorkspaceDirectory");
        }
    }
}
