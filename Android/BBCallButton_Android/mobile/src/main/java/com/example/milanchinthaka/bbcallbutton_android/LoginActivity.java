package com.example.milanchinthaka.bbcallbutton_android;

import android.content.Intent;
import android.content.SharedPreferences;
import android.media.RingtoneManager;
import android.net.Uri;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.TextView;
import android.widget.Toast;

import java.io.File;
import java.io.IOException;
import java.io.PrintWriter;

public class LoginActivity extends AppCompatActivity {

    TextView fileNameTxt;

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent intent) {
        // Check which request we're responding to
        if (requestCode == 5) {
            // Make sure the request was successful
            if (resultCode == RESULT_OK) {
                Uri uri = intent.getParcelableExtra(RingtoneManager.EXTRA_RINGTONE_PICKED_URI);
                fileNameTxt.setText(RingtoneManager.getRingtone(getApplicationContext(), uri).getTitle(getApplicationContext()));

                TCPService.toneURI = uri;
                SharedPreferences.Editor editor = getSharedPreferences("MY_PREF", MODE_PRIVATE).edit();
                editor.putString("tone", uri.toString());
                editor.commit();
            }
        }
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        final EditText userNameTxt = (EditText) findViewById(R.id.txtUName);
        final EditText passwordTxt= (EditText) findViewById(R.id.txtPassword);
        final Button btnLogin = (Button) findViewById(R.id.btnLogin);
        final Button btnCancel = (Button) findViewById(R.id.btnCancel);
        final EditText ipst = (EditText) findViewById(R.id.txtIP2);
        final TextView nameTxt = (TextView) findViewById(R.id.txtName);
        final ImageButton btnTone = (ImageButton) findViewById(R.id.toneSelectBtn);
       // final ImageButton btnSettings = (ImageButton) findViewById(R.id.btnSettings);
        fileNameTxt = (TextView) findViewById(R.id.fileNameTxt);

        ImageButton btnHistory = (ImageButton) findViewById(R.id.historyBtnLogin);
        ImageButton settingbtn = (ImageButton) findViewById(R.id.btnSettingsLogin);
        ImageButton configbtn = (ImageButton) findViewById(R.id.configBtnLogin);

        configbtn.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent credit = new Intent(getApplicationContext(), ConfigActivity.class);
                startActivity(credit);

                finish();
            }

        });

        settingbtn.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent credit = new Intent(getApplicationContext(), LoginActivity.class);
                startActivity(credit);

                finish();
            }

        });

        btnHistory.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent credit = new Intent(getApplicationContext(), CallListActivity.class);
                startActivity(credit);

                finish();
            }

        });


        btnTone.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(RingtoneManager.ACTION_RINGTONE_PICKER);
                intent.putExtra(RingtoneManager.EXTRA_RINGTONE_TYPE, RingtoneManager.TYPE_NOTIFICATION);
                intent.putExtra(RingtoneManager.EXTRA_RINGTONE_TITLE, "Select Tone");
                intent.putExtra(RingtoneManager.EXTRA_RINGTONE_EXISTING_URI, (Uri) null);
                startActivityForResult(intent, 5);
            }

        });

        SharedPreferences prefs = getSharedPreferences("MY_PREF", MODE_PRIVATE);
        String ip = prefs.getString("ipaddr", null);
        if(ip != null){
            ipst.setText(ip);
        }
        String uname = prefs.getString("username", null);
        if(uname != null){
            userNameTxt.setText(uname);
        }
        String pword = prefs.getString("password", null);
        if(pword != null){
            passwordTxt.setText(pword);
        }
        String empname = prefs.getString("emp_name", null);

        if (empname != null){
            nameTxt.setText(empname);
        }

        String tone = prefs.getString("tone", null);
        if(tone != null){
            fileNameTxt.setText(RingtoneManager.getRingtone(getApplicationContext(), Uri.parse(tone)).getTitle(getApplicationContext()));
        }

        btnLogin.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                SharedPreferences.Editor editor = getSharedPreferences("MY_PREF", MODE_PRIVATE).edit();
                if (ipst.getText().toString().matches("(^\\d{1,3}(\\.(\\d{1,3}(\\.(\\d{1,3}(\\.(\\d{1,3})?)?)?)?)?)?)")){
                    editor.putString("ipaddr", ipst.getText().toString());

                    final String uname = userNameTxt.getText().toString();
                    final String pword = passwordTxt.getText().toString();
                    editor.putString("username", uname);
                    editor.putString("password", pword);
                    editor.commit();

                    finish();
                    Intent mainActivity = new Intent(getApplicationContext(), MainActivity.class);
                    mainActivity.setFlags(Intent.FLAG_ACTIVITY_REORDER_TO_FRONT);
//                    mainActivity.putExtra("pop", "true");
                    startActivity(mainActivity);
                } else {
                    Toast.makeText(getApplicationContext(),"IP Pattern Not Matched!",Toast.LENGTH_LONG).show();
                    return;
                }
            }
        });
        btnCancel.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                userNameTxt.setText("");
                passwordTxt.setText("");

                finish();
            }

        });
    }
}
