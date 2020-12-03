package com.example.milanchinthaka.bbcallbutton_android;

import android.app.AlarmManager;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

import java.util.Calendar;

/**
 * Created by Milan Chinthaka on 3/20/2018.
 */

public class AutoStart extends BroadcastReceiver {

    private static final int EXEC_INTERVAL = 20 * 1000;

    @Override
    public void onReceive(Context context, Intent intent) {
        Intent intent3 = new Intent(context,TCPService.class);
        context.startService(intent3);


    }
}
