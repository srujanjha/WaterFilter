using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace WaterFilter
{
    /// <summary>
    /// Interaction logic for ViewSensors.xaml
    /// </summary>
    public partial class ViewSensors : Window
    {
        public ViewSensors()
        {
            InitializeComponent();
        }

        public static int click;
        public static int selectedIndex;
        public List<States> states = new List<States>();
        private void ListViewItem_Selected_1(object sender, RoutedEventArgs e)
        {
            click = 1;
            ViewLog cs1 = new ViewLog();
            this.Close();
            cs1.Show();
        }
        public async void refresh()
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync(new Uri("https://waterpurity.azure-mobile.net/tables/waterfilter?%24top=1&%24orderby=__createdAt+desc"));
                var jstring = await response.Content.ReadAsStringAsync();
                states.Clear();
                for (int i = 0; i < 3; i++)
                {
                    string id = "Device#" + (i + 1);
                    int ix = jstring.IndexOf("sensor_" + (i + 1));
                    int lx = jstring.IndexOf("\"", ix + 8);
                    int rx = jstring.IndexOf("\"", lx + 2);
                    string curr_state = jstring.Substring(lx + 2, rx - lx - 3);

                    int t1 = jstring.IndexOf("last0_" + (i + 1));
                    int t2 = jstring.IndexOf("last1_" + (i + 1));
                    int t11 = jstring.IndexOf("\"", t1 + 9);
                    int t21 = jstring.IndexOf("\"", t2 + 9);
                    int t12 = jstring.IndexOf("\"", t11 + 2);
                    int t22 = jstring.IndexOf("\"", t21 + 2);
                    string last0 = jstring.Substring(t11 + 1, t12 - 1 - t11);
                    DateTime dt = Convert.ToDateTime(last0);
                    string last1 = jstring.Substring(t21 + 1, t22 - 1 - t21);
                    DateTime dt1 = Convert.ToDateTime(last1);
                    TimeSpan ts = dt1.Subtract(dt);
                    string days = ts.Days.ToString();
                    string hours = ts.TotalSeconds.ToString();
                    string minutes = ts.TotalMinutes.ToString();
                    // MessageBox.Show("ID:" + (i + 1) + " Staus:" + curr_state + " Last0:" + last0 + " Last1:" + last1);
                    //TimeSpan ts1=ts.Duration();
                    //TextBlock txt = (TextBlock)Logs.Children[i];
                    //txt.Text = "Sensor #" + (i + 1) + ": " + s;
                    if (curr_state == "true")
                    {
                        states.Add(new States(id,"#FF00FF00", last0, last1,minutes+" minutes"));
                      
                    }
                    else
                        states.Add(new States(id,"#FFFF0000",last0, last1,minutes + " minutes"));
                }
                Sensors.ItemsSource= states;
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

        private void Sensors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           selectedIndex = Sensors.SelectedIndex;
            ViewLog cs = new ViewLog();
            this.Close();
            cs.Show();
        }
    }

    public class States
    {
       public States(string id,string status,string defect,string repair,string repairTime)
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
