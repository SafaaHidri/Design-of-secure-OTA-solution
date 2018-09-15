using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
namespace WindowsFormsApp1
{
    interface ICommunication
    {
        int Connect();
        void Disconnect();
        void Send (byte[] data, int offset, int count);
        void Receive(byte[] data, int offset, int count);
        
    }
}
