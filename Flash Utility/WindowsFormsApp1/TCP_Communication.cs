using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace WindowsFormsApp1
{
    class TCP_Communication : ICommunication
    {
       public String ipaddress;
       public int port;
       Socket server = new Socket(AddressFamily.InterNetwork,
                                   SocketType.Stream, ProtocolType.Tcp);
        public int Connect()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ipaddress),port);         
            try
            {
                server.Connect(ipep);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Unable to connect to server.");
                Console.WriteLine(e.ToString());
             
            }
            return 0;
           
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        
        public void Receive(byte[] data, int offset, int count)
        {
            server.Receive(data);
          
        }

        public void Send(byte[] data, int offset, int count)
        {

            server.Send(data);
            
        }
    }
}
