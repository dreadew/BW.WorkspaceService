using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkspaceService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "workspace");

            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<int>(type: "integer", nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    IsSent = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "workspace",
                schema: "workspace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Path = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workspace", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "workspace_directory",
                schema: "workspace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ObjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workspace_directory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workspace_directory_workspace_ObjectId",
                        column: x => x.ObjectId,
                        principalSchema: "workspace",
                        principalTable: "workspace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_workspace_directory_workspace_directory_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "workspace",
                        principalTable: "workspace_directory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workspace_position",
                schema: "workspace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workspace_position", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workspace_position_workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalSchema: "workspace",
                        principalTable: "workspace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workspace_role",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workspace_role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workspace_role_workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalSchema: "workspace",
                        principalTable: "workspace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workspace_directory_artifact",
                schema: "workspace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    DirectoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Path = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workspace_directory_artifact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workspace_directory_artifact_workspace_directory_DirectoryId",
                        column: x => x.DirectoryId,
                        principalSchema: "workspace",
                        principalTable: "workspace_directory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workspace_role_claim",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workspace_role_claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workspace_role_claim_workspace_role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "auth",
                        principalTable: "workspace_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workspace_user",
                schema: "workspace",
                columns: table => new
                {
                    WorkspaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    PositionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workspace_user", x => new { x.WorkspaceId, x.UserId });
                    table.ForeignKey(
                        name: "FK_workspace_user_workspace_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalSchema: "workspace",
                        principalTable: "workspace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_workspace_user_workspace_position_PositionId",
                        column: x => x.PositionId,
                        principalSchema: "workspace",
                        principalTable: "workspace_position",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_workspace_user_workspace_role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "auth",
                        principalTable: "workspace_role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_workspace_Name",
                schema: "workspace",
                table: "workspace",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_workspace_directory_Name_ObjectId",
                schema: "workspace",
                table: "workspace_directory",
                columns: new[] { "Name", "ObjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_workspace_directory_ObjectId",
                schema: "workspace",
                table: "workspace_directory",
                column: "ObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_workspace_directory_ParentId",
                schema: "workspace",
                table: "workspace_directory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_workspace_directory_artifact_DirectoryId",
                schema: "workspace",
                table: "workspace_directory_artifact",
                column: "DirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_workspace_position_Name_WorkspaceId",
                schema: "workspace",
                table: "workspace_position",
                columns: new[] { "Name", "WorkspaceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_workspace_position_WorkspaceId",
                schema: "workspace",
                table: "workspace_position",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_workspace_role_WorkspaceId_Name",
                schema: "auth",
                table: "workspace_role",
                columns: new[] { "WorkspaceId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_workspace_role_claim_RoleId",
                schema: "auth",
                table: "workspace_role_claim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_workspace_user_PositionId",
                schema: "workspace",
                table: "workspace_user",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_workspace_user_RoleId",
                schema: "workspace",
                table: "workspace_user",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "workspace_directory_artifact",
                schema: "workspace");

            migrationBuilder.DropTable(
                name: "workspace_role_claim",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "workspace_user",
                schema: "workspace");

            migrationBuilder.DropTable(
                name: "workspace_directory",
                schema: "workspace");

            migrationBuilder.DropTable(
                name: "workspace_position",
                schema: "workspace");

            migrationBuilder.DropTable(
                name: "workspace_role",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "workspace",
                schema: "workspace");
        }
    }
}
