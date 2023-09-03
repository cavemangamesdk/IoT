using Microsoft.AspNetCore.Mvc;
using MotionController.Sensor.Db.Data.Models;
using NSwag.Annotations;

namespace MotionController.API.Controllers;

public partial class DeviceSessionController
{
    [HttpGet]
    [Route("{sessionId:guid}/orientation", Name = nameof(GetDeviceSessionOrientationAsync))]
    [OpenApiOperation(nameof(GetDeviceSessionOrientationAsync), "Get a Device Session orientation by Session Id", "")]
    [ProducesResponseType(typeof(IEnumerable<DeviceSessionOrientation>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDeviceSessionOrientationAsync([FromRoute] Guid sessionId)
    {
        try
        {
            var deviceSession = await DeviceSessionService.GetDeviceSessionAsync(sessionId);
            if (deviceSession?.Equals(default) ?? true)
            {
                return NotFound();
            }

            var deviceSessionOrientations = await DeviceSessionOrientationService.GetDeviceSessionOrientationAsync(deviceSession);
            if (deviceSessionOrientations?.Equals(default) ?? true)
            {
                deviceSessionOrientations = Array.Empty<DeviceSessionOrientation>();
            }

            return Ok(deviceSessionOrientations);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"{nameof(GetDeviceSessionOrientationAsync)} operation failed.");
            throw;
        }
    }
}
