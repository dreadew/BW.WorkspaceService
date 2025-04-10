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

            migrationBuilder.CreateTable(
                name: "Workspaces",
                schema: "workspace",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PictureUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workspaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkspaceDirectory",
                schema: "workspace",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    WorkspaceId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceDirectory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkspaceDirectory_Workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalSchema: "workspace",
                        principalTable: "Workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkspacePositions",
                schema: "workspace",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    WorkspaceId = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspacePositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkspacePositions_Workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalSchema: "workspace",
                        principalTable: "Workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkspaceRoles",
                schema: "workspace",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    WorkspaceId = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkspaceRoles_Workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalSchema: "workspace",
                        principalTable: "Workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nesting",
                columns: table => new
                {
                    ParentDirectoryId = table.Column<string>(type: "text", nullable: false),
                    ChildDirectoryId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nesting", x => new { x.ParentDirectoryId, x.ChildDirectoryId });
                    table.ForeignKey(
                        name: "FK_Nesting_WorkspaceDirectory_ChildDirectoryId",
                        column: x => x.ChildDirectoryId,
                        principalSchema: "workspace",
                        principalTable: "WorkspaceDirectory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Nesting_WorkspaceDirectory_ParentDirectoryId",
                        column: x => x.ParentDirectoryId,
                        principalSchema: "workspace",
                        principalTable: "WorkspaceDirectory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkspaceDirectoryArtifact",
                schema: "workspace",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Url = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    DirectoryId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChangedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceDirectoryArtifact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkspaceDirectoryArtifact_WorkspaceDirectory_DirectoryId",
                        column: x => x.DirectoryId,
                        principalSchema: "workspace",
                        principalTable: "WorkspaceDirectory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkspaceRoleClaims",
                schema: "workspace",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    WorkspaceRolesId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkspaceRoleClaims_WorkspaceRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "workspace",
                        principalTable: "WorkspaceRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkspaceRoleClaims_WorkspaceRoles_WorkspaceRolesId",
                        column: x => x.WorkspaceRolesId,
                        principalSchema: "workspace",
                        principalTable: "WorkspaceRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkspaceUsers",
                schema: "workspace",
                columns: table => new
                {
                    WorkspaceId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    RoleId1 = table.Column<string>(type: "text", nullable: false),
                    PositionId = table.Column<string>(type: "text", nullable: false),
                    PositionId1 = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceUsers", x => new { x.WorkspaceId, x.UserId });
                    table.ForeignKey(
                        name: "FK_WorkspaceUsers_WorkspacePositions_PositionId",
                        column: x => x.PositionId,
                        principalSchema: "workspace",
                        principalTable: "WorkspacePositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkspaceUsers_WorkspacePositions_PositionId1",
                        column: x => x.PositionId1,
                        principalSchema: "workspace",
                        principalTable: "WorkspacePositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkspaceUsers_WorkspaceRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "workspace",
                        principalTable: "WorkspaceRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkspaceUsers_WorkspaceRoles_RoleId1",
                        column: x => x.RoleId1,
                        principalSchema: "workspace",
                        principalTable: "WorkspaceRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkspaceUsers_Workspaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalSchema: "workspace",
                        principalTable: "Workspaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nesting_ChildDirectoryId",
                table: "Nesting",
                column: "ChildDirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceDirectory_WorkspaceId",
                schema: "workspace",
                table: "WorkspaceDirectory",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceDirectoryArtifact_DirectoryId",
                schema: "workspace",
                table: "WorkspaceDirectoryArtifact",
                column: "DirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspacePositions_WorkspaceId",
                schema: "workspace",
                table: "WorkspacePositions",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceRoleClaims_RoleId",
                schema: "workspace",
                table: "WorkspaceRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceRoleClaims_WorkspaceRolesId",
                schema: "workspace",
                table: "WorkspaceRoleClaims",
                column: "WorkspaceRolesId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceRoles_WorkspaceId",
                schema: "workspace",
                table: "WorkspaceRoles",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceUsers_PositionId",
                schema: "workspace",
                table: "WorkspaceUsers",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceUsers_PositionId1",
                schema: "workspace",
                table: "WorkspaceUsers",
                column: "PositionId1");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceUsers_RoleId",
                schema: "workspace",
                table: "WorkspaceUsers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceUsers_RoleId1",
                schema: "workspace",
                table: "WorkspaceUsers",
                column: "RoleId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Nesting");

            migrationBuilder.DropTable(
                name: "WorkspaceDirectoryArtifact",
                schema: "workspace");

            migrationBuilder.DropTable(
                name: "WorkspaceRoleClaims",
                schema: "workspace");

            migrationBuilder.DropTable(
                name: "WorkspaceUsers",
                schema: "workspace");

            migrationBuilder.DropTable(
                name: "WorkspaceDirectory",
                schema: "workspace");

            migrationBuilder.DropTable(
                name: "WorkspacePositions",
                schema: "workspace");

            migrationBuilder.DropTable(
                name: "WorkspaceRoles",
                schema: "workspace");

            migrationBuilder.DropTable(
                name: "Workspaces",
                schema: "workspace");
        }
    }
}
