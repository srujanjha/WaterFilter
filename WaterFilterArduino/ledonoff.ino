const int ledPin = 2;
  byte serialA;
void setup()
{
  Serial.begin(19200);
  pinMode(ledPin, OUTPUT);
}

void loop() {

if (Serial.available() > 0) {serialA = Serial.read();Serial.println(serialA);}

   
      switch (serialA) {
    case 6:
      digitalWrite(ledPin, HIGH);
      break;
    case 24:
      digitalWrite(ledPin, LOW);
      break;
    case 30:digitalWrite(ledPin, HIGH);
      delay(100);
      digitalWrite(ledPin, LOW);
      delay(100);
     default:

      break;
  }

}
