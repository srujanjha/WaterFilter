using Microsoft.WindowsAzure.MobileServices;
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


namespace WaterFilter_Desktop


{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public static MobileServiceClient MobileService = new MobileServiceClient(
    "https://waterpurity.azure-mobile.net/",
    "OQKvyKMVopTsOpqmSvADpTDbMfzLNZ52");

       public int click=1;
        public Window1()
        {
            InitializeComponent();
            chart.Titles[0].Text = "Logs";
            chart.ChartAreas[0].AxisX.Title = "Time";
            chart.ChartAreas[0].AxisY.Title = "Device Status";
            chart.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart.Series[0].BorderWidth = 2;
        }

        private void Device1_Click(object sender, RoutedEventArgs e)
        {
            click = 1;
            refresh();
            Color color = (Color)ColorConverter.ConvertFromString("#FF3C8CB2");
            Device1.Background = new SolidColorBrush(Colors.White);
            Device2.Background = new SolidColorBrush(color);
            Device3.Background = new SolidColorBrush(color);
            Device1.Foreground = new SolidColorBrush(Colors.Black);
            Device2.Foreground = new SolidColorBrush(Colors.White);
            Device3.Foreground = new SolidColorBrush(Colors.White);
        }
        WaterFilter curr;
        private void Device2_Click(object sender, RoutedEventArgs e)
        {
            click = 2;
            refresh();
            Color color = (Color)ColorConverter.ConvertFromString("#FF3C8CB2");
            Device1.Background = new SolidColorBrush(color);
            Device2.Background = new SolidColorBrush(Colors.White);
            Device3.Background = new SolidColorBrush(color);
            Device1.Foreground = new SolidColorBrush(Colors.White);
            Device2.Foreground = new SolidColorBrush(Colors.Black);
            Device3.Foreground = new SolidColorBrush(Colors.White);

        }

        private void Device3_Click(object sender, RoutedEventArgs e)
        {
            click = 3;
            refresh();
            Color color = (Color)ColorConverter.ConvertFromString("#FF3C8CB2");
            Device1.Background = new SolidColorBrush(color);
            Device2.Background = new SolidColorBrush(color);
            Device3.Background = new SolidColorBrush(Colors.White);
            Device1.Foreground = new SolidColorBrush(Colors.White);
            Device2.Foreground = new SolidColorBrush(Colors.White);
            Device3.Foreground = new SolidColorBrush(Colors.Black);
        }
        bool refreshB = false;

        /* private async void readFile()
         {
             string prev;
             try
             {
                 var item = await MobileService.GetTable<WaterFilter>().Take(1).OrderByDescending(e => e.CreatedAt).ToListAsync();
                 if (item.Count != 0) prev = item[0];
             }
             catch (Exception e)
             { }
         }*/
        public async void refresh()
        {
            if (refreshB) return;
            refreshB = true;
            try
            {
                var item2 = await MobileService.GetTable<WaterFilter>().Select(e => e.CreatedAt).ToListAsync();
                item2.Sort();
                var item =await MobileService.GetTable<WaterFilter>().Take(1).OrderByDescending(e => e.CreatedAt).ToListAsync();
                if (item.Count != 0) curr = item[0];
                else return;
                //DateTime dt1 = curr.last1_1.Value.DateTime;
                //DateTime dt0 = curr.last0_1.Value.DateTime;
                //TimeSpan ts = dt1.Subtract(dt0);
                Ellipse.Fill =Brushes.Green;
                if(click==1)
                {
                    if (curr.sensor_1 == false) Ellipse.Fill = Brushes.Red;
                    //Stopped.Text = curr.last0_1.ToString();
                    //Repaired.Text = curr.last1_1.ToString();
                    //Elapsed.Text = (curr.last1_1 - curr.last0_1).ToString();
                    var item1 = await MobileService.GetTable<WaterFilter>().OrderBy(e1 => e1.CreatedAt).Select(e => e.sensor_1).ToListAsync();
                    int ch = 0;
                    chart.Series["Log"].Points.Clear();
                    chart.ChartAreas[0].AxisX.CustomLabels.Clear();
                    foreach (var item3 in item1)
                    {
                        int x = (item3 == true ? 1 : 0);
                        chart.Series["Log"].Points.AddXY(ch, x.ToString());
                        if (ch % 3 == 0)
                            chart.ChartAreas[0].AxisX.CustomLabels.Add(ch - 0.5, ch + 0.5, item2[ch].Value.ToString("hh:mm:ss"));
                        if(x==1)
                            chart.Series["Log"].Points[ch].Color = System.Drawing.Color.FromArgb(0, 128, 0);
                        else chart.Series["Log"].Points[ch].Color = System.Drawing.Color.FromArgb(128, 0, 0);
                        ch++;
                    }
                }
                else
                    if (click == 2)
                {
                    if (curr.sensor_2 == false) Ellipse.Fill = Brushes.Red;
                    var item1 = await MobileService.GetTable<WaterFilter>().OrderBy(e1 => e1.CreatedAt).Select(e => e.sensor_2).ToListAsync();
                    int ch = 0;
                    chart.Series["Log"].Points.Clear();
                    chart.ChartAreas[0].AxisX.CustomLabels.Clear();
                    foreach (var item3 in item1)
                    {
                        int x = (item3 == true ? 1 : 0);
                        chart.Series["Log"].Points.AddXY(ch, x.ToString());
                        if (ch % 3 == 0)
                            chart.ChartAreas[0].AxisX.CustomLabels.Add(ch - 0.5, ch + 0.5, item2[ch].Value.ToString("hh:mm:ss"));
                        if (x == 1)
                            chart.Series["Log"].Points[ch].Color = System.Drawing.Color.FromArgb(0, 128, 0);
                        else chart.Series["Log"].Points[ch].Color = System.Drawing.Color.FromArgb(128, 0, 0);
                        ch++;
                    }
                }

                else
                    if (click == 3)
                {
                    if (curr.sensor_3 == false) Ellipse.Fill = Brushes.Red;
                    var item1 = await MobileService.GetTable<WaterFilter>().OrderBy(e1 => e1.CreatedAt).Select(e => e.sensor_3).ToListAsync();
                    int ch = 0;
                    chart.Series["Log"].Points.Clear();
                    chart.ChartAreas[0].AxisX.CustomLabels.Clear();
                    foreach (var item3 in item1)
                    {
                        int x = (item3 == true ? 1 : 0);
                        chart.Series["Log"].Points.AddXY(ch, x.ToString());
                        if (ch % 3 == 0)
                            chart.ChartAreas[0].AxisX.CustomLabels.Add(ch - 0.5, ch + 0.5, item2[ch].Value.ToString("hh:mm:ss"));
                        if (x == 1)
                            chart.Series["Log"].Points[ch].Color = System.Drawing.Color.FromArgb(0, 128, 0);
                        else chart.Series["Log"].Points[ch].Color = System.Drawing.Color.FromArgb(128, 0, 0);
                        ch++;
                    }
                }
            }
            catch (Exception e1)
            { }
            refreshB = false;
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

       
    }
}
