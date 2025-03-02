using Microsoft.EntityFrameworkCore;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Infrastructure.Data.Configuration;

namespace WorkspaceService.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Workspaces> Workspaces { get; set; } = null!;
    public DbSet<WorkspaceRoles> Roles { get; set; } = null!;
    public DbSet<WorkspaceRoleClaims> RoleClaims { get; set; } = null!;
    public DbSet<WorkspacePositions> Positions { get; set; } = null!;
    public DbSet<WorkspaceUsers> Users { get; set; } = null!;
    public DbSet<WorkspaceDirectory> Directories { get; set; } = null!;
    public DbSet<WorkspaceDirectoryNesting> Nestings { get; set; } = null!;
    public DbSet<WorkspaceDirectoryArtifact> Artifacts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new WorkspacesConfiguration());
        modelBuilder.ApplyConfiguration(new WorkspaceUsersConfiguration());
        modelBuilder.ApplyConfiguration(new WorkspaceRolesConfiguration());
        modelBuilder.ApplyConfiguration(new WorkspaceRoleClaimsConfiguration());
        modelBuilder.ApplyConfiguration(new WorkspacePositionsConfiguration());
        modelBuilder.ApplyConfiguration(new WorkspaceDirectoryConfiguration());
        modelBuilder.ApplyConfiguration(new WorkspaceDirectoryNestingConfiguration());
        modelBuilder.ApplyConfiguration(new WorkspaceDirectoryArtifactConfiguration());
        base.OnModelCreating(modelBuilder);
    }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {}
}