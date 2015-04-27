package bioproject.srujan.waterfilter;

import android.os.AsyncTask;
import android.os.Handler;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.Reader;
import java.io.UnsupportedEncodingException;
import java.net.HttpURLConnection;
import java.net.URL;


public class MainActivity extends ActionBarActivity {
    TextView txt[]=new TextView[6];
    ImageView img[]=new ImageView[6];
    Button btnRefresh;
    final Handler h = new Handler();
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        btnRefresh=(Button)findViewById(R.id.btn_refresh);
        txt[0]=(TextView)findViewById(R.id.txt1);
        txt[1]=(TextView)findViewById(R.id.txt2);
        txt[2]=(TextView)findViewById(R.id.txt3);
        txt[3]=(TextView)findViewById(R.id.txt4);
        txt[4]=(TextView)findViewById(R.id.txt5);
        txt[5]=(TextView)findViewById(R.id.txt6);
        img[0]=(ImageView)findViewById(R.id.r1);
        img[1]=(ImageView)findViewById(R.id.r2);
        img[2]=(ImageView)findViewById(R.id.r3);
        img[3]=(ImageView)findViewById(R.id.r4);
        img[4]=(ImageView)findViewById(R.id.r5);
        img[5]=(ImageView)findViewById(R.id.r6);
        refresh();

       final int delay = 2000; //milliseconds

        h.postDelayed(new Runnable(){
            public void run(){
                refresh();
                h.postDelayed(this, delay);
            }
        }, delay);
        setContentView(R.layout.activity_main);
    }
    public static boolean refresh=true;
    public static String s="000000";
    private void refresh()
    {
        txt[0]=(TextView)findViewById(R.id.txt1);
        txt[1]=(TextView)findViewById(R.id.txt2);
        txt[2]=(TextView)findViewById(R.id.txt3);
        txt[3]=(TextView)findViewById(R.id.txt4);
        txt[4]=(TextView)findViewById(R.id.txt5);
        txt[5]=(TextView)findViewById(R.id.txt6);
        img[0]=(ImageView)findViewById(R.id.r1);
        img[1]=(ImageView)findViewById(R.id.r2);
        img[2]=(ImageView)findViewById(R.id.r3);
        img[3]=(ImageView)findViewById(R.id.r4);
        img[4]=(ImageView)findViewById(R.id.r5);
        img[5]=(ImageView)findViewById(R.id.r6);
        try {
            new RetrieveFeedTask().execute();
 s = s.substring(s.indexOf("title")+8,s.indexOf("title")+14);
               try{
                    for(int i=0;i<6;i++)
                    {
                        char c = s.charAt(i);
                        if (c == '0') { txt[i].setText("OFF");img[i].setImageResource(R.drawable.red); }
                        else { txt[i].setText("ON"); img[i].setImageResource(R.drawable.green); }
                    }
                    refresh = true;btnRefresh.setEnabled(true);}
                catch(Exception e){}
        }
    catch(Exception e){}

    }
    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            //if (refresh)
        //{
            refresh(); refresh = false;
        //}
            return true;
        }

        return super.onOptionsItemSelected(item);
    }
    public void onRefresh(View v)
    {
        //if (refresh)
        //{
        refresh();refresh = false;
        //}
    }
    class RetrieveFeedTask extends AsyncTask<Void, Void, String> {

        private Exception exception;

        protected String doInBackground(Void... urls) {
            try {
                InputStream stream = null;
                String str ="";
                try {
                    stream = downloadUrl("https://srujan.azure-mobile.net/tables/telemetry?$top=1&$orderby=__createdAt%20desc");
                    str = readIt(stream, 500);
                } finally {
                    if (stream != null) {
                        stream.close();
                    }
                }

                s=str;
                return str;
            }catch (Exception e) {
                //Toast.makeText(getApplicationContext(),e.toString(),Toast.LENGTH_SHORT).show();
                this.exception = e;
                return null;
            }
        }
        private InputStream downloadUrl(String urlString) throws IOException {
            URL url = new URL(urlString);
            HttpURLConnection conn = (HttpURLConnection) url.openConnection();
            conn.setReadTimeout(10000 /* milliseconds */);
            conn.setConnectTimeout(15000 /* milliseconds */);
            conn.setRequestMethod("GET");
            conn.setDoInput(true);
            // Start the query
            conn.connect();
            InputStream stream = conn.getInputStream();
            return stream;
        }
        private String readIt(InputStream stream, int len) throws IOException, UnsupportedEncodingException {
            Reader reader = null;
            reader = new InputStreamReader(stream, "UTF-8");
            char[] buffer = new char[len];
            reader.read(buffer);
            return new String(buffer);
        }
        protected void onPostExecute(String feed) {

        }
    }
}
