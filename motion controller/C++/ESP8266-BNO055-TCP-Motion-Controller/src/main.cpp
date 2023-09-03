/*
    Motion controller based on the ESP8266 and the BNO055 sensor
    Version 4, with automatic game websocket server detection
*/

// Includes
#include <ESP8266WiFiMulti.h>
#include <Adafruit_BNO055.h>
#include <WebSocketsClient.h>
#include <WiFiUdp.h>

// Constants
const unsigned long serial_baudrate = 115200;

// SSID & password of the Wi-Fi network you want to connect to (will connect to strongest)
const char* ssid1     = "network 42";   
const char* password1 = "12345678"; 
const char* ssid2     = "4G Wi-Fi 3Danmark-1CBA";
const char* password2 = "aircraft";
const char* ssid3     = "Grundforlob";
const char* password3 = "DataitGF";

// Websocket server
//const char* ws_ip = "192.168.8.104";
//const char* ws_ip = "192.168.5.113";
const uint16_t ws_port = 80;
const String ws_path = "/MotionController";

// Websocket server IP multicast
const IPAddress multicastIP(239, 1, 1, 1);
const uint16_t multicastPort = 5432;

//
Adafruit_BNO055 bno = Adafruit_BNO055(55, 0x29);
ESP8266WiFiMulti wifiMulti;
WebSocketsClient webSocket;
WiFiUDP udp;

// Functions
void ConnectBMO055() {
  Serial.println("Setting up orientation sensor...");
  if (!bno.begin()) {
    Serial.println("BNO055 not detected... Check wiring and I2C address!");
    while (1);
  }
  Serial.println("BNO055 detected!");
}

void ConnectWifi() {

  wifiMulti.addAP(ssid1, password1);
  wifiMulti.addAP(ssid2, password2);
  wifiMulti.addAP(ssid3, password3);

  Serial.println("Connecting Wifi");
  
  while (wifiMulti.run() != WL_CONNECTED) {
    delay(100);
    Serial.print('.');
  }
  Serial.println('\n');
  Serial.print("Connected to ");
  Serial.println(WiFi.SSID());
  Serial.print("IP address:\t");
  Serial.println(WiFi.localIP());
}

void webSocketEvent(WStype_t type, uint8_t * payload, size_t length) {
  // Boilerplate websocket event handler
  switch(type) {
    case WStype_DISCONNECTED:
      Serial.printf("[WSc] Disconnected!\n");
      break;
    case WStype_CONNECTED:
      Serial.printf("[WSc] Connected to url: %s\n", payload);
      break;
    case WStype_TEXT:
      Serial.printf("[WSc] get text: %s\n", payload);
      break;
    case WStype_BIN:
      Serial.printf("[WSc] get binary length: %u\n", length);
      break;
    case WStype_PING:
      Serial.printf("[WSc] get ping\n");
      break;
    case WStype_PONG:
      Serial.printf("[WSc] get pong\n");
      break;
    case WStype_ERROR:
      Serial.printf("[WSc] get error\n");
      break;
    case WStype_FRAGMENT_TEXT_START:
      Serial.printf("[WSc] get fragment text start\n");
      break;
    case WStype_FRAGMENT_BIN_START:
      Serial.printf("[WSc] get fragment bin start\n");
      break;
    case WStype_FRAGMENT:
      Serial.printf("[WSc] get fragment\n");
      break;
    case WStype_FRAGMENT_FIN:
      Serial.printf("[WSc] get fragment fin\n");
      break;
  }
}

String ConnectMulticast() {
  Serial.println("Attempting to start UDP multicast listener...");
  if (udp.beginMulticast(WiFi.localIP(), multicastIP, multicastPort)) {
      Serial.println("UDP multicast listener started");
  } else {
    Serial.println("UDP multicast listener failed to start");
    exit(1);
  }

  while(udp.parsePacket() <= 0) {
    int packetSize = udp.parsePacket();
    if (packetSize) {
      Serial.println(packetSize);
      char incomingPacket[255];
      int len = udp.read(incomingPacket, 255);
      if (len > 0) {
        incomingPacket[len] = 0;
      }
      Serial.printf("UDP packet contents: %s\n", incomingPacket);
      //ws_ip = incomingPacket;
      udp.stop();
      udp.flush();
      return String(incomingPacket);
    }
  }
  return String();
}

void ConnectWebSocket(String ws_ip) {
  Serial.print("Attempting to start websocket connection to IP " + ws_ip + "\n");
  webSocket.begin(ws_ip, ws_port, ws_path);
  webSocket.onEvent(webSocketEvent);
  webSocket.setReconnectInterval(1000);
  webSocket.enableHeartbeat(15000, 3000, 2);
}

void GetOrientationData() {
  sensors_event_t orientationData;
  bno.getEvent(&orientationData, Adafruit_BNO055::VECTOR_EULER);
  String oy = String( orientationData.orientation.y);
  String oz = String(-orientationData.orientation.z);
  String payload = oy + "," + oz;
  webSocket.sendTXT(payload);
  //Serial.println(message);
}

void setup() {
  Serial.begin(serial_baudrate);
  Serial.println("Setting up motion controller...");
  ConnectBMO055();
  ConnectWifi();
  String ip = ConnectMulticast();
  ConnectWebSocket(ip);
}

void loop() {
  webSocket.loop();
  GetOrientationData();
}