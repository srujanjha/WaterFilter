using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace WaterFilterWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBlock[] txt = new TextBlock[6];
        Ellipse[] rd = new Ellipse[6];
        public MainWindow()
        {
            InitializeComponent();
            txt[0] = txt1;
            txt[1] = txt2;
            txt[2] = txt3;
            txt[3] = txt4;
            txt[4] = txt5;
            txt[5] = txt6;
            rd[0] = r1;
            rd[1] = r2;
            rd[2] = r3;
            rd[3] = r4;
            rd[4] = r5;
            rd[5] = r6;
            refresh();
            var dueTime = TimeSpan.FromSeconds(5);
            var interval = TimeSpan.FromSeconds(5);

            // TODO: Add a CancellationTokenSource and supply the token here instead of None.
            DoPeriodicWorkAsync(dueTime, interval, CancellationToken.None);
        }
        Boolean Refresh = false;
        public async void refresh()
        {
            string url = "https://srujan.azure-mobile.net/tables/telemetry?$top=1&$orderby=__createdAt%20desc";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            string str = readStream.ReadLine();
            str = str.Substring(str.IndexOf("title") + 8, 6);
            for (int i = 0; i < 6; i++)
            {
                char c = str.ElementAt(i);
                if (c == '0') { txt[i].Text = "OFF"; rd[i].Fill = new SolidColorBrush(Color.FromRgb(255,0,0)); }
                else { txt[i].Text = "ON"; rd[i].Fill= new SolidColorBrush(Color.FromRgb(0,255, 0)); }
            }
            Refresh = true;
            btnRefresh.IsEnabled = true;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            if (Refresh)
            {
                Refresh = false; btnRefresh.IsEnabled = false; refresh();
            }
        }
        private async Task DoPeriodicWorkAsync(TimeSpan dueTime,
                                       TimeSpan interval,
                                       CancellationToken token)
        {
            // Initial wait time before we begin the periodic loop.
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
    }
}
