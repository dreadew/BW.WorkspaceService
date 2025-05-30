using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Infrastructure.Data.Configuration;

public class WorkspaceRolesConfiguration : IEntityTypeConfiguration<WorkspaceRole>
{
    public void Configure(EntityTypeBuilder<WorkspaceRole> builder)
    {
        builder.ToTable("workspace_role", "auth");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .IsRequired();

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(128);
        builder.HasIndex(r => new { r.WorkspaceId, r.Name })
            .IsUnique();

        builder.Property(r => r.WorkspaceId)
            .IsRequired();

        builder.Property(r => r.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.ModifiedAt);

        builder.Property(r => r.ChangedBy);

        builder.HasOne(r => r.Workspace)
            .WithMany(w => w.Roles)
            .HasForeignKey(r => r.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}