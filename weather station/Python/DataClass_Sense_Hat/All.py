from dataclasses import dataclass

from DataClass_Sense_Hat.Env import Env
from DataClass_Sense_Hat.Imu import Imu

# dataclass for all the data from the SenseHat
@dataclass
class All():
    env: Env.__dict__
    imu: Imu.__dict__
