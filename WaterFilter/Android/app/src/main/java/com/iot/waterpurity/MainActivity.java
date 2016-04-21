package com.iot.waterpurity;

import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Handler;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.Reader;
import java.io.UnsupportedEncodingException;
import java.net.HttpURLConnection;
import java.net.URL;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;

public class MainActivity extends AppCompatActivity {
    TextView txtDeviceId1,txtDeviceId2,txtDeviceId3;
    ImageView img1,img2,img3;
    final Handler h = new Handler();
    FloatingActionButton fab;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        toolbar.setTitle("Current Status");
        setSupportActionBar(toolbar);
        txtDeviceId1=(TextView)findViewById(R.id.txtDeviceId_1);
        txtDeviceId2=(TextView)findViewById(R.id.txtDeviceId_2);
        txtDeviceId3=(TextView)findViewById(R.id.txtDeviceId_3);
        img1=(ImageView)findViewById(R.id.imgStatus_1);
        img2=(ImageView)findViewById(R.id.imgStatus_2);
        img3=(ImageView)findViewById(R.id.imgStatus_3);
        fab = (FloatingActionButton) findViewById(R.id.fab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                new RetrieveFeedTask().execute();refresh = false;
            }
        });
        final int delay = 2000; //milliseconds
        new RetrieveFeedTask().execute();
        h.postDelayed(new Runnable(){
            public void run(){
                new RetrieveFeedTask().execute();
                h.postDelayed(this, delay);
            }
        }, delay);
        txtDeviceId1.setText("device#1");
        txtDeviceId2.setText("device#2");
        txtDeviceId3.setText("device#3");
        setContentView(R.layout.activity_main);
    }
    public static boolean refresh=true;
    public static String s="[{\"id\":\"F8AE5098-4034-48BB-8782-5992FED9761F\",\"sensor_1\":true,\"sensor_2\":true,\"sensor_3\":true,\"last0_1\":\"2016-03-15T08:32:57.468Z\",\"last0_2\":\"2016-03-15T08:32:57.468Z\",\"last0_3\":\"2016-03-15T08:32:57.468Z\",\"last1_1\":\"2016-03-15T08:35:23.846Z\",\"last1_2\":\"2016-03-15T08:35:23.846Z\",\"last1_3\":\"2016-03-15T08:35:23.846Z\"}]";
    private void refresh()
    {
        txtDeviceId1=(TextView)findViewById(R.id.txtDeviceId_1);
        txtDeviceId2=(TextView)findViewById(R.id.txtDeviceId_2);
        txtDeviceId3=(TextView)findViewById(R.id.txtDeviceId_3);
        img1=(ImageView)findViewById(R.id.imgStatus_1);
        img2=(ImageView)findViewById(R.id.imgStatus_2);
        img3=(ImageView)findViewById(R.id.imgStatus_3);
            try {
                System.out.println(s);
                JSONArray jsonArray=new JSONArray(s);
                JSONObject  jsonRootObject = jsonArray.getJSONObject(0);
                boolean s1= jsonRootObject.getBoolean("sensor_1");
                boolean s2= jsonRootObject.getBoolean("sensor_2");
                boolean s3= jsonRootObject.getBoolean("sensor_3");
                txtDeviceId1.setText("device#1");
                txtDeviceId2.setText("device#2");
                txtDeviceId3.setText("device#3");
                System.out.println(txtDeviceId1.getText()+" "+txtDeviceId2.getText()+" "+txtDeviceId3.getText());
                if(s1)img1.setImageResource(R.drawable.green);else img1.setImageResource(R.drawable.red);
                if(s2)img2.setImageResource(R.drawable.green);else img2.setImageResource(R.drawable.red);
                if(s3)img3.setImageResource(R.drawable.green);else img3.setImageResource(R.drawable.red);
                refresh = true;fab.setEnabled(true);
            } catch (Exception e) {e.printStackTrace();}
    }
    class RetrieveFeedTask extends AsyncTask<Void, Void, String> {

        private Exception exception;

        protected String doInBackground(Void... urls) {
            try {
                InputStream stream = null;
                String str ="";
                try {
                    stream = downloadUrl("https://waterpurity.azure-mobile.net/tables/waterfilter?$top=1&$orderby=__createdAt%20desc");
                    str = readIt(stream, 500);
                } finally {
                    if (stream != null) {
                        stream.close();
                    }
                }
                s=str;
                return str;
            }catch (Exception e) {
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
            System.out.println("Refresh Done");
            refresh();
        }
    }
}
