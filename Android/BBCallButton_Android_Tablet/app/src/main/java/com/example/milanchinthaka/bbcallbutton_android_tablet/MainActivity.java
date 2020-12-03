package com.example.milanchinthaka.bbcallbutton_android_tablet;

import android.app.ActivityManager;
import android.app.AlarmManager;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.SharedPreferences;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.graphics.drawable.ColorDrawable;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;
import java.util.Vector;
import java.util.concurrent.ExecutionException;

public class MainActivity extends AppCompatActivity {

    public TextView nameTxt;
    public LinearLayout newCallLayout;
    public static MainActivity instance;
    public View pendingDecisionView;
    public static boolean isVisible;
    String empid = null;
    String empname = null;
    Vector<Call> callHistory;
    ArrayAdapter<Call> adapter;

    public static boolean pressPower = false;
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
                if (callHistory != null && adapter != null){
                    for (Call c : callHistory){
                        if (c.uniqueId.equals(call.uniqueId)){
                            c.userAction = "A";
                            adapter.notifyDataSetChanged();
                            break;
                        }
                    }
                }
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

            call.userAction = "R";
            adapter.notifyDataSetChanged();

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
//        if (ScreenReceiver.wasScreenOn) {
//            // THIS IS THE CASE WHEN ONPAUSE() IS CALLED BY THE SYSTEM DUE TO A SCREEN STATE CHANGE
//            System.out.println("SCREEN TURNED OFF");
//            finish();
//        } else {
//            // THIS IS WHEN ONPAUSE() IS CALLED WHEN THE SCREEN STATE HAS NOT CHANGED
//        }
        super.onPause();
        finish();
    }

    @Override
    protected void onStop() {
//        if (ScreenReceiver.wasScreenOn) {
//            // THIS IS THE CASE WHEN ONPAUSE() IS CALLED BY THE SYSTEM DUE TO A SCREEN STATE CHANGE
//            System.out.println("SCREEN TURNED OFF");
//            finish();
//        } else {
//            // THIS IS WHEN ONPAUSE() IS CALLED WHEN THE SCREEN STATE HAS NOT CHANGED
//        }
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

    public void addCallHistory(Call call){
        if (callHistory != null && adapter != null){
            callHistory.add(0, call);
            adapter.notifyDataSetChanged();
        }
    }

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

//        IntentFilter filter = new IntentFilter(Intent.ACTION_SCREEN_ON);
//        filter.addAction(Intent.ACTION_SCREEN_OFF);
//        BroadcastReceiver mReceiver = new ScreenReceiver();
//        registerReceiver(mReceiver, filter);

        instance = this;
        nameTxt = (TextView) findViewById(R.id.txtName);
        final ListView callListVw = (ListView) findViewById(R.id.callListView);

        newCallLayout = (LinearLayout) findViewById(R.id.newCallLayouot);
        ImageButton btnHistory = (ImageButton) findViewById(R.id.historyBtn);
        ImageButton settingbtn = (ImageButton) findViewById(R.id.btnSettings);
        ImageButton configbtn = (ImageButton) findViewById(R.id.configBtn);
        final ImageButton powerBtn = (ImageButton) findViewById(R.id.onoffBtn);

//        settingbtn.setOnLongClickListener();

        final SharedPreferences prefs = getSharedPreferences("MY_PREF", MODE_PRIVATE);

        final String power = prefs.getString("power", null);

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

        btnHistory.setImageResource(R.mipmap.home_yellow);
//        btnHistory.setOnClickListener(new View.OnClickListener() {
//
//            @Override
//            public void onClick(View v) {
//                Intent credit = new Intent(getApplicationContext(), CallListActivity.class);
//                startActivity(credit);
//            }
//
//        });

        powerBtn.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
//                if(pressPower){
//                    return;
//                }
//                pressPower = true;
                //final String power = prefs.getString("power", null);
                        powerBtn.setEnabled(false);
//                        powerBtn.postDelayed(new Runnable() {
//                            public void run() {
//                                powerBtn.setEnabled(true);
//                            }
//                        }, 5000);

                if (!powerBool){
                    powerBtn.setImageResource(R.mipmap.ic_power);

                    SharedPreferences.Editor editor = prefs.edit();
                    editor.putString("power", "on");
                    editor.commit();

                    //boolean service = isServiceRunning(TCPService.class);
                    //if (TCPService.instance == null) {
                        Intent intent = new Intent(getApplicationContext(), TCPService.class);
//                    intent.putExtra("forcedStop", false);
//                    intent.putExtra("externalStart", true);
                        //stopService(intent);
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
//                    nameTxt.postDelayed(new Runnable() {
//                        public void run() {
//                            finish();
//                        }
//                    }, 1000);

                    powerBool = false;
                    //System.out.println("GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG");
                    powerBtn.setImageResource(R.mipmap.ic_power_off);
                    SharedPreferences.Editor editor = prefs.edit();
                    editor.putString("power", "off");
                    editor.commit();

                    TCPService.forcedStop = true;
                    //TCPService.instance.stopAllServices();
                    nameTxt.setText(empname + " : Disconnected");
                    Intent intent = new Intent(getApplicationContext(), TCPService.class);
                    stopService(intent);

//                    finish();

                    //pressPower = false;

                }

               //powerBtn.setEnabled(true);

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

        Cursor cursor = Database.getInstance().getCalls();
        callHistory=new Vector<Call>();
        final Vector<String> callActions=new Vector<String>();
        final SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
        while (cursor.moveToNext()){
            String room = cursor.getString(1);
            String uniqueId = cursor.getString(2);
            String datetime = cursor.getString(3);
            String action = cursor.getString(4);
            //CallView call = new CallView(room, datetime);
            Call c = new Call();
            c.roomNumber = room;
            c.userAction = action;
            c.uniqueId = uniqueId;
            try {
                c.datetime = sdf.parse(datetime);
            } catch (ParseException e) {
                e.printStackTrace();
            }
            callHistory.add(c);
            callActions.add(action);
        }

         adapter = new ArrayAdapter<Call>(this, R.layout.list_view_text, callHistory){

            @Override
            public View getView(int position, View convertView, ViewGroup parent){
                // Get the Item from ListView
                View view = convertView;

                if (view == null) // no view to re-use, create new
                    view = MainActivity.this.getLayoutInflater().inflate(R.layout.list_view_text, null);

                // Initialize a TextView for ListView each Item
                TextView tv = (TextView) view.findViewById(android.R.id.text1);
                tv.setEnabled(false);
                tv.setFocusable(false);
                TextView tv2 = (TextView) view.findViewById(android.R.id.text2);
                tv2.setEnabled(false);
                tv2.setFocusable(false);
                // Set the text color of TextView (ListView Item)

                ColorDrawable cd = new ColorDrawable();
                Call call = callHistory.get(position);
                tv.setText(call.roomNumber);
                try{
                    tv2.setText(sdf.format(call.datetime));
                }catch (Exception e){
                    tv2.setText("");
                }
                String action = call.userAction;

                if(action != null) {
                    if (action.equals("A")) {
                        cd.setColor(getResources().getColor(R.color.yellowlight));
                        tv.setTextColor(getResources().getColor(R.color.textBlue));
                        tv2.setTextColor(getResources().getColor(R.color.textBlue));
                    } else if (action.equals("R")) {
                        cd.setColor(getResources().getColor(R.color.lightbluebacklist));
                        tv.setTextColor(getResources().getColor(R.color.whiteText));
                        tv2.setTextColor(getResources().getColor(R.color.whiteText));
                    } else if (action.equals("P")) {
                        cd.setColor(getResources().getColor(R.color.redlight));
                        tv.setTextColor(getResources().getColor(R.color.whiteText));
                        tv2.setTextColor(getResources().getColor(R.color.whiteText));
                    }
                }else{
                    cd.setColor(getResources().getColor(R.color.redlight));
                    tv.setTextColor(getResources().getColor(R.color.whiteText));
                    tv2.setTextColor(getResources().getColor(R.color.whiteText));
                }

                view.setBackground(cd);

                // Generate ListView Item using TextView
                return view;
            }
        };
        callListVw.setAdapter(adapter);
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
                        Intent intent = new Intent(getApplicationContext(), TCPService.class);
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
