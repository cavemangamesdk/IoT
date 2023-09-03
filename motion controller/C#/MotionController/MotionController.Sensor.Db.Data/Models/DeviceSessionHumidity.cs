using MotionController.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotionController.Sensor.Db.Data.Models;

[Schema("H4")]
[Table("DeviceSessionHumidity")]
public sealed class DeviceSessionHumidity : DatabaseModel
{
    [Key]
    [Column("Id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("DeviceSession_Id")]
    public int DeviceSessionId { get; set; }

    [Column("TemperatureCelsius")]
    public float TemperatureCelsius { get; set; }

    [Column("HumidityPercentage")]
    public float HumidityPercentage { get; set; }

    [Column("Timestamp")]
    public DateTime Timestamp { get; set; }

    [Column("Created")]
    public DateTime Created { get; set; }

    [Column("Modified")]
    public DateTime Modified { get; set; }
}
