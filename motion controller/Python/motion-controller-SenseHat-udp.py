import socket
from sense_hat import SenseHat
import time
import asyncio

UDP_IP = "192.168.0.100"  # Replace with the target IP Address
UDP_PORT = 5100  # Replace with the target port number
MESSAGE = "Hello, World!"

print("UDP target IP:", UDP_IP)
print("UDP target port:", UDP_PORT)
print("message:", MESSAGE)

# Create a UDP socket
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

sense = SenseHat()

# Send data
# sock.sendto(bytes(MESSAGE, "utf-8"), (UDP_IP, UDP_PORT))

def getPitchRollData(sense_hat: SenseHat) -> str:
            
    orientation = sense_hat.get_orientation_degrees()

    x = round(orientation['roll'], 2)
    z = round(orientation['pitch'], 2)

    if(x > 180):
        x = x - 360
    
    if(z > 180):
        z = z - 360

    #return f"{round(orientation['roll'], 2)}, {round(orientation['pitch'], 2)}, {round(gyro['yaw'], 2)}"
    return f"{-x},{z}"

def sendData():
    while True:
        
        MESSAGE = getPitchRollData(sense)
        sock.sendto(bytes(MESSAGE, "utf-8"), (UDP_IP, UDP_PORT))
        #print(MESSAGE)
        #time.sleep(0.01)

asyncio.run(sendData())

