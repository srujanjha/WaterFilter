using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WaterFilterApps
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewSensors : Page
    {
        public static MobileServiceClient MobileService = new MobileServiceClient(
    "https://waterpurity.azure-mobile.net/",
    "OQKvyKMVopTsOpqmSvADpTDbMfzLNZ52"
);
        public ViewSensors()
        {
            this.InitializeComponent();
        }
        
        WaterFilter curr;
        private async void readFile()
        {
            try
            {
                var item = await MobileService.GetTable<WaterFilter>().Take(1).OrderByDescending(e => e.CreatedAt).ToListAsync();
                if (item.Count != 0) curr = item[0];
            }
            catch (Exception e)
            { }
        }
        public static int click;
        public static int selectedIndex;
        public List<States> states = new List<States>();
        public async void refresh()
        {
            try
            {
                var item = await MobileService.GetTable<WaterFilter>().Take(1).OrderByDescending(e => e.CreatedAt).ToListAsync();
                if (item.Count != 0) curr = item[0];
                //DateTime dt1 = curr.last1_1.Value.DateTime;
                //DateTime dt0 = curr.last0_1.Value.DateTime;
                //TimeSpan ts = dt1.Subtract(dt0);
                string color = "#FF00FF00";
                string sta = "ON";
                if (curr.sensor_1 == false)
                {
                    sta = "OFF"; color = "#FFFF0000";
                }
                states.Add(new States("Device#1: " + sta, color, "", "", "" + " minutes"));
                // dt1 = curr.last1_2.Value.DateTime;
                // dt0 = curr.last0_2.Value.DateTime;
                // ts = dt1.Subtract(dt0);
                sta = "ON"; color = "#FF00FF00";
                if (curr.sensor_2 == false) { sta = "OFF"; color= "#FFFF0000"; }
                states.Add(new States("Device#2: "+sta, color, "","","" + " minutes"));
                //  dt1 = curr.last1_3.Value.DateTime;
                //  dt0 = curr.last0_3.Value.DateTime;
                //  ts = dt1.Subtract(dt0);
                sta = "ON"; color = "#FF00FF00";
                if (curr.sensor_3 == false) { sta = "OFF"; color = "#FFFF0000"; }
                states.Add(new States("Device#3: "+sta, color,"", "", ""+ " minutes"));
                Sensors.ItemsSource = states;
            }
            catch (Exception) { }
        }
        private async Task DoPeriodicWorkAsync(TimeSpan dueTime, TimeSpan interval, CancellationToken token)
        {
            // Initial wait time before we begin the periodic loop.
            try
            {
                if (dueTime > TimeSpan.Zero)
                    await Task.Delay(dueTime, token);

                // Repeat this loop until cancelled.
                while (!token.IsCancellationRequested)
                {
                    refresh();
                    // Wait to repeat again.
                    if (interval > TimeSpan.Zero)
                        await Task.Delay(interval, token);
                }
            }
            catch (Exception) { }
        }

        private async void Window_ContentRendered(object sender, EventArgs e)
        {
            try
            {
                refresh();
                var dueTime = TimeSpan.FromSeconds(2);
                var interval = TimeSpan.FromSeconds(2);
                await DoPeriodicWorkAsync(dueTime, interval, CancellationToken.None);
            }
            catch (Exception) { }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                refresh();
                var dueTime = TimeSpan.FromSeconds(2);
                var interval = TimeSpan.FromSeconds(2);
                await DoPeriodicWorkAsync(dueTime, interval, CancellationToken.None);
            }
            catch (Exception) { }
        }
    }
    public class States
    {
        public States(string id, string status, string defect, string repair, string repairTime)
        {
            ID = id;
            Status = status;
            Defect = defect;
            Repair = repair;
            RepairTime = repairTime;
        }
        public string ID { get; set; }
        public string Status { get; set; }
        public string Defect { get; set; }
        public string Repair { get; set; }
        public string RepairTime { get; set; }
    }
}