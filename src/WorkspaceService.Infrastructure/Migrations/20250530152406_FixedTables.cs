using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkspaceService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_workspace_role_claim_workspace_role_WorkspaceRolesId",
                schema: "auth",
                table: "workspace_role_claim");

            migrationBuilder.RenameColumn(
                name: "WorkspaceRolesId",
                schema: "auth",
                table: "workspace_role_claim",
                newName: "WorkspaceRoleId");

            migrationBuilder.RenameIndex(
                name: "IX_workspace_role_claim_WorkspaceRolesId",
                schema: "auth",
                table: "workspace_role_claim",
                newName: "IX_workspace_role_claim_WorkspaceRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_workspace_role_claim_workspace_role_WorkspaceRoleId",
                schema: "auth",
                table: "workspace_role_claim",
                column: "WorkspaceRoleId",
                principalSchema: "auth",
                principalTable: "workspace_role",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_workspace_role_claim_workspace_role_WorkspaceRoleId",
                schema: "auth",
                table: "workspace_role_claim");

            migrationBuilder.RenameColumn(
                name: "WorkspaceRoleId",
                schema: "auth",
                table: "workspace_role_claim",
                newName: "WorkspaceRolesId");

            migrationBuilder.RenameIndex(
                name: "IX_workspace_role_claim_WorkspaceRoleId",
                schema: "auth",
                table: "workspace_role_claim",
                newName: "IX_workspace_role_claim_WorkspaceRolesId");

            migrationBuilder.AddForeignKey(
                name: "FK_workspace_role_claim_workspace_role_WorkspaceRolesId",
                schema: "auth",
                table: "workspace_role_claim",
                column: "WorkspaceRolesId",
                principalSchema: "auth",
                principalTable: "workspace_role",
                principalColumn: "Id");
        }
    }
}
