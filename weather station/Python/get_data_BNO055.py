#
import board # pip3 install adafruit-blinka
import adafruit_bno055 # pip3 install adafruit-circuitpython-bno055 (NOT adafruit-bno055)
import datetime
import uuid

#
from DataClass_BNO055.Base import Base
from DataClass_BNO055.Quaternion import Quaternion
from DataClass_BNO055.Temperature import Temperature
from DataClass_BNO055.Vector3 import Vector3

#
i2c = board.I2C()
sensor = adafruit_bno055.BNO055_I2C(i2c, address=0x29)

# Configuration

# Turn on external clock crystal
sensor._write_register(0x3F, 0x01)
# Read from register to check if external crystal is enabled
external_crystal_enabled = sensor._read_register(0x3F)
print("external crystal: ", bool(external_crystal_enabled & 0x01))


# Acceleration
def getAccelerometerDataBase(sensor: adafruit_bno055.BNO055_I2C) -> Vector3:
    
    accel = sensor.acceleration

    return Vector3(
        x = accel[0],
        y = accel[1],
        z = accel[2]
    )

def getAccelerometerData(sensor: adafruit_bno055.BNO055_I2C, session_id: uuid.UUID, datetime: datetime) -> Base:
    
    return Base(
        session_id = str(session_id),
        timestamp = str(datetime.datetime.now()),
        data = getAccelerometerDataBase(sensor).__dict__
    )

# Gyroscope
def getGyroscopeDataBase(sensor: adafruit_bno055.BNO055_I2C) -> Vector3:
    
    gyro = sensor.gyro

    return Vector3(
        x = gyro[0],
        y = gyro[1],
        z = gyro[2]
    )

def getGyroscopeData(sensor: adafruit_bno055.BNO055_I2C, session_id: uuid.UUID, datetime: datetime) -> Base:
    
    return Base(
        session_id = str(session_id),
        timestamp = str(datetime.datetime.now()),
        data = getGyroscopeDataBase(sensor).__dict__
    )

# Magnetometer
def getMagnetometerDataBase(sensor: adafruit_bno055.BNO055_I2C) -> Vector3:
    
    mag = sensor.magnetic

    return Vector3(
        x = mag[0],
        y = mag[1],
        z = mag[2]
    )

def getMagnetometerData(sensor: adafruit_bno055.BNO055_I2C, session_id: uuid.UUID, datetime: datetime) -> Base:
    
    return Base(
        session_id = str(session_id),
        timestamp = str(datetime.datetime.now()),
        data = getMagnetometerDataBase(sensor).__dict__
    )

# Euler
def getEulerDataBase(sensor: adafruit_bno055.BNO055_I2C) -> Vector3:
    
    euler = sensor.euler

    return Vector3(
        x = euler[0],
        y = euler[1],
        z = euler[2]
    )

def getEulerData(sensor: adafruit_bno055.BNO055_I2C, session_id: uuid.UUID, datetime: datetime) -> Base:
    
    return Base(
        session_id = str(session_id),
        timestamp = str(datetime.datetime.now()),
        data = getEulerDataBase(sensor).__dict__
    )

# Quaternion
def getQuaternionDataBase(sensor: adafruit_bno055.BNO055_I2C) -> Quaternion:
    
    quat = sensor.quaternion

    return Quaternion(
        w = quat[0],
        x = quat[1],
        y = quat[2],
        z = quat[3]
    )   

def getQuaternionData(sensor: adafruit_bno055.BNO055_I2C, session_id: uuid.UUID, datetime: datetime) -> Base:
    
    return Base(
        session_id = str(session_id),
        timestamp = str(datetime.datetime.now()),
        data = getQuaternionDataBase(sensor).__dict__
    )

# Linear Acceleration
def getLinearAccelerationDataBase(sensor: adafruit_bno055.BNO055_I2C) -> Vector3:
    
    linear_accel = sensor.linear_acceleration

    return Vector3(
        x = linear_accel[0],
        y = linear_accel[1],
        z = linear_accel[2]
    )

def getLinearAccelerationData(sensor: adafruit_bno055.BNO055_I2C, session_id: uuid.UUID, datetime: datetime) -> Base:
    
    return Base(
        session_id = str(session_id),
        timestamp = str(datetime.datetime.now()),
        data = getLinearAccelerationDataBase(sensor).__dict__
    )  

# Gravity
def getGravityDataBase(sensor: adafruit_bno055.BNO055_I2C) -> Vector3:
    
    gravity = sensor.gravity

    return Vector3(
        x = gravity[0],
        y = gravity[1],
        z = gravity[2]
    )

def getGravityData(sensor: adafruit_bno055.BNO055_I2C, session_id: uuid.UUID, datetime: datetime) -> Base:
    
    return Base(
        session_id = str(session_id),
        timestamp = str(datetime.datetime.now()),
        data = getGravityDataBase(sensor).__dict__
    )

# Temperature
def getTemperatureDataBase(sensor: adafruit_bno055.BNO055_I2C) -> Temperature:
    
    temp = sensor.temperature

    return Temperature(
        temp = temp
    )

def getTemperatureData(sensor: adafruit_bno055.BNO055_I2C, session_id: uuid.UUID, datetime: datetime) -> Base:

    return Base(
        session_id = str(session_id),
        timestamp = str(datetime.datetime.now()),
        data = getTemperatureDataBase(sensor).__dict__
    ) 

#
def getPitchRollData(sensor: adafruit_bno055.BNO055_I2C) -> str:
    
    ox, oy, oz = -sensor.euler[0], sensor.euler[1], sensor.euler[2]
    ax, ay, az = sensor.linear_acceleration[0], sensor.linear_acceleration[1], sensor.linear_acceleration[2]

    return f"{oy}, {ox}, {oz}, {ax}, {ay}, {az}"

def getMotionControllerData(sensor: adafruit_bno055.BNO055_I2C) -> str:
    
    x, y, z = sensor.euler[0], -sensor.euler[1], sensor.euler[2]
    
    return f"{x}, {y}, {z}"