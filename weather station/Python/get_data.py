#
from sense_hat import SenseHat
import datetime
import uuid

# Base data class
from DataClass_Sense_Hat.Base import Base

# Env sensors
from DataClass_Sense_Hat.Humidity import Humidity
from DataClass_Sense_Hat.Pressure import Pressure

# IMU sensors
from DataClass_Sense_Hat.Accelerometer import Accelerometer
from DataClass_Sense_Hat.Gyroscope import Gyroscope
from DataClass_Sense_Hat.Magnetometer import Magnetometer
from DataClass_Sense_Hat.Orientation import Orientation

# Aggregate data classes
from DataClass_Sense_Hat.Env import Env
from DataClass_Sense_Hat.Imu import Imu

# All data classes
from DataClass_Sense_Hat.All import All

#
# All data
#

def getAllDataBase(sense_hat: SenseHat) -> All:
    
    return All(
        env = getEnvDataBase(sense_hat).__dict__,
        imu = getImuDataBase(sense_hat).__dict__
    )

def getAllData(sense_hat: SenseHat, session_id: uuid.UUID, datetime: datetime) -> Base:
        
    return Base(
        session_id = str(session_id),
        timestamp = str(datetime.datetime.now()),
        data = getAllDataBase(sense_hat).__dict__
    )

#
# Aggregate data
#

def getEnvDataBase(sense_hat: SenseHat) -> Env:
    
    return Env(
        humidity_sensor = getHumidityDataBase(sense_hat).__dict__,
        pressure_sensor = getPressureDataBase(sense_hat).__dict__
    )

def getEnvData(sense_hat: SenseHat, session_id: uuid.UUID, datetime: datetime) -> Base:
        
    return Base(
        session_id = str(session_id),
        timestamp = str(datetime.datetime.now()),
        data = getEnvDataBase(sense_hat).__dict__
    )

def getImuDataBase(sense_hat: SenseHat) -> Imu:
        
    return Imu(
        accelerometer_sensor = getAccelerometerDataBase(sense_hat).__dict__,
        gyroscope_sensor = getGyroscopeDataBase(sense_hat).__dict__,
        magnetometer_sensor = getMagnetometerDataBase(sense_hat).__dict__,
        orientation_sensor = getOrientationDataBase(sense_hat).__dict__
    )

def getImuData(sense_hat: SenseHat, session_id: uuid.UUID, datetime: datetime) -> Base:
            
    return Base(
        session_id = str(session_id),
        timestamp = str(datetime.datetime.now()),
        data = getImuDataBase(sense_hat).__dict__
    )

#
# Environment data
#

# Humidity
def getHumidityDataBase(sense_hat: SenseHat) -> Humidity:

    return Humidity(
        humidity = sense_hat.get_humidity(),
        temperature = sense_hat.get_temperature_from_humidity()
    )

def getHumidityData(sense_hat: SenseHat, session_id: uuid.UUID, datetime: datetime) -> Base:
    
    return Base(
        session_id = str(session_id),
        timestamp = str(datetime.datetime.now()),
        data = getHumidityDataBase(sense_hat).__dict__
    )

# Pressure
def getPressureDataBase(sense_hat: SenseHat) -> Pressure:
            
    return Pressure(
        pressure = sense_hat.get_pressure(),
        temperature = sense_hat.get_temperature_from_pressure() 
    )

def getPressureData(sense_hat: SenseHat, session_id: uuid.UUID, datetime: datetime) -> Base:

    return Base(
        session_id = str(session_id),
        timestamp = str(datetime.datetime.now()),
        data = getPressureDataBase(sense_hat).__dict__
    )

#
# IMU data
# 

# Acceleration
def getAccelerometerDataBase(sense_hat: SenseHat) -> Accelerometer:
        
    accel = sense_hat.get_accelerometer()
    accelRaw = sense_hat.get_accelerometer_raw()

    return Accelerometer(
        roll = accel['roll'],
        pitch = accel['pitch'],
        yaw = accel['yaw'],
        x_raw = accelRaw['x'],
        y_raw = accelRaw['y'],
        z_raw = accelRaw['z']
    )

def getAccelerometerData(sense_hat: SenseHat, session_id: uuid.UUID, datetime: datetime) -> Base:
    
    return Base(
        session_id = str(session_id),
        timestamp = str(datetime.datetime.now()),
        data = getAccelerometerDataBase(sense_hat).__dict__
    )

# Gyroscope
def getGyroscopeDataBase(sense_hat: SenseHat) -> Gyroscope:
    
    gyro = sense_hat.get_gyroscope()
    gyroRaw = sense_hat.get_gyroscope_raw()

    return Gyroscope(
        roll = gyro['roll'],
        pitch = gyro['pitch'],
        yaw = gyro['yaw'],
        x_raw = gyroRaw['x'],
        y_raw = gyroRaw['y'],
        z_raw = gyroRaw['z']
    )

def getGyroscopeData(sense_hat: SenseHat, uuid: uuid.UUID, datetime: datetime) -> Base:
        
    return Base(
        session_id = str(uuid),
        timestamp = str(datetime.datetime.now()),
        data = getGyroscopeDataBase(sense_hat).__dict__
    )

# Magnetometer
def getMagnetometerDataBase(sense_hat: SenseHat) -> Magnetometer:
        
    compass = sense_hat.get_compass()
    compassRaw = sense_hat.get_compass_raw()

    return Magnetometer(
        north = compass,
        x_raw = compassRaw['x'],
        y_raw = compassRaw['y'],
        z_raw = compassRaw['z']
    )

def getMagnetometerData(sense_hat: SenseHat, uuid: uuid.UUID, datetime: datetime) -> Base:
        
    return Base(
        session_id = str(uuid),
        timestamp = str(datetime.datetime.now()),
        data = getMagnetometerDataBase(sense_hat).__dict__
    )

# Orientation
def getOrientationDataBase(sense_hat: SenseHat) -> Orientation:
        
    orientation = sense_hat.get_orientation_degrees()
    orientation_rad = sense_hat.get_orientation_radians()

    return Orientation(
        roll_deg = orientation['roll'],
        pitch_deg = orientation['pitch'],
        yaw_deg = orientation['yaw'],
        roll_rad = orientation_rad['roll'],
        pitch_rad = orientation_rad['pitch'],
        yaw_rad = orientation_rad['yaw']
    )

def getOrientationData(sense_hat: SenseHat, uuid: uuid.UUID, datetime: datetime) -> Base:
            
    return Base(
        session_id = str(uuid),
        timestamp = str(datetime.datetime.now()),
        data = getOrientationDataBase(sense_hat).__dict__
    )

#
# Pitch Roll data for Unity
#

def getPitchRollData(sense_hat: SenseHat) -> str:
            
    orientation = sense_hat.get_orientation_degrees()

    x = -round(orientation['roll'], 2)
    y = round(orientation['yaw'], 2)
    z = round(orientation['pitch'], 2)

    #return f"{round(orientation['roll'], 2)}, {round(orientation['pitch'], 2)}, {round(gyro['yaw'], 2)}"
    return f"{x}, {y}, {z}"

def getMotionControllerData(sense_hat: SenseHat) -> str:
    
    orientation = sense_hat.get_orientation_degrees()

    x = round(orientation['roll'], 2)
    y = round(orientation['pitch'], 2)
    z = round(orientation['yaw'], 2)

    #return f"{round(orientation['roll'], 2)}, {round(orientation['pitch'], 2)}, {round(gyro['yaw'], 2)}"
    return f"{x}, {y}, {z}"