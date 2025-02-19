using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Infrastructure.Data.Configuration;

public class WorkspacePositionsConfiguration : IEntityTypeConfiguration<WorkspacePositions>
{
    public void Configure(EntityTypeBuilder<WorkspacePositions> builder)
    {
        builder.ToTable("WorkspacePositions", "workspace");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .IsRequired();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(p => p.WorkspaceId)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.ModifiedAt);

        builder.HasOne(p => p.Workspace)
            .WithMany(w => w.Positions)
            .HasForeignKey(p => p.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}