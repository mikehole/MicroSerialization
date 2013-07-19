using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MicroSerialization.WIndowsPhone.TestApp.Resources;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using Windows.Networking.Proximity;
using MicroSerialization.Agent.TestApp.Model;
using System.IO;

namespace MicroSerialization.WIndowsPhone.TestApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        private StreamSocket _socket = null;
        private PeerInformation _peer = null;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private async Task<bool> SetupDeviceConn()
        {
            //Connect to your paired host PC using BT + StreamSocket (over RFCOMM)
            PeerFinder.AlternateIdentities["Bluetooth:PAIRED"] = "";

            var devices = await PeerFinder.FindAllPeersAsync();

            if (devices.Count == 0)
            {
                MessageBox.Show("No paired device");
                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:"));
                return false;
            }

            _peer = devices.FirstOrDefault(c => c.DisplayName.Contains(TxtDevice.Text));
            if (_peer == null)
            {
                MessageBox.Show("No paired device");
                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:"));
                return false;
            }

            _socket = new StreamSocket();

            //"{00001101-0000-1000-8000-00805f9b34fb}" - is the GUID for the serial port service.
            await _socket.ConnectAsync(_peer.HostName, "{00001101-0000-1000-8000-00805f9b34fb}");

            return true;
        }

        private async void CmdDoStuff_OnTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (await SetupDeviceConn())
            {
                CmdDoStuff.IsEnabled = false;
                CmdSendStuff.IsEnabled = true;
            }
        }

        private async void CmdSendStuff_OnTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //send the objet to the agent app with a trailing byte of zero st that the app knows it 
            //has all of the current data.

            var objectSender = new MicroSerialization.Pcl.ObjectSerializer<TestMessage>(WindowsRuntimeStreamExtensions.AsStreamForWrite(_socket.OutputStream));

            var bytesSent = objectSender.SaveToStream(new TestMessage() 
            { 
                TestInt = 1
                , TestString = TxtText.Text
            });

            txtBytes.Text = bytesSent.ToString();

        }
    }
}