using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Infrastructure.Data.Configuration;

public class WorkspacesConfiguration : IEntityTypeConfiguration<Workspace>
{
    public void Configure(EntityTypeBuilder<Workspace> builder)
    {
        builder.ToTable("workspace", "workspace");

        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id)
            .IsRequired();

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(256);
        builder.HasIndex(w => w.Name)
            .IsUnique();

        builder.Property(w => w.PicturePath)
            .HasMaxLength(512);

        builder.Property(w => w.CreatedAt)
            .IsRequired();

        builder.Property(w => w.ModifiedAt);

        builder.Property(w => w.IsDeleted)
            .HasDefaultValue(false);
        
        builder.HasMany(w => w.Roles)
            .WithOne(r => r.Workspace)
            .HasForeignKey(r => r.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(w => w.Positions)
            .WithOne(p => p.Workspace)
            .HasForeignKey(p => p.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(w => w.Directories)
            .WithOne(d => d.Workspace)
            .HasForeignKey(d => d.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(w => w.Users)
            .WithOne()
            .HasForeignKey(u => u.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}