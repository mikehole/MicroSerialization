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

            _serial.Open();

            // initialize display buffer
            _display = new Bitmap(Bitmap.MaxWidth, Bitmap.MaxHeight);

            // sample "hello world" code
            _display.Clear();
            _display.DrawText("Ready :)", _fontNinaB, Color.White, 10, 64);
            _display.Flush();

            while (true)
            {
                if (_serial.BytesToRead > 0)
                {
                    GetAllBytes();
                }
            }
        }

        private static byte[] GetAllBytes()
        {
            byte[] mybuffer = new byte[1024];

            int bytesRead = 0;

            while (_serial.BytesToRead > 0)
            {
                byte[] chunk = new byte[_serial.BytesToRead];

                _serial.Read(chunk, 0, chunk.Length);

                foreach (var b in chunk)
                {
                    mybuffer[bytesRead] = b;
                    bytesRead++;
                }

                Thread.Sleep(500);
            }

            MicroSerialization.Mf.ObjectSerializer os = new MicroSerialization.Mf.ObjectSerializer();

            object objectRecieved = os.LoadFromBytes(mybuffer);

            switch (objectRecieved.GetType().Name)
            {
                case "TestMessage":
                    _display.Clear();

                    if (((Model.TestMessage)objectRecieved).TestString != null)
                        _display.DrawText( bytesRead.ToString() + " - " +  ((Model.TestMessage)objectRecieved).TestString, _fontNinaB, Color.White, 10, 64);
                    else
                        _display.DrawText(bytesRead.ToString(), _fontNinaB, Color.White, 10, 64);

                    _display.Flush();
                    break;
            }

            return mybuffer;
        }
    }
}
