using Microsoft.AspNetCore.Mvc;
using MotionController.Sensor.Db.Data.Models;
using NSwag.Annotations;

namespace MotionController.API.Controllers;

public partial class DeviceSessionController
{
    [HttpGet]
    [Route("{sessionId:guid}/humidity", Name = nameof(GetDeviceSessionHumidityAsync))]
    [OpenApiOperation(nameof(GetDeviceSessionHumidityAsync), "Get a Device Session humidity by Session Id", "")]
    [ProducesResponseType(typeof(IEnumerable<DeviceSessionHumidity>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDeviceSessionHumidityAsync([FromRoute] Guid sessionId)
    {
        try
        {
            var deviceSession = await DeviceSessionService.GetDeviceSessionAsync(sessionId);
            if (deviceSession?.Equals(default) ?? true)
            {
                return NotFound();
            }

            var deviceSessionHumidities = await DeviceSessionHumidityService.GetDeviceSessionHumidityAsync(deviceSession);
            if (deviceSessionHumidities?.Equals(default) ?? true)
            {
                deviceSessionHumidities = Array.Empty<DeviceSessionHumidity>();
            }

            return Ok(deviceSessionHumidities);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"{nameof(GetDeviceSessionHumidityAsync)} operation failed.");
            throw;
        }
    }
}
