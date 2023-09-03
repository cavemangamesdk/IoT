from dataclasses import dataclass

from DataClass_Sense_Hat.Accelerometer import Accelerometer
from DataClass_Sense_Hat.Gyroscope import Gyroscope
from DataClass_Sense_Hat.Magnetometer import Magnetometer
from DataClass_Sense_Hat.Orientation import Orientation

# dataclass for all SenseHat IMU data 
@dataclass
class Imu():
    accelerometer_sensor: Accelerometer.__dict__
    gyroscope_sensor: Gyroscope.__dict__
    magnetometer_sensor: Magnetometer.__dict__
    orientation_sensor: Orientation.__dict__