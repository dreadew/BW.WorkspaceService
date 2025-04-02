using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Infrastructure.Data.Configuration;

public class WorkspaceDirectoryConfiguration : IEntityTypeConfiguration<WorkspaceDirectory>
{
    public void Configure(EntityTypeBuilder<WorkspaceDirectory> builder)
    {
        builder.ToTable("WorkspaceDirectory", "workspace");

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id)
            .IsRequired();

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(d => d.WorkspaceId)
            .IsRequired();

        builder.Property(d => d.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(d => d.CreatedAt)
            .IsRequired();

        builder.Property(d => d.ModifiedAt);

        builder.Property(d => d.ChangedBy);

        builder.HasOne(d => d.Workspace)
            .WithMany(w => w.Directories)
            .HasForeignKey(d => d.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Artifacts)
            .WithOne(a => a.Directory)
            .HasForeignKey(a => a.DirectoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}