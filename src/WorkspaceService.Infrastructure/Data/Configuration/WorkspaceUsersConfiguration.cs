using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Infrastructure.Data.Configuration;

public class WorkspaceUsersConfiguration : IEntityTypeConfiguration<WorkspaceUser>
{
    public void Configure(EntityTypeBuilder<WorkspaceUser> builder)
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
        
        builder.HasOne<WorkspaceRole>()
            .WithMany()
            .HasForeignKey(wu => wu.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<WorkspacePosition>()
            .WithMany()
            .HasForeignKey(wu => wu.PositionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}