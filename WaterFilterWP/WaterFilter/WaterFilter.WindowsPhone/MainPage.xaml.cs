using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WaterFilter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }
        TextBlock[] txt = new TextBlock[6];
        Ellipse[] rd = new Ellipse[6];

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.
            try { txt[0] = txt1;
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
                var dueTime = TimeSpan.FromSeconds(2);
                var interval = TimeSpan.FromSeconds(2);

                // TODO: Add a CancellationTokenSource and supply the token here instead of None.
                DoPeriodicWorkAsync(dueTime, interval, CancellationToken.None);
            }
            catch (Exception) { }
        }
        Boolean Refresh = false;
        public async void refresh()
        {
            try
            {
                var client = new HttpClient();
            var response = await client.GetAsync(new Uri("https://srujan.azure-mobile.net/tables/telemetry?$top=1&$orderby=__createdAt%20desc"));
            var jstring = await response.Content.ReadAsStringAsync();
            JsonValue ob = JsonValue.Parse(jstring);
            JsonArray ob1 = ob.GetArray();
            string s = ob1.GetObjectAt(0).GetNamedString("title");
            for(int i=0;i<6;i++)
            {
                char c = s.ElementAt(i);
                if (c == '0') { txt[i].Text = "OFF";rd[i].Fill = new SolidColorBrush(Windows.UI.Colors.Red); }
                else { txt[i].Text = "ON"; rd[i].Fill = new SolidColorBrush(Windows.UI.Colors.Green); }
            }
            Refresh = true;
            }
            catch (Exception) { }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Refresh)
            { refresh(); Refresh = false;
        }
            }
            catch (Exception) { }
        }
        private async Task DoPeriodicWorkAsync(TimeSpan dueTime,
                                       TimeSpan interval,
                                       CancellationToken token)
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
    }
}
