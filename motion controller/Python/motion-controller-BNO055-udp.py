import time
import socket
import board
import adafruit_bno055

# Constants
UDP_PORT = 5100
serial_baudrate = 115200
multicastIP = ('239.1.1.1', 5432)
ssid0 = "dlink-C134"
password0 = "_BossPanda25"
target_ip = "192.168.0.100"

# Initialize BNO055
i2c = board.I2C()
sensor = adafruit_bno055.BNO055_I2C(i2c)

# Initialize UDP
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

def ConnectBMO055():
    print("Setting up orientation sensor...")
    if not sensor.begin():
        print("BNO055 not detected... Check wiring and I2C address!")
        while True:
            pass
    print("BNO055 detected!")

def ConnectWifi():
    print("Connecting Wifi")
    # Assuming WiFi connection is handled by the OS

def GetOrientationData():
    orientationData = sensor.euler
    oy = str(orientationData[1])
    oz = str(-orientationData[2])
    payload = oy + "," + oz
    sock.sendto(payload.encode(), (target_ip, UDP_PORT))

def ConnectUdp(port):
    print("Setting up UDP client...")
    sock.bind(('', port))
    print("UDP server started on port ", port)

def setup():
    print("Setting up motion controller...")
    ConnectBMO055()
    ConnectWifi()
    ConnectUdp(UDP_PORT)

def loop():
    while True:
        GetOrientationData()
        time.sleep(0.1)  # to avoid overloading the CPU

if __name__ == "__main__":
    setup()
    loop()
