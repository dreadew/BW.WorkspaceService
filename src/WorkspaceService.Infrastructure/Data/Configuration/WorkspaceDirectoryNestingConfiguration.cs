using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Infrastructure.Data.Configuration;

public class WorkspaceDirectoryNestingConfiguration : IEntityTypeConfiguration<WorkspaceDirectoryNesting>
{
    public void Configure(EntityTypeBuilder<WorkspaceDirectoryNesting> builder)
    {
        builder.HasKey(dn => new { dn.ParentDirectoryId, dn.ChildDirectoryId });

        builder.HasOne(dn => dn.ParentDirectoryNavigation)
            .WithMany(d => d.ChildNestings)
            .HasForeignKey(dn => dn.ParentDirectoryId);

        builder.HasOne(dn => dn.ChildDirectoryNavigation)
            .WithMany(d => d.ParentNestings)
            .HasForeignKey(dn => dn.ChildDirectoryId);
    }
}