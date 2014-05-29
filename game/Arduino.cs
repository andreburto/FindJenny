using System;
using System.IO.Ports;
using System.Text;

namespace Arduino
{
    class Arduino
    {
        private string _comPort = "";
        private int _baudRate = 9600;
        private int _timeOut = 500;
        private string _test = "";
        private string _confirmation = "";

        public string Comport { get { return _comPort; } set { _comPort = value; } }
        public int Baudrate { get { return _baudRate; } set { _baudRate = value; } }
        public int Timeout { get { return _timeOut; } set { _timeOut = value; } }
        public string Test { get { return _test; } set { _test = value; } }
        public string Confirmation { get { return _confirmation; } set { _confirmation = value; } }

        public SerialPort New()
        {
            if (CheckForArduino() == true)
            {
                return new SerialPort(_comPort, _baudRate, Parity.None, 8, StopBits.One);
            }
            else
            {
                throw new Exception("No Arduino plugged in.");
            }
        }

        public bool CheckForArduino()
        {
            string[] sp = SerialPort.GetPortNames();
            foreach (string s in sp)
            {
                string readback = "";

                SerialPort temp = new SerialPort(s, _baudRate, Parity.None, 8, StopBits.One);
                temp.ReadTimeout = _timeOut;
                temp.Open();
                temp.Write(_test);
                readback = temp.ReadLine();
                temp.Close();
                temp.Dispose();

                if (readback.Substring(0, _confirmation.Length) == _confirmation)
                {
                    _comPort = s;
                    return true;
                }
            }
            return false;
        }

        public Arduino() { }
        public Arduino(string t, string c) { _test = t; _confirmation = c; }
    }
}
