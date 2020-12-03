package com.example.milanchinthaka.bbcallbutton_android;

import android.content.Intent;
import android.content.SharedPreferences;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import java.io.BufferedInputStream;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.PrintWriter;

public class IPSetActivity extends AppCompatActivity {

    private Button btnsetip;
    private TextView ipn;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_ipset);

        final EditText ipst = (EditText) findViewById(R.id.txtIP);
        //final File ipAddress = new File(getFilesDir(), "IPAddress.txt");
        SharedPreferences prefs = getSharedPreferences("MY_PREF", MODE_PRIVATE);
        String ip = prefs.getString("ipaddr", null);
        if(ip != null){
            ipst.setText(ip);
        }
        //final File ipAddress = new File(Environment.getExternalStorageDirectory().getAbsolutePath() + "/IPAddress.txt");

//        try {
////            File secondInputFile = new File(ipAddress.getAbsolutePath());
//            BufferedReader br = new BufferedReader(new FileReader(ipAddress));
//            String line=br.readLine();
//            br.close();
//            ipst.setText(line);
//        } catch (Exception e) {
//            e.printStackTrace();
//        }


        btnsetip = (Button) findViewById(R.id.btnSetIP);
        ipn = (TextView) findViewById(R.id.txtIPFnotification);


        try {
            btnsetip.setOnClickListener(new View.OnClickListener() {


                //BufferedWriter writer = new BufferedWriter(new FileWriter(ipAdress));
//                MainFragment mainf = new MainFragment();
//
                @Override
                public void onClick(View view) {
                    if (ipst.getText().toString().matches("(^\\d{1,3}(\\.(\\d{1,3}(\\.(\\d{1,3}(\\.(\\d{1,3})?)?)?)?)?)?)")){


//                        try {
//                            PrintWriter pr = new PrintWriter(ipAddress);
//                            pr.write(ipst.getText().toString());
//                            pr.close();
//                        } catch (IOException e) {
//                            e.printStackTrace();
//                        }

                        SharedPreferences.Editor editor = getSharedPreferences("MY_PREF", MODE_PRIVATE).edit();
                        editor.putString("ipaddr", ipst.getText().toString());
                        editor.commit();

                        TCPService.externalStart = true;

                        finish();
                        Intent mainActivity = new Intent(getApplicationContext(), MainActivity.class);
                        startActivity(mainActivity);

                    } else {
                        //may be pass error.
                        Toast.makeText(getApplicationContext(),"IP Pattern Not Matched!",Toast.LENGTH_LONG).show();
                    }

                }
            });
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
