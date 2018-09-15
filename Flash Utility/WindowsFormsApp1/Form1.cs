using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public byte Interface;
        public string _ipaddress;
        public ushort _TCPport;
        public String _serialport;
        public uint _SerialBaudrate;
        public uint _StartAddress;
        public string _FileName;

        public Form1()
        {
            InitializeComponent();
        }

        public void ChoiceBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChoiceBox.SelectedItem.Equals("TCP/IP"))
            {
                tcp.Show();
                txtB_ipaddress.Show();
                txtB_Port.Show();
                ble.Hide();
                serial.Hide();
                
            }
            else if (ChoiceBox.SelectedItem.Equals("UART"))
            {
                serial.Show();
                tcp.Hide();
                ble.Hide();
                GetAvailablePorts();
            }
            else if (ChoiceBox.SelectedItem.Equals("BLE"))
            {
                ble.Show();
                tcp.Hide();
                serial.Hide();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dlgResult = MessageBox.Show("Launch download procedure ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlgResult == DialogResult.Yes)
            {
                frmDownload wDownload = new frmDownload();
                
                wDownload.strFilename = FileName.Text;
                wDownload.u8Interface = ChoiceBox.SelectedItem.ToString();
                wDownload.u32AddrStart = 0x80008000;
                if (ChoiceBox.SelectedItem.ToString()=="TCP/IP")
                {
                    wDownload.strIPAddr = txtB_ipaddress.Text;
                    wDownload.tcpPort = Convert.ToUInt16(txtB_Port.Text);
                }
                else if (ChoiceBox.SelectedItem.ToString() == "UART")
                {
                    wDownload.serialPort = comboBox1.Text;
                    wDownload.SerialBaudrate = Convert.ToUInt32(comboBox2.Text);
                }
                else if(ChoiceBox.SelectedItem.ToString() == "BLE")
                {

                }
                
                wDownload.ShowDialog();
             
            }
            else
            {
            }





        }

        private void button2_Click(object sender, EventArgs e)
        {
            
           
            if (openFileDLG.ShowDialog()==DialogResult.OK)
            {
                           
                FileName.Text = openFileDLG.FileName;
            }
        }

       

        
        
        private void GetAvailablePorts()
        {
            String[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
        }
    }
}
