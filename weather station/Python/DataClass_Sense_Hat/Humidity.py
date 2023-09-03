from dataclasses import dataclass

@dataclass
class Humidity():
    # from get_humidity()
    humidity: float
    # from get_temperature_from_humidity()
    temperature: float