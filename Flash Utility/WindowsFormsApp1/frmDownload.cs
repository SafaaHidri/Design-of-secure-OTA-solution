using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using WindowsFormsApp1;

namespace WindowsFormsApp1
{
	public partial class frmDownload : Form
	{

		public string strFilename;
		public uint u32AddrStart = 0;

        public string u8Interface;
		public string serialPort;
		public UInt32 SerialBaudrate = 0;
		public ushort tcpPort = 0;
		public string strIPAddr = "0.0.0.0";
        private BackgroundWorker backgndWorker;
        private ImageList imageList1;
        private IContainer components;


		private BootLogic BootLogic = new BootLogic();

		public frmDownload()
		{
			InitializeComponent();
		}

		private void vAddEventLog(DataGridView FusedView, byte Fu8Type, string FstrMessage)
		{
            FusedView.Rows.Add(imageList1.Images[Fu8Type], DateTime.Now.ToString("HH:mm:ss tt"), FstrMessage);
			FusedView.Rows[gridStatus.Rows.Count - 1].Selected = true;
			FusedView.FirstDisplayedScrollingRowIndex = (gridStatus.Rows.Count - 1);
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			BootLogic.s32AbortDownload();
            this.gridStatus.Rows.Clear();
            this.gridStatus.Refresh();
			Close();
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			// Abort Download
			//
			BootLogic.s32AbortDownload();
		}

        private string strState(ushort Fu16State)
        {
            string strRet ="..";
            switch (Fu16State)
            {
                case 0:
                    strRet = "Download init";
                    break;
                case 1:
                    strRet = "Connecting...";
                    break;
                case 2:
                    strRet = "Connected.";
                    break;
                case 3:
                    strRet = "Hooking Up ...";
                    break;
                case 4:
                    strRet = "Hooked Up .";
                    break;
                case 5:
                    strRet = "Erase Started...";
                    break;
                case 6:
                    strRet = "erase Finished.";
                    break;
                case 7:
                    strRet = "Writing Started...";
                    break;
                case 8:
                    strRet = "Writing started at" + String.Format("0x{0:X}", u32CurrentAddr);
                    break;
                case 9:
                    strRet = "Writing Finished.";
                    break;
                case 10:
                    strRet = "Checking Started...";
                    break;
                case 11:
                    strRet = "Check Finished.";
                    break;
                case 12:
                    strRet = "Jump To the Application";
                    break;
                case 13:
                    strRet = "Diconnect";
                    break;
                case 14:
                    strRet = "Download Stopped.";
                    break;
                case 15:
                    strRet = "Error download...";
                    break;
                case 18:
                    strRet = "Sucess download...";
                    break;
                case 19:
                    strRet = "...";
                    break;
                case 20:
                    strRet = "...";
                    break;
                default:
                    strRet = "Unknown state";
                    break;
            }

            return (strRet);
        }

        static private uint u32CurrentAddr = 0;
        static private int TotalSize = 0;
        static private uint u32Prog = 0;
        static private DateTime dtStartTime;
        static private DateTime dtEventNow;
        double dbRate;

        private void dwnlWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            uint state = (UInt32)e.UserState;
            uint u32totalProgrammed = u32Prog;
            int u32TotalSize = TotalSize;
            int tmpValue = 0;

            DateTime dtTemp;
            TimeSpan tsDiff;

            switch (state)
            {
                case 0:
                        vAddEventLog(this.gridStatus, 0, strState((ushort)e.ProgressPercentage));
                    break;
                case 1:
                    if (u32totalProgrammed <= u32TotalSize)
                    {
                        //Update download rate
                        dtTemp = dtEventNow;//dtNow = DateTime.Now;
                        tsDiff = dtTemp.Subtract(dtStartTime);
                        dbRate = 1000.0 * ((double)u32Prog) / ((double)(1024 * tsDiff.TotalMilliseconds));
                        this.progressBar.Value = (int)(u32totalProgrammed * 100 / u32TotalSize);
                        this.labelDown.Text = this.progressBar.Value.ToString() + "% " + dbRate.ToString("F") + "kib/s";
                    }
                    else
                    {
                        this.progressBar.Value = 100;
                        this.labelDown.Text = this.progressBar.Value.ToString() + "%" + dbRate.ToString("F") + "kib/s";
                    }
                    break;
                case 2:
                    vAddEventLog(this.gridStatus, 0, "Application timeout ...");
                    break;
                default:
                    break;
            }

        }


        private void dwnlWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.BootLogic.IsFlashInProgress == 1)
            {
                vAddEventLog(this.gridStatus, 2, "Download error.");
                btnStop.Enabled = false;
                btnClose.Enabled = true;
            }
            else if (this.BootLogic.IsFlashInProgress == 2)
            {
                vAddEventLog(this.gridStatus, 1, "Success.");
                this.progressBar.Value = 100;
                this.labelDown.Text = this.progressBar.Value.ToString() + "%   " + dbRate.ToString("F") + "kib/s";
                btnStop.Enabled = false;
                btnClose.Enabled = true;
            }
            else if (this.BootLogic.IsFlashInProgress == 3)
            {
                vAddEventLog(this.gridStatus, 2, "Stopped.");
                btnStop.Enabled = false;
                btnClose.Enabled = true;
            }

        }

		private void frmDownload_Load(object sender, EventArgs e)
		{
			ProcessStartInfo startInfo = new ProcessStartInfo();
			Process p = new Process();
			uint u32TmpAddrStart;
			uint u32TmpAddrEnd;
            Int16 s16retunedVal = 0;

			// Initialize Download Application
			//
			labelDown.Text = "0%";
			btnStop.Enabled = false;
			btnClose.Enabled = true;
            

			BootLogic.Interface       = u8Interface;
			BootLogic._serialportname     = serialPort;
			BootLogic._SerialBaudrate = SerialBaudrate;
			BootLogic._TCPport        = tcpPort;
			BootLogic._ipaddress      = strIPAddr;
			BootLogic._StartAddress   = u32AddrStart;
			BootLogic._FileName       = strFilename;
			
			BootLogic._u32Capturetimeout = 30000; // 30 sec timeout
			
			if (BootLogic.Start() != 0)
			{
                vAddEventLog(this.gridStatus, 2, "Error on starting download...");
			}
			else
			{
                dwnlWorker.RunWorkerAsync();
                btnStop.Enabled = true;
                btnClose.Enabled = false;
			}
	
			// Prepare downloading
			//
		}



        private void dwnlWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            uint state = 0;
            uint oldstate = 0xFF;
            WindowsFormsApp1.BootLogic.stEventState evState;


            while (this.BootLogic.IsFlashInProgress == 0)
            {
                if (this.BootLogic.handleUpdate.WaitOne())
                {
                    evState = (WindowsFormsApp1.BootLogic.stEventState)this.BootLogic.queueStatusSync.Dequeue();
                    state = (uint)evState.s32DLState;
                    u32CurrentAddr = evState.u32CurrentAddr;
                    u32Prog = evState.u32Prog;
                    TotalSize = evState.totalsize;
                    dtEventNow = evState.dtTimeStamp;

                    if (state != oldstate)
                    {
                        if ((state == 17))
                        {
                            vAddEventLog(this.gridStatus, 2, "Download error.");
                            btnStop.Enabled = false;
                            btnClose.Enabled = true;
                        }
                        else if ((state == 18))
                        {
                            vAddEventLog(this.gridStatus, 1, "Success.");
                            this.progressBar.Value = 100;
                            this.labelDown.Text = this.progressBar.Value.ToString() + "%   " + dbRate.ToString("F") + "kib/s";
                            btnStop.Enabled = false;
                            btnClose.Enabled = true;
                        }
                        else if ((state == 14))
                        {
                            vAddEventLog(this.gridStatus, 2, "Stopped.");
                            btnStop.Enabled = false;
                            btnClose.Enabled = true;
                        }
                        else
                        {
                            dtStartTime = evState.dtTimeStamp; //Store data transfert start time
                            oldstate = state;
                            this.dwnlWorker.ReportProgress((int)state, (UInt32)0); // State Change
                        }
                    }
                    else if ((state == 8))
                    {
                        this.dwnlWorker.ReportProgress(0, (UInt32)1);
                    }
                    else
                    {
                        this.dwnlWorker.ReportProgress((int)state, (UInt32)0); // State Change 
                    }
                }
            }
            
        }



    }
}
