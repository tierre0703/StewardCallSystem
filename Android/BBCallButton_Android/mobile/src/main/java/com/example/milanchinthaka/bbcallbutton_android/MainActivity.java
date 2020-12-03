package com.example.milanchinthaka.bbcallbutton_android;

import android.app.ActivityManager;
import android.app.AlarmManager;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.database.sqlite.SQLiteDatabase;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.Button;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.util.Calendar;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;

public class MainActivity extends AppCompatActivity {

    public TextView nameTxt;
    public LinearLayout newCallLayout;
    public static MainActivity instance;
    public View pendingDecisionView;
    public static boolean isVisible;
    String empid = null;
    String empname = null;
    boolean powerBool = false;

    public HashMap<String, View> viewMap = new HashMap<>();

    public void setCallAccept(String uniqueId){
        final View rowView = viewMap.get(uniqueId);
        if(rowView == null){
            return;
        }

        TextView messageTxt = (TextView) rowView.findViewById(R.id.txtMessage);
        TextView headerTxt = (TextView) rowView.findViewById(R.id.txtHeader);
        final TextView btnAccept = (Button) rowView.findViewById(R.id.btnAccept);
        final TextView btnCancel = (Button) rowView.findViewById(R.id.btnCancelMain);
        btnAccept.setVisibility(View.GONE);
        btnCancel.setVisibility(View.GONE);

        String roomNo = "";
        if(Notifications.map.get(uniqueId) != null){
            Call call = Notifications.map.get(uniqueId).call;
            if(call != null){
                roomNo = call.roomNumber;
            }
        }
        headerTxt.setText("You accepted the call");
        messageTxt.setText("Please proceed to: " + roomNo);

        Notifications.map.remove(uniqueId);
    }

    public void setCallCancel(Call call){
        if(call != null){
            View v = viewMap.get(call.uniqueId);
            if(v != null) {
                newCallLayout.removeView(v);
            }
            viewMap.remove(call.uniqueId);

            Notifications.map.remove(call.uniqueId);
        }
    }

    public void removeCallWidget(Call call){
        if(call != null){
            View v = viewMap.get(call.uniqueId);
            if(v != null) {
                newCallLayout.removeView(v);
            }
            viewMap.remove(call.uniqueId);
        }
    }

    public void setErrorLabel(){
        if(pendingDecisionView != null){
            TextView messageTxt = (TextView) pendingDecisionView.findViewById(R.id.txtMessage);
            TextView headerTxt = (TextView) pendingDecisionView.findViewById(R.id.txtHeader);

            headerTxt.setText("Error!");
            messageTxt.setText("Error has occured on server");
            final TextView btnAccept = (Button) pendingDecisionView.findViewById(R.id.btnAccept);
            final TextView btnCancel = (Button) pendingDecisionView.findViewById(R.id.btnCancelMain);
            btnAccept.setVisibility(View.GONE);
            btnCancel.setVisibility(View.GONE);

            pendingDecisionView.postDelayed(new Runnable() {
                public void run() {
                    newCallLayout.removeView(pendingDecisionView);
                }
            }, 3000);
        }else{

            LayoutInflater inflater = (LayoutInflater) getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            final View rowView = inflater.inflate(R.layout.new_call_layout, null);

            TextView messageTxt = (TextView) rowView.findViewById(R.id.txtMessage);
            TextView headerTxt = (TextView) rowView.findViewById(R.id.txtHeader);

            headerTxt.setText("Error!");
            messageTxt.setText("Error has occured on server");
            final TextView btnAccept = (Button) rowView.findViewById(R.id.btnAccept);
            final TextView btnCancel = (Button) rowView.findViewById(R.id.btnCancelMain);
            btnAccept.setVisibility(View.GONE);
            btnCancel.setVisibility(View.GONE);

            newCallLayout.addView(rowView, 0);
            LinearLayout l = (LinearLayout) rowView;
            LinearLayout.LayoutParams layoutParams = (LinearLayout.LayoutParams) l.getLayoutParams();
            layoutParams.setMargins(0, 0, 0, 20);
            l.setLayoutParams(layoutParams);

            rowView.postDelayed(new Runnable() {
                public void run() {
                    newCallLayout.removeView(rowView);
                }
            }, 3000);
        }
    }

    @Override
    protected void onResume() {
        super.onResume();
        isVisible = true;
    }

    @Override
    protected void onPause() {
        super.onPause();
        finish();
    }

    @Override
    protected void onStop() {
        super.onStop();
        isVisible = false;
        if (Notifications.map.size() == 0){
            NotificationManager notificationManager = (NotificationManager) getApplicationContext().getSystemService(Context.NOTIFICATION_SERVICE);
            notificationManager.cancel(Notifications.notificationID);
        }
        finish();
    }

    public void addNewCallLayout(final Call call){
        LayoutInflater inflater = (LayoutInflater) getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        final View rowView = inflater.inflate(R.layout.new_call_layout, null);

        if(call != null){
            View v = viewMap.get(call.uniqueId);
            if(v != null) {
                newCallLayout.removeView(v);
            }
            viewMap.remove(call.uniqueId);
        }

        viewMap.put(call.uniqueId, rowView);

        final TextView messageTxt = (TextView) rowView.findViewById(R.id.txtMessage);
        TextView headerTxt = (TextView) rowView.findViewById(R.id.txtHeader);
        final TextView btnAccept = (Button) rowView.findViewById(R.id.btnAccept);
        final TextView btnCancel = (Button) rowView.findViewById(R.id.btnCancelMain);

        btnAccept.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
            new Thread(new Runnable() {
                @Override
                public void run() {
                if(TCPService.instance.connected) {
                    runOnUiThread(new Runnable() {
                        @Override
                        public void run() {
                            btnAccept.setEnabled(false);
                            btnCancel.setEnabled(false);
//                            for (int i =0 ; i < newCallLayout.getChildCount() ; i++){
//                                View v = newCallLayout.getChildAt(i);
//                                v.setEnabled(false);
//                            }
                            messageTxt.setText("Waiting for Server Respond");
                    }});

                    pendingDecisionView = rowView;

                    call.userAction = "A";
                    Database.getInstance().updateCall(call);

                    SharedPreferences prefs = getSharedPreferences("MY_PREF", MODE_PRIVATE);
                    String empid = prefs.getString("emp_id", null);

//                    Notifications.map.remove(call.uniqueId);
                    TCPService.instance.send(CommunicationStrings.ACCEPT_MESSAGE + call.uniqueId + ":" + empid + ":");

                    rowView.postDelayed(new Runnable() {
                        public void run() {
                        if(!messageTxt.getText().toString().startsWith("Please proceed to:")) {
                            messageTxt.setText("No response from the server. Try again");
                            runOnUiThread(new Runnable() {
                                  @Override
                                  public void run() {
                                  btnAccept.setEnabled(true);
                                  btnCancel.setEnabled(true);

//                                      for (int i = 0; i < newCallLayout.getChildCount(); i++) {
//                                          View v = newCallLayout.getChildAt(i);
//                                          v.setEnabled(true);
//                                      }
                                  }}
                            );
                        }
                        }
                    }, 5000);
                }else{
                    runOnUiThread(new Runnable() {
                        @Override
                        public void run() {
                    Toast.makeText(getApplicationContext(),"No connection with server",Toast.LENGTH_LONG).show();
                        }});
                }
                }
            }).start();
            }

        });

        btnCancel.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
            new Thread(new Runnable() {
                @Override
                public void run() {
                if(TCPService.instance.connected) {
//                  desicionPending = false;
                    runOnUiThread(new Runnable() {
                        @Override
                        public void run() {
                            btnAccept.setEnabled(false);
                            btnCancel.setEnabled(false);
//                            finish();
                            newCallLayout.removeView(rowView);
                            viewMap.remove(call.uniqueId);
                     }});

                    call.userAction = "R";
                    SharedPreferences prefs = getSharedPreferences("MY_PREF", MODE_PRIVATE);
                    String empid = prefs.getString("emp_id", null);
                    Database.getInstance().updateCall(call);

//                    Notifications.map.remove(call.uniqueId);

                    TCPService.instance.send(CommunicationStrings.CANCEL_MESSAGE + call.uniqueId + ":" + empid + ":");
                }else{
                    runOnUiThread(new Runnable() {
                        @Override
                        public void run() {
                          Toast.makeText(getApplicationContext(), "No connection with server", Toast.LENGTH_LONG).show();
                    }});
                }
                }
            }).start();
            }
        });

        headerTxt.setText("NEW CALL: " + call.roomNumber);

        messageTxt.setText("Received Call:" + call.roomNumber);

        newCallLayout.addView(rowView, 0);
        LinearLayout l = (LinearLayout) rowView;
        LinearLayout.LayoutParams layoutParams = (LinearLayout.LayoutParams) l.getLayoutParams();
        layoutParams.setMargins(0, 0, 0, 20);
        l.setLayoutParams(layoutParams);
    }

//    public void setCall(Call call){
//        this.call = call;
//        desicionPending = true;
////        btnAccept.setVisibility(View.VISIBLE);
////        btnCancel.setVisibility(View.VISIBLE);
////        messageTxt.setVisibility(View.VISIBLE);
////
////        btnAccept.setEnabled(true);
////        btnCancel.setEnabled(true);
////
////        headerTxt.setText("NEW CALL RECEIVED");
////        messageTxt.setText("Received a Call from " + call.roomNumber);
//
//        NotificationManager notificationManager = (NotificationManager) getApplicationContext().getSystemService(Context.NOTIFICATION_SERVICE);
//        notificationManager.cancel(Notifications.map.get(call.uniqueId));
//
//        Notifications.map.remove(call.uniqueId);
//    }

    private boolean isServiceRunning(Class<?> serviceClass) {
        ActivityManager manager = (ActivityManager) getSystemService(Context.ACTIVITY_SERVICE);
        for (ActivityManager.RunningServiceInfo service : manager.getRunningServices(Integer.MAX_VALUE)) {
            if (serviceClass.getName().equals(service.service.getClassName())) {
                return true;
            }
        }
        return false;
    }


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        SQLiteDatabase callDB = openOrCreateDatabase("call_db",MODE_PRIVATE,null);
        Database.getInstance().callDB = callDB;
        Database.getInstance().initDatabase();

        instance = this;
        nameTxt = (TextView) findViewById(R.id.txtName);

        newCallLayout = (LinearLayout) findViewById(R.id.newCallLayouot);
        ImageButton btnHistory = (ImageButton) findViewById(R.id.historyBtn);
        ImageButton settingbtn = (ImageButton) findViewById(R.id.btnSettings);
        ImageButton configbtn = (ImageButton) findViewById(R.id.configBtn);
        final ImageButton powerBtn = (ImageButton) findViewById(R.id.onoffBtn);

//        settingbtn.setOnLongClickListener();

        final SharedPreferences prefs = getSharedPreferences("MY_PREF", MODE_PRIVATE);

        String power = prefs.getString("power", null);

        if (power == null || power.equals("off")){
            powerBool = false;
            powerBtn.setImageResource(R.mipmap.ic_power_off);
        }else{
            powerBool = true;
            powerBtn.setImageResource(R.mipmap.ic_power);
        }

        configbtn.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent credit = new Intent(getApplicationContext(), ConfigActivity.class);
                startActivity(credit);
            }

        });

        settingbtn.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
            Intent credit = new Intent(getApplicationContext(), LoginActivity.class);
            startActivity(credit);
            }

        });

        btnHistory.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent credit = new Intent(getApplicationContext(), CallListActivity.class);
                startActivity(credit);
            }

        });

//        powerBtn.setOnClickListener(new View.OnClickListener() {
//
//            @Override
//            public void onClick(View v) {
//            final String power = prefs.getString("power", null);
//                powerBtn.setEnabled(false);
//            if (power == null || power.equals("off")){
//                powerBtn.setImageResource(R.mipmap.ic_power);
//                SharedPreferences.Editor editor = prefs.edit();
//                editor.putString("power", "on");
//                editor.commit();
//
//                Intent intent = new Intent(MainActivity.this, TCPService.class);
//                //stopService(intent);
//                TCPService.forcedStop = false;
//                TCPService.externalStart=true;
//                startService(intent);
//            }else {
//                powerBtn.setImageResource(R.mipmap.ic_power_off);
//                SharedPreferences.Editor editor = prefs.edit();
//                editor.putString("power", "off");
//                editor.commit();
//
//                TCPService.forcedStop = true;
//                Intent intent = new Intent(MainActivity.this, TCPService.class);
//                stopService(intent);
//            }
//
//            powerBtn.postDelayed(new Runnable() {
//                public void run() {
//                  powerBtn.setEnabled(true);
//                }
//            }, 3000);
//            }
//
//        });

        powerBtn.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                powerBtn.setEnabled(false);

                if (!powerBool){
                    powerBtn.setImageResource(R.mipmap.ic_power);

                    SharedPreferences.Editor editor = prefs.edit();
                    editor.putString("power", "on");
                    editor.commit();

                    Intent intent = new Intent(getApplicationContext(), TCPService.class);
                    startService(intent);
                    //}
                    TCPService.forcedStop = false;
                    TCPService.externalStart=true;

                    powerBool = true;

                    powerBtn.postDelayed(new Runnable() {
                        public void run() {
                            powerBtn.setEnabled(true);
                        }
                    }, 5000);
                }else {
                    powerBool = false;
                    powerBtn.setImageResource(R.mipmap.ic_power_off);
                    SharedPreferences.Editor editor = prefs.edit();
                    editor.putString("power", "off");
                    editor.commit();

                    TCPService.forcedStop = true;

                    nameTxt.setText(empname + " : Disconnected");
                    Intent intent = new Intent(getApplicationContext(), TCPService.class);
                    stopService(intent);

//                    finish();
                }
            }

        });


        empid = prefs.getString("emp_id", null);
        empname = prefs.getString("emp_name", null);

        if (empname != null){
            nameTxt.setText(empname);
        }

        for (Iterator<Map.Entry<String, CallNotify>> it = Notifications.map.entrySet().iterator(); it.hasNext();) {
            addNewCallLayout(it.next().getValue().call);
        }

//        NotificationManager notificationManager = (NotificationManager) getApplicationContext().getSystemService(Context.NOTIFICATION_SERVICE);
//        notificationManager.cancel(Notifications.notificationID);

        String ip = prefs.getString("ipaddr", null);
        String uname = prefs.getString("username", null);
        String pword = prefs.getString("password", null);

        if (ip != null){
            if(uname != null && pword != null){
                if (powerBool) {
                    boolean service = isServiceRunning(TCPService.class);
                    if (service && TCPService.instance.connected && TCPService.instance.authenticated){

                    }else {
                        Intent intent = new Intent(this, TCPService.class);
                        //stopService(intent);
                        TCPService.externalStart = true;
                        TCPService.forcedStop = false;
                        startService(intent);
                    }
                }
            }else{
                finish();
                Intent credit = new Intent(getApplicationContext(), LoginActivity.class);
                startActivity(credit);
                return;
            }
        } else {
                finish();
                Intent ipSet = new Intent(getApplicationContext(), LoginActivity.class);
                startActivity(ipSet);
                return;
        }

        AlarmManager alarmManager = (AlarmManager) getSystemService(Context.ALARM_SERVICE);
        Intent i = new Intent(this, ServiceRunCheckReceiver.class); // explicit
        // intent
        PendingIntent intentExecuted = PendingIntent.getBroadcast(this, 0, i, PendingIntent.FLAG_CANCEL_CURRENT);
        Calendar now = Calendar.getInstance();
        now.add(Calendar.SECOND, 20);
        alarmManager.cancel(intentExecuted);
        alarmManager.setRepeating(AlarmManager.RTC_WAKEUP, now.getTimeInMillis(), 60000, intentExecuted);
    }
}
