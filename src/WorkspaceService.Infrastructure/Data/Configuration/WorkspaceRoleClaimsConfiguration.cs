using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Infrastructure.Data.Configuration;

public class WorkspaceRoleClaimsConfiguration : IEntityTypeConfiguration<WorkspaceRoleClaims>
{
    public void Configure(EntityTypeBuilder<WorkspaceRoleClaims> builder)
    {
        builder.ToTable("WorkspaceRoleClaims", "workspace");

        builder.HasKey(rc => rc.Id);
        builder.Property(rc => rc.Id)
            .IsRequired();

        builder.Property(rc => rc.Value)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(rc => rc.RoleId)
            .IsRequired();

        builder.HasOne(rc => rc.Role)
            .WithMany()
            .HasForeignKey(rc => rc.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}