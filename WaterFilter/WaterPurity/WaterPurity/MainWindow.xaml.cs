using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WaterPurity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MobileServiceClient MobileService = new MobileServiceClient(
    "https://waterpurity.azure-mobile.net/",
    "OQKvyKMVopTsOpqmSvADpTDbMfzLNZ52"
);
        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer1.Tick += new EventHandler(dispatcherTimer1_Tick);
            GetAllPorts();
            btnBrowse.IsEnabled = false;
            btnMeasure.IsEnabled = false;
            prgBar.Maximum = 100;
            prgBar.Minimum = 0;
        }
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer dispatcherTimer1 = new System.Windows.Threading.DispatcherTimer();

        private int InitializeCOMPorts()
        {
            int serial = 1;
            try
            {
                _serialPort = new SerialPort(COMPortSelected, 9600, Parity.None, 8, StopBits.One)
                {
                    Handshake = Handshake.None,
                    ReadTimeout = 1000,
                    WriteTimeout = 500
                };
                _serialPort.Open();
                _serialPort.Close();
                _serialPort.Dispose();
                return serial;
            }
            catch { }
            return -1;
        }
        private void btnMeasure_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop(); dispatcherTimer.IsEnabled = false;
            dispatcherTimer1.Stop(); dispatcherTimer1.IsEnabled = false;
            _serialPort.Close();
            _serialPort.Dispose();
            btnMeasure.IsEnabled = false;
            btnStop.IsEnabled = true;
            txtValue.Text = "";
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            dispatcherTimer.IsEnabled = true;
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer1.IsEnabled = true;
            dispatcherTimer.Start();
            dispatcherTimer1.Start();
            new Thread(() =>
            {
                readFile();
            }).Start();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            int serial = InitializeCOMPorts();
            if (serial == -1)
            {
                System.Windows.MessageBox.Show("Device is not connected at " + COMPortSelected + " !!", "Error");
                btnBrowse.IsEnabled = false;
            }
            else { btnBrowse.IsEnabled = true; InitSerialPort(COMPortSelected); }
            btnMeasure.IsEnabled = false;
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            ReceiveData();
        }
        int ch = 0;
        private void dispatcherTimer1_Tick(object sender, EventArgs e)
        {
            ch = (ch % 10) + 1;
            prgBar.Value = ch * 10;
        }
        private SerialPort _serialPort;
        public void GetAllPorts()
        {
            List<String> allPorts = new List<String>();
            foreach (String portName in System.IO.Ports.SerialPort.GetPortNames())
            {
                allPorts.Add(portName);
            }
            COMPort.ItemsSource = allPorts;
        }
        private void InitSerialPort(string serial)
        {
            Serials ob = new Serials(serial);
            _serialPort = ob.getSerialPort();
        }
        private void ReceiveData()
        {
            string readings = ""; Boolean[] ar = new Boolean[3];
            _serialPort.Close();
            _serialPort.Dispose();
            if (!_serialPort.IsOpen)
                _serialPort.Open();
            readings = _serialPort.ReadTo("\r\n");
            if(readings.Length<3)return;
            ar[0] = (readings[0] == '1')? true :false;
            ar[1] = (readings[1] == '1')?true:false;
            ar[2] = (readings[2] == '1')?true:false;

            new Thread(() =>
            {
                appendFile(readings);
                txtValue.Dispatcher.BeginInvoke((Action)(() => txtValue.Text = "Sensor 1:  " + ar[0] + "\nSensor 2:  " + ar[1] + "\nSensor 3:  " + ar[2]));
            }).Start();
            _serialPort.Close();
            _serialPort.Dispose();
        }
        WaterFilter prev = new WaterFilter();
        private async void readFile()
        {
            try
            {
                var item = await MobileService.GetTable<WaterFilter>().Take(1).OrderByDescending(e => e.CreatedAt).ToListAsync();
                if(item.Count!=0)prev= item[0];
            }
            catch (Exception e)
            { }
        }
        private async void appendFile(string reading)
        {
            new Thread(() =>
            {
                readFile();
            }).Start();
            bool[] ar1 = new bool[3];
            if (reading.Length < 3) return;
            ar1[0] = (reading[0] == '1') ? true : false;
            ar1[1] = (reading[1] == '1') ? true : false;
            ar1[2] = (reading[2] == '1') ? true : false;
            try
            {
                using (StreamWriter sw = File.AppendText(file1))
                {
                    sw.WriteLine(DateTime.Now.ToString() + "," + reading[0]+"," +reading[1] +"," +reading[2]);
                }
            }
            catch (Exception e)
            { }
            try
            {
                WaterFilter item = new WaterFilter { sensor_1 = ar1[0], sensor_2 = ar1[1], sensor_3 = ar1[2],last0_1=prev.last0_1,last0_2=prev.last0_2,last0_3=prev.last0_3,last1_1=prev.last1_1,last1_2=prev.last1_2,last1_3=prev.last1_3};
                if (item.sensor_1 == true && prev.sensor_1 == false)
                    item.last1_1 = prev.CreatedAt;
                if (item.sensor_2 == true && prev.sensor_2 == false)
                    item.last1_2 = prev.CreatedAt;
                if (item.sensor_3 == true && prev.sensor_3 == false)
                    item.last1_3 = prev.CreatedAt;
                if (item.sensor_1 == false && prev.sensor_1 == true)
                    item.last0_1 = prev.CreatedAt;
                if (item.sensor_2 == false && prev.sensor_2 == true)
                    item.last0_2 = prev.CreatedAt;
                if (item.sensor_3 == false && prev.sensor_3 == true)
                    item.last0_3 = prev.CreatedAt;
                await MobileService.GetTable<WaterFilter>().InsertAsync(item);
            }
            catch (Exception e)
            { }
        }
        string file1;
        string COMPortSelected = "COM4";
        private void COMPort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            COMPortSelected = COMPort.SelectedItem.ToString();
            int serial = InitializeCOMPorts();
            if (serial == -1)
            {
                System.Windows.MessageBox.Show("Device is not connected at " + COMPortSelected + " !!", "Error");
                btnBrowse.IsEnabled = false;
            }
            else { btnBrowse.IsEnabled = true; InitSerialPort(COMPortSelected); }
            btnMeasure.IsEnabled = false;
        }
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop(); dispatcherTimer.IsEnabled = false;
            dispatcherTimer1.Stop(); dispatcherTimer1.IsEnabled = false;
            _serialPort.Close();
            _serialPort.Dispose();
            prgBar.Value = 0; ch = 0;
            btnStop.IsEnabled = false;
            btnMeasure.IsEnabled = true;
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                DialogResult result = fbd.ShowDialog();
                txtFile.Text = fbd.SelectedPath;
                file1 = fbd.SelectedPath + "/WaterFilter.csv";
                if (File.Exists(file1)) { btnMeasure.IsEnabled = true; return; }
                using (StreamWriter sw = File.AppendText(file1))
                {
                    sw.WriteLine("TimeStamp,Sensor-1,Sensor-2,Sensor-3");
                }
                btnMeasure.IsEnabled = true;
            }
            catch { }
        }

    }
}
