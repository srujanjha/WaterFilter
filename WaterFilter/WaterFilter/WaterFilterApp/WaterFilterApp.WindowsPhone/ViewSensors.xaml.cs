using WaterFilterApp.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using System.Threading;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace WaterFilterApp
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
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ViewSensors()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

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
               // TimeSpan ts = dt1.Subtract(dt0);
                string color = "#FF00FF00";
                if (curr.sensor_1 == false) color = "#FFFF0000";
                states.Add(new States("Device#1", color, "", "", "" + " minutes"));
                //dt1 = curr.last1_2.Value.DateTime;
                //dt0 = curr.last0_2.Value.DateTime;
               // ts = dt1.Subtract(dt0);
                color = "#FF00FF00";
                if (curr.sensor_2 == false) color = "#FFFF0000";
                states.Add(new States("Device#1", color, "","", ""+ " minutes"));
               // dt1 = curr.last1_3.Value.DateTime;
               // dt0 = curr.last0_3.Value.DateTime;
               // ts = dt1.Subtract(dt0);
                color = "#FF00FF00";
                if (curr.sensor_3 == false) color = "#FFFF0000";
                states.Add(new States("Device#1", color,"", "",""+ " minutes"));
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