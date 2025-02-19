using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Infrastructure.Data.Configuration;

public class WorkspaceDirectoryArtifactConfiguration : IEntityTypeConfiguration<WorkspaceDirectoryArtifact>
{
    public void Configure(EntityTypeBuilder<WorkspaceDirectoryArtifact> builder)
    {
        builder.ToTable("WorkspaceDirectoryArtifact", "workspace");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .IsRequired();

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(a => a.Url)
            .IsRequired()
            .HasMaxLength(1024);

        builder.Property(a => a.DirectoryId)
            .IsRequired();

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.Property(a => a.ModifiedAt);

        builder.HasOne(a => a.Directory)
            .WithMany(d => d.Artifacts)
            .HasForeignKey(a => a.DirectoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}