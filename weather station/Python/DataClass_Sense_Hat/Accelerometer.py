from dataclasses import dataclass

@dataclass
class Accelerometer():
    # from get_accelerometer()
    roll: float
    pitch: float
    yaw: float
    # from get_accelerometer_raw()
    x_raw: float
    y_raw: float
    z_raw: float