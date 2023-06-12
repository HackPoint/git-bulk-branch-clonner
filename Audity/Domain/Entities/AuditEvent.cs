using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities; 
[Table("audit")]
public class AuditEvent : BaseAuditEntity {
    public string Location { get; set; }
    public string ApplicationName { get; set; }
    public string ApplicationScreen { get; set; }
    public string ChangeType { get; set; }
    public Guid DcaId { get; set; }
    public Guid UpdatedBy { get; init; }
}