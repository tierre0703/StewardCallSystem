package com.example.milanchinthaka.bbcallbutton_android;

import android.app.Activity;
import android.content.Intent;
import android.content.SharedPreferences;
import android.media.RingtoneManager;
import android.net.Uri;
import android.os.Bundle;
import android.support.wearable.view.WatchViewStub;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

public class LoginActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        final WatchViewStub stub = (WatchViewStub) findViewById(R.id.watch_view_stub_login);
        stub.setOnLayoutInflatedListener(new WatchViewStub.OnLayoutInflatedListener() {
            @Override
            public void onLayoutInflated(WatchViewStub stub) {
                final EditText userNameTxt = (EditText) stub.findViewById(R.id.txtUName);
                final EditText passwordTxt= (EditText) stub.findViewById(R.id.txtPassword);
                final Button btnLogin = (Button) stub.findViewById(R.id.btnLogin);
                final Button btnCancel = (Button) stub.findViewById(R.id.btnCancel);
                final EditText ipst = (EditText) stub.findViewById(R.id.txtIP2);
                final TextView nameTxt = (TextView) stub.findViewById(R.id.txtName);

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
        });

    }
}
