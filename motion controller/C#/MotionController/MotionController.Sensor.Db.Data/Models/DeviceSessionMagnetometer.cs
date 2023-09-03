using MotionController.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotionController.Sensor.Db.Data.Models;

[Schema("H4")]
[Table("DeviceSessionMagnetometer")]
public sealed class DeviceSessionMagnetometer : DatabaseModel
{
    [Key]
    [Column("Id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("DeviceSession_Id")]
    public int DeviceSessionId { get; set; }

    [Column("North")]
    public float North { get; set; }

    [Column("XRaw")]
    public float XRaw { get; set; }

    [Column("YRaw")]
    public float YRaw { get; set; }

    [Column("ZRaw")]
    public float ZRaw { get; set; }

    [Column("Timestamp")]
    public DateTime Timestamp { get; set; }

    [Column("Created")]
    public DateTime Created { get; set; }

    [Column("Modified")]
    public DateTime Modified { get; set; }
}
