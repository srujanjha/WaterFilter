using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WaterFilterWeb
{
    public partial class HomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txt[0] = Label1;
            txt[1] = Label2;
            txt[2] = Label3;
            txt[3] = Label4;
            txt[4] = Label5;
            txt[5] = Label6;
            rd[0] = R1;
            rd[1] = R2;
            rd[2] = R3;
            rd[3] = R4;
            rd[4] = R5;
            rd[5] = R6;
            gd[0] = G1;
            gd[1] = G2;
            gd[2] = G3;
            gd[3] = G4;
            gd[4] = G5;
            gd[5] = G6;
            refresh();
        }
        Label[] txt = new Label[6];
        Image[] rd = new Image[6];
        Image[] gd = new Image[6];
        Boolean Refresh = true;
        public void refresh()
        {
            string url = "https://srujan.azure-mobile.net/tables/telemetry?$top=1&$orderby=__createdAt%20desc";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream receiveStream =  response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            string str = readStream.ReadLine();
            str = str.Substring(str.IndexOf("title") + 8, 6);
            for (int i = 0; i < 6; i++)
            {
                char c = str.ElementAt(i);
                if (c == '0') { txt[i].Text = "OFF";
                    rd[i].Visible = true;gd[i].Visible = false;
                }
                else { txt[i].Text = "ON";
                    gd[i].Visible = true; rd[i].Visible = false;
                }
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