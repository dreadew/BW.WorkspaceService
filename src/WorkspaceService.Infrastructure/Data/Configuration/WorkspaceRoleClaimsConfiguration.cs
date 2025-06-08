using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Infrastructure.Data.Configuration;

public class WorkspaceRoleClaimsConfiguration : IEntityTypeConfiguration<WorkspaceRoleClaim>
{
    public void Configure(EntityTypeBuilder<WorkspaceRoleClaim> builder)
    {
        builder.ToTable("workspace_role_claim", "auth");

        builder.HasKey(rc => rc.Id);
        builder.Property(rc => rc.Id)
            .IsRequired();

        builder.Property(rc => rc.Value)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(rc => rc.RoleId)
            .IsRequired();

        builder.HasOne<WorkspaceRole>()
            .WithMany(r => r.Claims)
            .HasForeignKey(rc => rc.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}