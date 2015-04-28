/*
** This sample Arduino sketch uploads telemetry data to Azure Mobile Services
** See the full article here: http://hypernephelist.com/2014/07/11/arduino-uno-azure-mobile-services.html
**
** Thomas Conté @tomconte
*/
 
#include <SPI.h>
#include <Ethernet.h>
 int ledPins[] = {
  2, 3, 4, 5, 6, 7 };       // an array of pin numbers to which LEDs are attached
int pinCount = 6; 
// Ethernet shield MAC address (sticker in the back)
byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED };
 
// Azure Mobile Service address
// You can find this in your service dashboard
const char *server = "srujan.azure-mobile.net";
 
// Azure Mobile Service table name
// The name of the table you created
const char *table_name = "telemetry";

// Azure Mobile Service Application Key
// You can find this key in the 'Manage Keys' menu on the dashboard
const char *ams_key = "RikaPRJBXAZLqIFnkHHPhSEEJqVfQT95";
 
EthernetClient client;
 
char buffer[64];
 char ar[6];
/*
** Send an HTTP POST request to the Azure Mobile Service data API
*/
 boolean created=true;
void send_request()
{
  Serial.println("\nconnecting...");
 
  if (client.connect(server, 80)) {
 
    Serial.print("sending ");
 
    // POST URI
    sprintf(buffer, "POST /tables/%s HTTP/1.1", table_name);
    client.println(buffer);
 
    // Host header
    sprintf(buffer, "Host: %s", server);
    client.println(buffer);
 
    // Azure Mobile Services application key
    sprintf(buffer, "X-ZUMO-APPLICATION: %s", ams_key);
    client.println(buffer);
 
    // JSON content type
    client.println("Content-Type: application/json");
   
   if(created){
    // POST body
    for (int thisPin = 0; thisPin < pinCount; thisPin++) {
    int ch=random(0,2);
    ar[thisPin]=ch+48;
    } }
    sprintf(buffer, "{\"title\": \"%c%c%c%c%c%c\"}",ar[0],ar[1],ar[2],ar[3],ar[4],ar[5]);
    // Content length
    client.print("Content-Length: ");
    client.println(strlen(buffer));
 Serial.println(buffer);
    // End of headers
    client.println();
 
    // Request body
    client.println(buffer);
    
  } else {
    Serial.println("connection failed");
  }
}
 
/*
** Wait for response
*/
 
void wait_response()
{
  while (!client.available()) {
    if (!client.connected()) {
      return;
    }
  }
}
 
/*
** Read the response and dump to serial
*/
 
void read_response()
{
  bool print = true;
  Serial.println("Response");
  while (client.available()) {
    char c = client.read();
    // Print only until the first carriage return
    if (c == '\n')
      print = false;
    if (print)
     { Serial.print(c);if(c=='C')created=true;if(c=='B')created=false;}
  }
  Serial.println(created);
}
 
/*
** Close the connection
*/
 
void end_request()
{
  client.stop();
}
 
/*
** Arduino Setup
*/
 
void setup()
{
  Serial.begin(9600);
  while (!Serial) {
    ; // wait for serial port to connect.
  }
  Serial.println("ethernet");
 for (int thisPin = 0; thisPin < pinCount; thisPin++)
    pinMode(ledPins[thisPin], OUTPUT);      
  if (Ethernet.begin(mac) == 0) {
    Serial.println("ethernet failed");
    for (;;) ;
  }
  // give the Ethernet shield a second to initialize:
  delay(1000);
}
 
/*
** Arduino Loop
*/
 
void loop()
{
  
  send_request();
  wait_response();
  read_response();
  end_request();
  if(created)
  {
    Serial.print(ar[0]);
  Serial.print(ar[1]);
  Serial.print(ar[2]);
  Serial.print(ar[3]);
  Serial.print(ar[4]);
  Serial.print(ar[5]);
  
    for (int thisPin = 0; thisPin < pinCount; thisPin++) {
      
    if(ar[thisPin]-48)digitalWrite(ledPins[thisPin], HIGH);  
    else digitalWrite(ledPins[thisPin], LOW);
    } delay(10000);}
  
}
