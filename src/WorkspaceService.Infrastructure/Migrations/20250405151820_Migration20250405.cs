using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkspaceService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration20250405 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureUrl",
                schema: "workspace",
                table: "Workspaces");

            migrationBuilder.RenameColumn(
                name: "Url",
                schema: "workspace",
                table: "WorkspaceDirectoryArtifact",
                newName: "Path");

            migrationBuilder.AddColumn<string>(
                name: "PositionId1",
                schema: "workspace",
                table: "WorkspaceUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RoleId1",
                schema: "workspace",
                table: "WorkspaceUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PicturePath",
                schema: "workspace",
                table: "Workspaces",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceUsers_PositionId1",
                schema: "workspace",
                table: "WorkspaceUsers",
                column: "PositionId1");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceUsers_RoleId1",
                schema: "workspace",
                table: "WorkspaceUsers",
                column: "RoleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkspaceUsers_WorkspacePositions_PositionId1",
                schema: "workspace",
                table: "WorkspaceUsers",
                column: "PositionId1",
                principalSchema: "workspace",
                principalTable: "WorkspacePositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkspaceUsers_WorkspaceRoles_RoleId1",
                schema: "workspace",
                table: "WorkspaceUsers",
                column: "RoleId1",
                principalSchema: "workspace",
                principalTable: "WorkspaceRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkspaceUsers_WorkspacePositions_PositionId1",
                schema: "workspace",
                table: "WorkspaceUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkspaceUsers_WorkspaceRoles_RoleId1",
                schema: "workspace",
                table: "WorkspaceUsers");

            migrationBuilder.DropIndex(
                name: "IX_WorkspaceUsers_PositionId1",
                schema: "workspace",
                table: "WorkspaceUsers");

            migrationBuilder.DropIndex(
                name: "IX_WorkspaceUsers_RoleId1",
                schema: "workspace",
                table: "WorkspaceUsers");

            migrationBuilder.DropColumn(
                name: "PositionId1",
                schema: "workspace",
                table: "WorkspaceUsers");

            migrationBuilder.DropColumn(
                name: "RoleId1",
                schema: "workspace",
                table: "WorkspaceUsers");

            migrationBuilder.DropColumn(
                name: "PicturePath",
                schema: "workspace",
                table: "Workspaces");

            migrationBuilder.RenameColumn(
                name: "Path",
                schema: "workspace",
                table: "WorkspaceDirectoryArtifact",
                newName: "Url");

            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                schema: "workspace",
                table: "Workspaces",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");
        }
    }
}
