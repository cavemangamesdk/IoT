using MotionController.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotionController.Sensor.Db.Data.Models;

[Schema("H4")]
[Table("GameSession")]
public sealed class GameSession : DatabaseModel
{
    [Key]
    [Column("Id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("SessionId")]
    public Guid SessionId { get; set; }

    [Column("PlayerName")]
    public string? PlayerName { get; set; }

    [Column("Lives")]
    public int Lives { get; set; }

    [Column("GameTime")]
    public TimeSpan GameTime { get; set; }

    [Column("Created")]
    public DateTime Created { get; set; }

    [Column("Modified")]
    public DateTime Modified { get; set; }
}
