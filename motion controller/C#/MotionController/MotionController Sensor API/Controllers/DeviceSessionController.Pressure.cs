using Microsoft.AspNetCore.Mvc;
using MotionController.Sensor.Db.Data.Models;
using NSwag.Annotations;

namespace MotionController.API.Controllers;

public partial class DeviceSessionController
{
    [HttpGet]
    [Route("{sessionId:guid}/pressure", Name = nameof(GetDeviceSessionPressureAsync))]
    [OpenApiOperation(nameof(GetDeviceSessionPressureAsync), "Get a Device Session pressure by Session Id", "")]
    [ProducesResponseType(typeof(IEnumerable<DeviceSessionPressure>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDeviceSessionPressureAsync([FromRoute] Guid sessionId)
    {
        try
        {
            var deviceSession = await DeviceSessionService.GetDeviceSessionAsync(sessionId);
            if (deviceSession?.Equals(default) ?? true)
            {
                return NotFound();
            }

            var deviceSessionPressures = await DeviceSessionPressureService.GetDeviceSessionPressuresAsync(deviceSession);
            if (deviceSessionPressures?.Equals(default) ?? true)
            {
                deviceSessionPressures = Array.Empty<DeviceSessionPressure>();
            }

            return Ok(deviceSessionPressures);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"{nameof(GetDeviceSessionPressureAsync)} operation failed.");
            throw;
        }
    }
}
