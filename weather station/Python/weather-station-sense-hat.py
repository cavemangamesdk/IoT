import paho.mqtt.client as mqtt
import threading, time, keyboard, datetime, json, uuid
import paho.mqtt.client as paho
import get_data as getData
from sense_hat import SenseHat
from typing import Callable, Any

host = "3c6ea0ec32f6404db6fd0439b0d000ce.s2.eu.hivemq.cloud"
port = 8883
username = "mvp2023"
password = "wzq6h2hm%WLaMh$KYXj5"
client_id = ""
protocol = mqtt.MQTTv5
tls = mqtt.ssl.PROTOCOL_TLS

# Global flag variable to indicate when the threads should stop
running = True

clients = []
threads = []

# 
topics = {
    "humidity":      "sensehat/env/humidity",
    "pressure":      "sensehat/env/pressure",
    "accelerometer": "sensehat/imu/accelerometer",
    "gyroscope":     "sensehat/imu/gyroscope",
    "magnetometer":  "sensehat/imu/magnetometer",
    "orientation":   "sensehat/imu/orientation"
}

#
def on_connect(client, userdata, flags, rc, properties=None):
    print("CONNACK received with code %s." % rc)

def on_disconnect(client, userdata, rc, properties=None):
    print("DISCONNECT received with code %s." % rc)

def on_publish(client, userdata, mid):
    print("Message published")

def on_message(client, userdata, msg):
    print(f"Received message: {msg.topic} {str(msg.payload)}")

#
def init_client(client_id, protocol, host, port, username, password, tls):
    client = mqtt.Client(client_id, userdata=None, protocol=protocol)
    if tls:
        client.tls_set(tls_version=tls)
    if username and password:
        client.username_pw_set(username, password)
    client.on_connect = on_connect
    client.on_disconnect = on_disconnect
    client.on_publish = on_publish
    client.on_message = on_message
    client.connect_async(host, port)
    clients.append(client)
    return client

def init_thread(target: object, client: mqtt.Client, topic: str, interval: float, function: Callable[[Any], Any], sense, uuidDevice, dateTime) -> threading.Thread:
    thread = threading.Thread(target=target, args=(client, topic, interval, function, sense, uuidDevice, dateTime))
    threads.append(thread)
    return thread

#
def publish(client: mqtt.Client, topic: str, interval: float, function: Callable[[Any], Any], sense, uuidDevice, dateTime):
    while running:
        data = json.dumps(function(sense, uuidDevice, dateTime).__dict__)
        client.publish(topic=topic, payload=data, qos=2)
        time.sleep(interval)

def stop_program(_=None):
    global running
    running = False

#
sense = SenseHat()
uuidDevice = uuid.uuid4()
dateTime = datetime

for i in range(2):
    client = init_client(f"client_{i}", protocol, host, port, username, password, tls)
    print(f"Client created with name: client_{i}")

for client in clients:
    client.loop_start()

#
init_thread(publish, clients[0], topics["humidity"], 10.0, getData.getHumidityData, sense, uuidDevice, dateTime)
init_thread(publish, clients[1], topics["pressure"], 10.0, getData.getPressureData, sense, uuidDevice, dateTime)
# init_thread(publish, clients[2], topics["accelerometer"], 4.0, getData.getAccelerometerData, sense, uuidDevice, dateTime)
# init_thread(publish, clients[3], topics["gyroscope"], 3.0, getData.getGyroscopeData, sense, uuidDevice, dateTime)
# init_thread(publish, clients[4], topics["magnetometer"], 2.0, getData.getMagnetometerData, sense, uuidDevice, dateTime)
# init_thread(publish, clients[5], topics["orientation"], 1.0, getData.getOrientationData, sense, uuidDevice, dateTime)


# Start the publishing threads
for thread in threads:
    thread.start()

# Register the escape key press event handler
keyboard.on_press_key("esc", stop_program)

# Wait for the threads to finish
for thread in threads:
    thread.join()

# Stop the MQTT client loops
for client in clients:
    client.loop_stop()

# Disconnect from the MQTT broker
for client in clients:
    client.disconnect()

# Exit the program
exit(0)
