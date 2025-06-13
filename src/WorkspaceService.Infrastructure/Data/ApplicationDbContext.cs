using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Infrastructure.Data.Configuration;

namespace WorkspaceService.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Workspace> Workspaces { get; set; } = null!;
    public DbSet<WorkspaceRole> Roles { get; set; } = null!;
    public DbSet<WorkspaceRoleClaim> RoleClaims { get; set; } = null!;
    public DbSet<WorkspacePosition> Positions { get; set; } = null!;
    public DbSet<WorkspaceUser> Users { get; set; } = null!;
    public DbSet<WorkspaceDirectory> Directories { get; set; } = null!;
    public DbSet<WorkspaceDirectoryArtifact> Artifacts { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {}
}