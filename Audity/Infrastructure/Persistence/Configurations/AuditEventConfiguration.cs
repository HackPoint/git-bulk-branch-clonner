using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations; 

public class AuditEventConfiguration : IEntityTypeConfiguration<AuditEvent> {
    public void Configure(EntityTypeBuilder<AuditEvent> builder) {
        builder.Property(cfg => cfg.CreatedAt)
            .HasColumnName("created_at")
            .HasConversion<DateTimeOffset>();
        
        builder.Property(cfg => cfg.ApplicationName)
            .HasColumnName("application_name")
            .IsRequired();
        
        builder.Property(cfg => cfg.UpdatedBy)
            .HasColumnName("updated_by")
            .IsRequired();
        
        builder.Property(cfg => cfg.Location)
            .HasColumnName("location")
            .IsRequired();
        
        builder.Property(cfg => cfg.ApplicationScreen)
            .HasColumnName("application_screen")
            .IsRequired();
        
        builder.Property(cfg => cfg.ChangeType)
            .HasColumnName("change_type")
            .IsRequired();
        
        builder.Property(cfg => cfg.DcaId)
            .HasColumnName("dca_id")
            .IsRequired();
    }
    
}