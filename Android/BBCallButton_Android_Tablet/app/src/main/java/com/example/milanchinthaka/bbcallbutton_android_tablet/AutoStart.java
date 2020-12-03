package com.example.milanchinthaka.bbcallbutton_android_tablet;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

/**
 * Created by Milan Chinthaka on 3/20/2018.
 */

public class AutoStart extends BroadcastReceiver {
    @Override
    public void onReceive(Context context, Intent intent) {
        Intent i = new Intent(context,TCPService.class);
        context.startService(i);

    }
}
