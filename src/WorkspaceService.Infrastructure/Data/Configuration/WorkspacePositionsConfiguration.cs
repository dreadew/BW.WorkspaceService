using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Infrastructure.Data.Configuration;

public class WorkspacePositionsConfiguration : IEntityTypeConfiguration<WorkspacePosition>
{
    public void Configure(EntityTypeBuilder<WorkspacePosition> builder)
    {
        builder.ToTable("workspace_position", "workspace");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .IsRequired();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(128);
        builder.HasIndex(p => new { p.Name, p.WorkspaceId })
            .IsUnique();

        builder.Property(p => p.WorkspaceId)
            .IsRequired();

        builder.Property(p => p.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt);

        builder.HasOne<Workspace>()
            .WithMany(w => w.Positions)
            .HasForeignKey(p => p.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}