using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Infrastructure.Data.Configuration;

public class WorkspaceDirectoryConfiguration : IEntityTypeConfiguration<WorkspaceDirectory>
{
    public void Configure(EntityTypeBuilder<WorkspaceDirectory> builder)
    {
        builder.ToTable("workspace_directory", "workspace");

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id)
            .IsRequired();

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(256);
        builder.HasIndex(d => new { d.Name, d.ObjectId })
            .IsUnique();

        builder.Property(d => d.ObjectId)
            .IsRequired();

        builder.Property(d => d.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(d => d.CreatedAt)
            .IsRequired();

        builder.Property(d => d.UpdatedAt);

        builder.HasOne<Workspace>()
            .WithMany(w => w.Directories)
            .HasForeignKey(d => d.ObjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<WorkspaceDirectory>(x => x.Parent)
            .WithMany(x => x.Children)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<WorkspaceDirectoryArtifact>(d => d.Artifacts)
            .WithOne(x => x.Directory)
            .HasForeignKey(a => a.DirectoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}