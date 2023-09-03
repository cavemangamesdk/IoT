from dataclasses import dataclass

from DataClass_Sense_Hat.Humidity import Humidity
from DataClass_Sense_Hat.Pressure import Pressure

# Dataclass for all Sense Hat environment sensors
@dataclass
class Env():
    humidity_sensor: Humidity.__dict__
    pressure_sensor: Pressure.__dict__