using Microsoft.AspNetCore.Mvc;
using MotionController.Sensor.Db.Data.Models;
using NSwag.Annotations;

namespace MotionController.API.Controllers;

public partial class DeviceSessionController
{
    [HttpGet]
    [Route("{sessionId:guid}/accelerometer", Name = nameof(GetDeviceSessionAccelerometerAsync))]
    [OpenApiOperation(nameof(GetDeviceSessionAccelerometerAsync), "Get a Device Session accelerometer by Session Id", "")]
    [ProducesResponseType(typeof(IEnumerable<DeviceSessionAccelerometer>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDeviceSessionAccelerometerAsync([FromRoute] Guid sessionId)
    {
        try
        {
            var deviceSession = await DeviceSessionService.GetDeviceSessionAsync(sessionId);
            if (deviceSession?.Equals(default) ?? true)
            {
                return NotFound();
            }

            var deviceSessionAccelerometers = await DeviceSessionAccelerometerService.GetDeviceSessionAccelerometerAsync(deviceSession);
            if (deviceSessionAccelerometers?.Equals(default) ?? true)
            {
                deviceSessionAccelerometers = Array.Empty<DeviceSessionAccelerometer>();
            }

            return Ok(deviceSessionAccelerometers);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"{nameof(GetDeviceSessionAccelerometerAsync)} operation failed.");
            throw;
        }
    }
}
