using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Infrastructure.Data.Configuration;

public class WorkspaceDirectoryArtifactConfiguration : IEntityTypeConfiguration<WorkspaceDirectoryArtifact>
{
    public void Configure(EntityTypeBuilder<WorkspaceDirectoryArtifact> builder)
    {
        builder.ToTable("workspace_directory_artifact", "workspace");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .IsRequired();

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(a => a.Path)
            .IsRequired()
            .HasMaxLength(1024);

        builder.Property(a => a.DirectoryId)
            .IsRequired();

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.Property(a => a.UpdatedAt);

        builder.HasOne<WorkspaceDirectory>(a => a.Directory)
            .WithMany(d => d.Artifacts)
            .HasForeignKey(a => a.DirectoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}