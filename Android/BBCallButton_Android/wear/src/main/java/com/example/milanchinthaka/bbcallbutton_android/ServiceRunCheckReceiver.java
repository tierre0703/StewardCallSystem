package com.example.milanchinthaka.bbcallbutton_android;

import android.app.ActivityManager;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

public class ServiceRunCheckReceiver extends BroadcastReceiver {

    @Override
    public void onReceive(Context context, Intent intent) {
        boolean service = isServiceRunning(TCPService.class, context);
        if (service){
            if(TCPService.instance != null && TCPService.instance.connected && TCPService.instance.authenticated){
                System.out.println("MMMMMMMMMMMMMMMMMMMMMMMMM------ connected");
            }else  {
                System.out.println("MMMMMMMMMMMMMMMMMMMMMMMMM------ Starting");
                Intent intent2 = new Intent(context, TCPService.class);
                TCPService.externalStart = true;
                TCPService.forcedStop = false;
                context.startService(intent2);
            }
        }else{
            System.out.println("MMMMMMMMMMMMMMMMMMMMMMMMM------ Starting");
            Intent intent2 = new Intent(context, TCPService.class);
            TCPService.externalStart = true;
            TCPService.forcedStop = false;
            context.startService(intent2);
        }
    }

    private boolean isServiceRunning(Class<?> serviceClass, Context context) {
        ActivityManager manager = (ActivityManager) context.getSystemService(Context.ACTIVITY_SERVICE);
        for (ActivityManager.RunningServiceInfo service : manager.getRunningServices(Integer.MAX_VALUE)) {
            if (serviceClass.getName().equals(service.service.getClassName())) {
                return true;
            }
        }
        return false;
    }

}
