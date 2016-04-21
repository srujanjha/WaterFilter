using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WaterFiltersWeb
{
    public partial class HomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txt[0] = Label1;
            txt[1] = Label2;
            txt[2] = Label3;
            refresh();
        }
        Label[] txt = new Label[6];
        Boolean Refresh = true;
        public async void refresh()
        {

            var client = new HttpClient();
            var response = await client.GetAsync(new Uri("https://waterpurity.azure-mobile.net/tables/waterfilter?$top=1&$orderby=__createdAt%20desc&__systemproperties=createdAt"));
            var jstring = await response.Content.ReadAsStringAsync();
            for (int i = 0; i < 3; i++)
            {
                string id = "Device#" + (i + 1);
                int ix = jstring.IndexOf("sensor_" + (i + 1));
                int lx = jstring.IndexOf("\"", ix + 8);
                int rx = jstring.IndexOf("\"", lx + 2);
                string curr_state = jstring.Substring(lx + 2, rx - lx - 3);
                txt[i].Text = curr_state;
            }
            Refresh = true;
            btnRefresh.Enabled = true;
        }


        protected void btnRefresh_Click(object sender, EventArgs e)
        {

            if (Refresh)
            {
                Refresh = false;// btnRefresh.Enabled = false; 
                refresh();
            }
        }
    }
}