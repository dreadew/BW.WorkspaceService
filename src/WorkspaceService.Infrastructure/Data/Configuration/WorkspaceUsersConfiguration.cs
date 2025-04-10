using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Infrastructure.Data.Configuration;

public class WorkspaceUsersConfiguration : IEntityTypeConfiguration<WorkspaceUsers>
{
    public void Configure(EntityTypeBuilder<WorkspaceUsers> builder)
    {
        builder.ToTable("workspace_user", "workspace");
        
        builder.HasKey(wu => new { wu.WorkspaceId, wu.UserId });

        builder.Property(wu => wu.WorkspaceId)
            .IsRequired();

        builder.Property(wu => wu.UserId)
            .IsRequired();

        builder.Property(wu => wu.RoleId)
            .IsRequired();

        builder.Property(wu => wu.PositionId)
            .IsRequired();
        
        builder.HasOne<WorkspaceRoles>()
            .WithMany()
            .HasForeignKey(wu => wu.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<WorkspacePositions>()
            .WithMany()
            .HasForeignKey(wu => wu.PositionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}