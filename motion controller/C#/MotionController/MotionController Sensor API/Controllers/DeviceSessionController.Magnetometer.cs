using Microsoft.AspNetCore.Mvc;
using MotionController.Sensor.Db.Data.Models;
using NSwag.Annotations;

namespace MotionController.API.Controllers;

public partial class DeviceSessionController
{
    [HttpGet]
    [Route("{sessionId:guid}/magnetometer", Name = nameof(GetDeviceSessionMagnetometerAsync))]
    [OpenApiOperation(nameof(GetDeviceSessionMagnetometerAsync), "Get a Device Session magnetometer by Session Id", "")]
    [ProducesResponseType(typeof(IEnumerable<DeviceSessionMagnetometer>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDeviceSessionMagnetometerAsync([FromRoute] Guid sessionId)
    {
        try
        {
            var deviceSession = await DeviceSessionService.GetDeviceSessionAsync(sessionId);
            if (deviceSession?.Equals(default) ?? true)
            {
                return NotFound();
            }

            var deviceSessionMagnetometers = await DeviceSessionMagnetometerService.GetDeviceSessionMagnetometerAsync(deviceSession);
            if (deviceSessionMagnetometers?.Equals(default) ?? true)
            {
                deviceSessionMagnetometers = Array.Empty<DeviceSessionMagnetometer>();
            }

            return Ok(deviceSessionMagnetometers);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"{nameof(GetDeviceSessionMagnetometerAsync)} operation failed.");
            throw;
        }
    }
}
