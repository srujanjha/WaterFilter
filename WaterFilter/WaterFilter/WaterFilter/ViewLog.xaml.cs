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
    /// Interaction logic for ViewLog.xaml
    /// </summary>
    public partial class ViewLog : Window
    {
        public ViewLog()
        {
            InitializeComponent();
            Device.Text = "Device" + (ViewSensors.selectedIndex+1);
            Color color = (Color)ColorConverter.ConvertFromString("#FF3C8CB2");
            SolidColorBrush brush = new SolidColorBrush(color);
            Thickness thick = new Thickness(0, 0, 40, 0);
            var style = new Style(typeof(System.Windows.Controls.Primitives.DataGridColumnHeader));
            style.Setters.Add(new Setter { Property = BackgroundProperty, Value = brush });
            style.Setters.Add(new Setter { Property = ForegroundProperty, Value = Brushes.White });
            style.Setters.Add(new Setter { Property = BorderBrushProperty, Value = brush });
            style.Setters.Add(new Setter { Property = BorderThicknessProperty, Value = thick });
            Logs.ColumnHeaderStyle = style;
            Logs.ColumnHeaderHeight = 30;

            Logs.FontSize = 15;
           
        }
        public List<Logs1> log = new List<Logs1>();
       


        public async void refresh()
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync(new Uri("http://waterpurity.azure-mobile.net/tables/waterfilter?$select=sensor_" + (ViewSensors.selectedIndex + 1)));
                var jstring = await response.Content.ReadAsStringAsync();
                int index = 0;
                log.Clear();
                while (index<jstring.LastIndexOf("sensor_" + (ViewSensors.selectedIndex + 1))+8)
                {
                    int ix = jstring.IndexOf("sensor_" + (ViewSensors.selectedIndex + 1), index);
                    int lx = jstring.IndexOf("\"", ix + 8);
                    int rx = jstring.IndexOf("}", lx + 2);
                    string curr_state = jstring.Substring(lx + 2, rx - lx - 2);
                    index = rx + 2;
                    if(curr_state=="true")
                    log.Add(new Logs1("16-3-2015","#FF00FF00"));
                    else
                    log.Add(new Logs1("16-3-2015","#FFFF0000"));
                }
                Logs.ItemsSource = log;
                
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
                var dueTime = TimeSpan.FromSeconds(10);
                var interval = TimeSpan.FromSeconds(10);
                await DoPeriodicWorkAsync(dueTime, interval, CancellationToken.None);
            }
            catch (Exception) { }
        }

      
    }

    public class Logs1
    {
        public Logs1(string date, string status)
        {
            Date = date;
            Status = status;
        }
        public string Date { get;set;}
        public string Status { get; set; }
        
    }
}
