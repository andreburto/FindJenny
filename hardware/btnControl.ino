int btnA = 5;
int btnB = 6;
int btnC = 7;
int led = 13;

int ins[3] = {5, 6, 7};

void setup() {
  pinMode(btnA, INPUT);
  pinMode(btnB, INPUT);
  pinMode(btnC, INPUT);
  pinMode(led, OUTPUT);
  digitalWrite(led, LOW);
  Serial.begin(9600);
}

void loop() {
  // Check for incoming bits that are looking for the controller
  if (Serial.available() > 0) {
    char c = Serial.read();
    if (c == 'a') {
      Serial.println("Yes!");
    }
    if (c == 'p') {
      delay(150);
    }
  }
  
  // The main loop
  int val = 0;
  for(int c = 0; c < 3; c++) {
    val = digitalRead(ins[c]);
    if (val == 1) {
      Serial.println(ins[c], DEC);
      val = 0;
    }
    
    // Small delay
    delay(50);
  }
}
