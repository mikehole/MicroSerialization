using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Media;
using System.Threading;
using System.IO.Ports;
using MicroSerialization.Mf;

namespace MicroSerialization.Agent.TestApp
{
    public class Program
    {
        static Font _fontNinaB = null;

        static Bitmap _display;

        static System.IO.Ports.SerialPort _serial = null;

        public static void Main()
        {
            _fontNinaB = Resources.GetFont(Resources.FontResources.NinaB);

            _serial = new SerialPort("COM1");

            _serial.ReadTimeout = 500;

            //Handle the DataReceived event when data is sent to the serial port.
            _serial.DataReceived += _serial_DataReceived;

            _serial.Open();

            // initialize display buffer
            _display = new Bitmap(Bitmap.MaxWidth, Bitmap.MaxHeight);

            // sample "hello world" code
            _display.Clear();
            _display.DrawText("Ready :)", _fontNinaB, Color.White, 10, 64);
            _display.Flush();

            // go to sleep; all further code should be timer-driven or event-driven
            Thread.Sleep(Timeout.Infinite);
        }

        static void _serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //Read the length bytes.
            byte[] dataLengthBytes = new byte[4];

            _serial.Read(dataLengthBytes, 0, dataLengthBytes.Length);

            var dataLength = BitConverter.ToInt32(dataLengthBytes, 0);

            byte[] buffer = new byte[dataLength];

            int bytesRead = 0;
          
            while (bytesRead != dataLength)
            {
                int readValue = _serial.ReadByte();

                buffer[bytesRead] = (byte) readValue;
                bytesRead++;
            }

            MicroSerialization.Mf.ObjectSerializer os = new MicroSerialization.Mf.ObjectSerializer();

            object objectRecieved = os.LoadFromBytes(buffer);

            switch (objectRecieved.GetType().Name)
            {
                case "TestMessage":
                    _display.Clear();
                    
                    if(((Model.TestMessage)objectRecieved).TestString != null)
                        _display.DrawText(((Model.TestMessage)objectRecieved).TestString, _fontNinaB, Color.White, 10, 64);

                    _display.Flush();
                    break;
            }
        }
    }
}
