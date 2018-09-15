using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
namespace WindowsFormsApp1
{
    class SerialCommunication : ICommunication
    {
        private SerialPort SerialPort;
        public UInt32 BaudRate;
        public string portname;

        public int Connect()
        {
            SerialPort = new SerialPort();
            if (SerialPort.IsOpen == true)
            {
                SerialPort.Close();
            }
          
             
                SerialPort.PortName = portname;
                SerialPort.BaudRate = (int)BaudRate;
                SerialPort.Open();
                SerialPort.DiscardInBuffer();
                SerialPort.DiscardOutBuffer();
            
            return 0;

        }

        public void Disconnect()
        {
           
            SerialPort.Close();

        }

       
        public void Receive(byte[] data, int offset, int count)
        {
            var bs = SerialPort.BaseStream;
            int br = 0;
            while (br < count)
            {
                br += bs.Read(data, offset + br, count - br);
            }

        }

        public void Send(byte[] data, int offset, int count)
        {
            // var bs = SerialPort.BaseStream;
            // SerialPort.Write(data, offset, count);  
            
            SerialPort.Write(data, offset, count);
        }
    }
}
