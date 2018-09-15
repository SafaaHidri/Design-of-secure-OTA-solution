using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;
using System.ComponentModel;
using System.IO;
using System.Collections;

namespace WindowsFormsApp1
{  
    public class BootLogic
    {
        public string Interface ;
        public string _ipaddress;
        public ushort _TCPport;
        public  String _serialportname;
        public UInt32 _SerialBaudrate ;
        public uint _StartAddress;
        public string _FileName;
        public UInt32  _u32Capturetimeout;
        private ICommunication ComObjet;
        private double _flashedBytes;
        private Thread oThread;
        private Action[,] _stateAction;
        private long _flashSize;
        private long FlashedBytes;
        public UInt32 IsFlashInProgress;
        private Int32 startAddress = 0x08008000;
        /// <summary>
        /// Status event queue
        /// </summary>
        private Queue usedQueue = new Queue();
        public Queue queueStatusSync;
        /// <summary>
        /// Handle to signal a transition
        /// </summary>
        public EventWaitHandle handleUpdate = new AutoResetEvent(false);

        /// <summary>
        /// Defines the states of the state machine
        /// </summary>
        private enum ProcessState
        {
            Hookup,
            Erase,
            Write,
            Check,
            Jump,
            Disconnect_Sucess,
            Disconnect_Failure,
        }


        public enum Command
        {
            Next_Sucess,   // Move to next state with success result
            Next_Fail,   // Move to the next state with failed result
        }


        private ProcessState _currentState;

        private Command _command;

        private enum TargetCommands
        {
            Hook = 0x46,
            Erase = 0x45,
            Write = 0x57,
            Check = 0x43,
            Jump = 0x4A,
        };

        private enum bootlogicState
        {
            Init = 0,
            Connect,
            Connected,
            Hook,
            Hookend,
            Erasestarted,
            Eraseend,
            writestart,
            writing,
            writeend,
            checkstart,
            checkend,
            jump,
            disconnected,
            stoped ,
            error,
            sucess
            
        };

        private enum TargetResponse
        {
            ACK = 0x06,
            NACK = 0x16
        };

        public struct stEventState
        {
            public int s32DLState;
            public uint u32CurrentAddr;
            public uint u32Prog;
            public DateTime dtTimeStamp;
            public int totalsize;
        }

        private void vUpdateStatus(int State)
        {
            stEventState evState = new stEventState();

            evState.s32DLState = State;
            evState.u32CurrentAddr = (uint)startAddress;
            evState.u32Prog = (uint)FlashedBytes;
            evState.dtTimeStamp = DateTime.Now; //Store data transfert start time
            evState.totalsize = (int)_flashSize;
            queueStatusSync.Enqueue(evState);
            handleUpdate.Set();
            Thread.Sleep(100);
        }
        /// <summary>
        /// Download Abort
        /// </summary>
        /// <returns>Returns -1 if error is occured</returns>
        public short s32AbortDownload()
        {
            if (0 == IsFlashInProgress)
            {
                IsFlashInProgress = 3;
                // A download in progress
                //
               _command = Command.Next_Fail;
               vUpdateStatus((int)bootlogicState.stoped);
                Thread.Sleep(500);
                try
                {
                    ComObjet.Disconnect();
                    Thread.Sleep(500);
                }
                catch { }
            }
            return (0);
        }
        

        public int Start()
        {

            IsFlashInProgress = 0;      
            queueStatusSync = Queue.Synchronized(usedQueue);
            vUpdateStatus((int)bootlogicState.Init);

            if(Interface.Equals("TCP/IP"))
            {
                TCP_Communication cominterface = new TCP_Communication();
                cominterface.ipaddress = _ipaddress;
                cominterface.port = _TCPport;
                ComObjet = cominterface;
            }
            else if(Interface.Equals("UART"))
            {
                SerialCommunication cominterface = new SerialCommunication();
                cominterface.BaudRate = _SerialBaudrate;
                cominterface.portname = _serialportname;
                ComObjet = cominterface;
            }
            else
            {

            }

            vUpdateStatus((int)bootlogicState.Connect);
            if (ComObjet.Connect() != 0)
            {
                return -1;
            }
            vUpdateStatus((int)bootlogicState.Connected);
            oThread = new Thread(new ThreadStart(Bootlogic_Thread));
            oThread.Start();

            _stateAction = new Action[6, 2]
           {
                //Next Success, Next Fail
               
                { Erase, TargetDisconnectFailure},  // Hookup State
                { Write, TargetDisconnectFailure},  // Erase State
                { Check, TargetDisconnectFailure},  // Write State
                { Jump, TargetDisconnectFailure},  // Check State
                { TargetDisconnectSuccess, TargetDisconnectFailure }, // Jump state
                { null, null}, // Target Disconnect Failure State
           };
            return 0;

        }
        private void Bootlogic_Thread()
        {  
                Hookup();
                while (IsFlashInProgress == 0)
                {
                    ExecuteState(_command);
                }

           

        }
        private void Hookup()
        {
          _currentState = ProcessState.Hookup;
          vUpdateStatus((int)bootlogicState.Hook);

            byte[] tx = new byte[2];
            byte[] tmp = new byte[2];

            // TODO: Send reset command

            // Wait for ACK from target device
            tx[0] = (byte)TargetCommands.Hook;
            tx[1] = 0x01;
            ComObjet.Send(tx,0,2);

            // Wait for ACK or NACK
            ComObjet.Receive(tmp,0,2);

            if (tmp[0] != (byte)TargetResponse.ACK)
            {
                _command = Command.Next_Fail;
            }
            else
            {
                _command = Command.Next_Sucess;
            }
            vUpdateStatus((int)bootlogicState.Hookend);
        }

        private void Erase()
        {
          _currentState = ProcessState.Erase;
          vUpdateStatus((int)bootlogicState.Erasestarted);

            byte[] tx1 = new byte[4];
            byte[] tmp1 = new byte[2];

            // Send the Erase command
            tx1[0] = (byte)TargetCommands.Erase;
            tx1[1] = 6;                              // Erase 6 sectors (Sector 2 - 7)
            tx1[2] =  (byte)0;                       // Initial sector to begin erase
            tx1[3] =  (byte)0;

            ComObjet.Send(tx1,0,4);

            // Wait for ACK or NACK
            ComObjet.Receive(tmp1,0,2);

            if (tmp1[0] != (byte)TargetResponse.ACK)
            {
                // Invalid ACK received
                _command = Command.Next_Fail;
            }
            else
            {
                _command = Command.Next_Sucess;

            }
            vUpdateStatus((int)bootlogicState.Eraseend);
        }


        // Write to the target device
        private void Write()
        {
            _currentState = ProcessState.Write;
            vUpdateStatus((int)bootlogicState.writestart);

            byte WriteFrameOffset = 8;
            byte WriteDataSize = 64;
            byte TotalNbrWriteFrame = 0;
            byte WriteSeqNbr = 0;
            int totalBytesFlashed = 0;     // the total number of bytes flashed to the target
            byte[] tx = new byte[WriteFrameOffset + WriteDataSize];
            byte[] tmp = new byte[2];

            // Read bin file
            byte[] bin = ReadFile();
            int totalBytes = bin.Length;   // the total number of bytes to flash           
            _flashSize = bin.Length;    //progress needed
            TotalNbrWriteFrame = (byte)(totalBytes / WriteDataSize);

            while (totalBytesFlashed < totalBytes)
            {
                
                // Send the Write command
                tx[0] = (byte)TargetCommands.Write;
                tx[1] = (byte)TotalNbrWriteFrame;
                WriteSeqNbr++;
                tx[2] = (byte)WriteSeqNbr;
                byte[] startAddressByte = BitConverter.GetBytes(startAddress);
                startAddressByte.CopyTo(tx, 3);
                tx[7] = WriteDataSize;
               

                // Will be sending WriteDataSize bytes of data at a time
                if (totalBytes - totalBytesFlashed >= WriteDataSize)
                {
                    for (int i = 0; i < WriteDataSize; i++)
                    {
                        tx[i + WriteFrameOffset] = bin[i + totalBytesFlashed];
                    }
                }
                else
                {
                    for (int i = 0; i < totalBytes - totalBytesFlashed; i++)
                    {
                        tx[i + WriteFrameOffset] = bin[i + totalBytesFlashed];
                    }

                    for (int i = WriteFrameOffset + totalBytes - totalBytesFlashed; i < WriteDataSize; i++)
                    {
                        tx[i] = 0xFF;
                    }
                }

                vUpdateStatus((int)bootlogicState.writing);

                ComObjet.Send(tx,0, WriteFrameOffset + WriteDataSize);
                // Wait for ACK or NACK
                ComObjet.Receive(tmp,0,2);
                if (tmp[0] != (byte)TargetResponse.ACK)
                {
                    // Invalid ACK received
                    // Write was not successful
                    _command = Command.Next_Fail;
                    return;
                }
                else
                {
                    // Successful: update the starting address and the totalbytesflashed
                    startAddress += WriteDataSize;
                    totalBytesFlashed += WriteDataSize;
                    FlashedBytes = totalBytesFlashed;

                }
               
            }

            vUpdateStatus((int)bootlogicState.writeend);
           _command = Command.Next_Sucess;

        }

        private void Check()
        {
            Int32 startAddress = 0x08008000;

            byte[] tx = new byte[12];
            byte[] tmp = new byte[2];

            _currentState = ProcessState.Check;
            vUpdateStatus((int)bootlogicState.checkstart);
 
            // Send the Write command
            tx[0] = (byte)TargetCommands.Check;

            byte[] startAddressByte = BitConverter.GetBytes(startAddress);
            startAddressByte.CopyTo(tx, 1);

            byte[] bin = ReadFile();
            Int32 endAddress = startAddress + bin.Length;
            //Send start address and checksum
            byte[] endAddressByte = BitConverter.GetBytes(endAddress);
            endAddressByte.CopyTo(tx, 5);

            //Hash value caclulated or the signature  size will be updated later 
            tx[9] = 0x25;
            tx[10] = 0x25;
            tx[11] = 0x25;
            //tx[12] = 0x25;
            ComObjet.Send(tx,0,12);

            // Wait for ACK or NACK
            ComObjet.Receive(tmp,0,2);
            if (tmp[0] != (byte)TargetResponse.ACK)
            {
                // Invalid ACK received
                _command = Command.Next_Fail;
            }
            else
            {
                _command = Command.Next_Sucess;
            }
            vUpdateStatus((int)bootlogicState.checkend);    
        }

        private  void Jump()
        {
            byte[] tx = new byte[2];

            _currentState = ProcessState.Jump;
            vUpdateStatus((int)bootlogicState.jump);

            // Send the Erase command
            tx[0] = (byte)TargetCommands.Jump;
            ComObjet.Send(tx,0,2);
           // Logger.Log("Jump successful!");
            _command = Command.Next_Sucess;
        }
        private  void TargetDisconnectFailure()
        {
            _currentState = ProcessState.Disconnect_Failure;
            vUpdateStatus((int)bootlogicState.disconnected);
            //TargetDisconnect();
            IsFlashInProgress = 1;
            vUpdateStatus((int)bootlogicState.error);
            Thread.Sleep(500);
        }
        private void TargetDisconnectSuccess()
        {
            _currentState = ProcessState.Disconnect_Sucess;
            vUpdateStatus((int)bootlogicState.disconnected);
            //TargetDisconnect();
            IsFlashInProgress = 2;
            vUpdateStatus((int)bootlogicState.sucess);
        }

        private byte[] ReadFile()
        {
            byte[] bin;
            using (var s = new FileStream(_FileName, FileMode.Open, FileAccess.Read))
            {
                /* allocate memory */
                bin = new byte[s.Length];

                /* read file contents */
                s.Read(bin, 0, bin.Length);
            }

            return bin;
        }
    
        private void ExecuteState(Command command)
        {
            _stateAction[(int)_currentState, (int)command].Invoke();
        }


    }
}
