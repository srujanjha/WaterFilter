using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterPurity
{
    class Serials
    {
        private SerialPort _serialPort;
        public Serials(string serial)
        {
            if (_serialPort == null) { InitSerialPort(serial); }
        }
        public SerialPort getSerialPort()
        {
            return _serialPort;
        }
        private void InitSerialPort(string serial)
        {
            try
            {
                _serialPort = new SerialPort(serial, 9600, Parity.None, 8, StopBits.One)
                {
                    Handshake = Handshake.None,
                    ReadTimeout = 1000,
                    WriteTimeout = 1000
                };
                _serialPort.Open();
                return;
            }
            catch { }
        }
    }
}
