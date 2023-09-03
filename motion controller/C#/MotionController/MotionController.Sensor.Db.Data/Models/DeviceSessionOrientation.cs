using MotionController.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotionController.Sensor.Db.Data.Models;

[Schema("H4")]
[Table("DeviceSessionOrientation")]
public sealed class DeviceSessionOrientation : DatabaseModel
{
    [Key]
    [Column("Id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("DeviceSession_Id")]
    public int DeviceSessionId { get; set; }

    [Column("RollDegrees")]
    public float RollDegrees { get; set; }

    [Column("PitchDegrees")]
    public float PitchDegrees { get; set; }

    [Column("YawDegrees")]
    public float YawDegrees { get; set; }

    [Column("RollRadians")]
    public float RollRadians { get; set; }

    [Column("PitchRadians")]
    public float PitchRadians { get; set; }

    [Column("YawRadians")]
    public float YawRadians { get; set; }

    [Column("Timestamp")]
    public DateTime Timestamp { get; set; }

    [Column("Created")]
    public DateTime Created { get; set; }

    [Column("Modified")]
    public DateTime Modified { get; set; }
}
