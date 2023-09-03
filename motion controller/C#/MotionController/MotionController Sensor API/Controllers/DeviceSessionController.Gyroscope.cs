using Microsoft.AspNetCore.Mvc;
using MotionController.Sensor.Db.Data.Models;
using NSwag.Annotations;

namespace MotionController.API.Controllers;

public partial class DeviceSessionController
{
    [HttpGet]
    [Route("{sessionId:guid}/gyroscope", Name = nameof(GetDeviceSessionGyroscopeAsync))]
    [OpenApiOperation(nameof(GetDeviceSessionGyroscopeAsync), "Get a Device Session gyroscope by Session Id", "")]
    [ProducesResponseType(typeof(IEnumerable<DeviceSessionGyroscope>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDeviceSessionGyroscopeAsync([FromRoute] Guid sessionId)
    {
        try
        {
            var deviceSession = await DeviceSessionService.GetDeviceSessionAsync(sessionId);
            if (deviceSession?.Equals(default) ?? true)
            {
                return NotFound();
            }

            var deviceSessionGyroscopes = await DeviceSessionGyroscopeService.GetDeviceSessionGyroscopeAsync(deviceSession);
            if (deviceSessionGyroscopes?.Equals(default) ?? true)
            {
                deviceSessionGyroscopes = Array.Empty<DeviceSessionGyroscope>();
            }

            return Ok(deviceSessionGyroscopes);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"{nameof(GetDeviceSessionGyroscopeAsync)} operation failed.");
            throw;
        }
    }
}
