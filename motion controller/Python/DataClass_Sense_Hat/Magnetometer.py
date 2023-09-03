from dataclasses import dataclass

@dataclass
class Magnetometer():
    # from get_compass()
    north: float
    # from get_compass_raw()
    x_raw: float
    y_raw: float
    z_raw: float