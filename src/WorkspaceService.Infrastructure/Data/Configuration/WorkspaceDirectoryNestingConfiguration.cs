using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Infrastructure.Data.Configuration;

public class WorkspaceDirectoryNestingConfiguration : IEntityTypeConfiguration<WorkspaceDirectoryNesting>
{
    public void Configure(EntityTypeBuilder<WorkspaceDirectoryNesting> builder)
    {
        builder.ToTable("workspace_directory_nesting", "workspace");
        builder.HasKey(dn => new { dn.ParentDirectoryId, dn.ChildDirectoryId });

        builder.HasOne(dn => dn.ParentDirectoryNavigation)
            .WithMany(d => d.ChildNesting)
            .HasForeignKey(dn => dn.ParentDirectoryId);

        builder.HasOne(dn => dn.ChildDirectoryNavigation)
            .WithMany(d => d.ParentNesting)
            .HasForeignKey(dn => dn.ChildDirectoryId);
    }
}