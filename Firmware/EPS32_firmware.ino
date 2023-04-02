/*
  ------------------------------------------- CONTEXT --------------------------------------------------
    Simple HTTP Server for esp32 waiting for a particular GET request to execute giveMeOnePepsi()
  This application was created at HACKAUBG 5.0 and is for testing pourposes of the interaction
  with our Mobile Application. The ESP board is controlling a vending machine mechanism used for a
  potential collaborative marketing campaign with our Challenger Platform ( a marketplace for challenges)
  When a specific challenge has been completed the mobile app will send us a GET request with a valuw "finished"
*/

#include <WiFi.h>

const char* ssid = "HUAWEI";
const char* password = "12345678";

const int DIR = 12;
const int STEP = 14;
const int  steps_per_rev = 200;

// Static IP address. The ESP sometimes changes its IP after restart.
IPAddress local_IP(192, 168, 1, 184);
// Gateway IP address
IPAddress gateway(192, 168, 1, 1);

IPAddress subnet(255, 255, 0, 0);
IPAddress primaryDNS(8, 8, 8, 8);
IPAddress secondaryDNS(8, 8, 4, 4);

// Set web server port number to 80
WiFiServer server(80);

// Variable to store the HTTP request
String header;

// Current time
unsigned long currentTime = millis();
// Previous time
unsigned long previousTime = 0;
// Define timeout time in milliseconds (example: 2000ms = 2s)
const long timeoutTime = 2000;

void setup() {
  Serial.begin(115200);

  pinMode(STEP, OUTPUT);
  pinMode(DIR, OUTPUT);

  // Setting the static campaign
  if (!WiFi.config(local_IP, gateway, subnet, primaryDNS, secondaryDNS)) {
    Serial.println("STA Failed to configure");
  }


  // Connect to Wi-Fi network with SSID and password
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  // Print local IP address and start web server
  Serial.println("");
  Serial.println("WiFi connected.");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
  server.begin();
}

void loop(){
  WiFiClient client = server.available();   // Listen for incoming clients

  if (client) {                             // If a new client connects,
    currentTime = millis();
    previousTime = currentTime;
    Serial.println("New Client.");
    String currentLine = "";
    while (client.connected() && currentTime - previousTime <= timeoutTime) {
      currentTime = millis();
      if (client.available()) {
        char c = client.read();
        Serial.write(c);
        header += c;
        if (c == '\n') {

          if (currentLine.length() == 0) {
            client.println("HTTP/1.1 200 OK");
            client.println("Content-type:text/html");
            client.println("Connection: close");
            client.println();

            // Checks if the received http request is having the keyword winning a prize
            if (header.indexOf("GET /finished") >= 0) {
              Serial.println("Give me one pepsi please!");
              giveMeOnePepsi();
            } else {
              Serial.println("No pepsi for you");
            }

            // Printing some stuf at the http server just to verify it is working without getting a pepsi
            client.println("<!DOCTYPE html><html>");
            client.println("<head><meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
            client.println("<link rel=\"icon\" href=\"data:,\">");

            client.println("<style>html { font-family: Helvetica; display: inline-block; margin: 0px auto; text-align: center;}");
            client.println(".button { background-color: #4CAF50; border: none; color: white; padding: 16px 40px;");
            client.println("text-decoration: none; font-size: 30px; margin: 2px; cursor: pointer;}");
            client.println(".button2 {background-color: #555555;}</style></head>");

            // Web Page Heading
            client.println("<body><h1>ESP32 Web Server</h1>");


            // The HTTP response ends with another blank line
            client.println();
            // Break out of the while loop
            break;
          } else { // if you got a newline, then clear currentLine
            currentLine = "";
          }
        } else if (c != '\r') {  // if you got anything else but a carriage return character,
          currentLine += c;      // add it to the end of the currentLine
        }
      }
    }
    // Clear the header variable
    header = "";
    // Close the connection
    client.stop();
    Serial.println("Client disconnected.");
    Serial.println("");
  }
}

// Function that allows us to rotate the stepper motor enough to recieve a pepsi
void giveMeOnePepsi(){
  digitalWrite(DIR, HIGH);
  Serial.println("Spinning Clockwise...");

  for(int i = 0; i<steps_per_rev; i++)
  {
    digitalWrite(STEP, HIGH);
    delayMicroseconds(2000);
    digitalWrite(STEP, LOW);
    delayMicroseconds(2000);
  }
  delay(1000);

  digitalWrite(DIR, LOW);
  Serial.println("Spinning Anti-Clockwise...");
}
