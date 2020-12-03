package com.example.milanchinthaka.bbcallbutton_android_tablet;

import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.support.v4.app.NotificationCompat;
import android.support.v4.app.TaskStackBuilder;

import java.util.HashMap;

/**
 * Created by Milan Chinthaka on 2/15/2018.
 */

public class Notifications {
    static int notificationID=11;

    public static HashMap<String, CallNotify> map = new HashMap<>();

    public static void displayNotification(Context context, Call call){
//        notificationID++;
        map.put(call.uniqueId, new CallNotify(notificationID, call));

        NotificationCompat.Builder mBuilder = new NotificationCompat.Builder(context);

        //byte[] decodedString = Base64.decode(enroll.getImageString(), Base64.DEFAULT);
        ///Bitmap decodedByte = BitmapFactory.decodeByteArray(decodedString, 0, decodedString.length);

        mBuilder.setContentTitle("You have " + map.size() + " Calls");
        mBuilder.setContentText("Call received: " + call.roomNumber);
        mBuilder.setAutoCancel(true);
        mBuilder.setDefaults(Notification.DEFAULT_LIGHTS | Notification.DEFAULT_VIBRATE);
        mBuilder.setSmallIcon(R.mipmap.ic_logo);

        mBuilder.setVisibility(Notification.VISIBILITY_PUBLIC);

        //mBuilder.setLargeIcon(decodedByte);
        mBuilder.setOngoing(true);

        Intent resultIntent = new Intent(context, MainActivity.class);
        resultIntent.setFlags(Intent.FLAG_ACTIVITY_SINGLE_TOP | Intent.FLAG_ACTIVITY_CLEAR_TOP);

        //pass data from entity
//        resultIntent.putExtra("pop", "true");
//        resultIntent.putExtra("call", call);
        //resultIntent.putExtra("empid", TCPService.Id);

        TaskStackBuilder stackBuilder = TaskStackBuilder.create(context);
        stackBuilder.addParentStack(MainActivity.class);

        //Adds the Intent that starts the Activity to the top of the stack

        stackBuilder.addNextIntent(resultIntent);

        //if service restart cotinuesly,i think, we must use big random int as below NotificationID
        PendingIntent resultPendingIntent =stackBuilder.getPendingIntent(notificationID, PendingIntent.FLAG_UPDATE_CURRENT);

        mBuilder.setContentIntent(resultPendingIntent);
        NotificationManager mNotificationManager = (NotificationManager) context.getSystemService(Context.NOTIFICATION_SERVICE);

        if(TCPService.toneURI != null){
            mBuilder.setSound(TCPService.toneURI);
        }else{
            mBuilder.setDefaults(Notification.DEFAULT_ALL);
        }

        mBuilder.mNotification.flags |= Notification.FLAG_NO_CLEAR;
        mNotificationManager.notify(notificationID, mBuilder.build());
    }

    public static int auth_notificationID=1000;
    public static void authenticationFailureNotification(Context context){

        NotificationCompat.Builder mBuilder = new NotificationCompat.Builder(context);

        //byte[] decodedString = Base64.decode(enroll.getImageString(), Base64.DEFAULT);
        ///Bitmap decodedByte = BitmapFactory.decodeByteArray(decodedString, 0, decodedString.length);

        mBuilder.setContentTitle("Authentication Failed!");
        mBuilder.setContentText("Incorrect Username or Password");
        mBuilder.setAutoCancel(true);
        mBuilder.setDefaults(Notification.DEFAULT_LIGHTS | Notification.DEFAULT_VIBRATE);
        mBuilder.setSmallIcon(R.mipmap.ic_logo);
        mBuilder.setVisibility(Notification.VISIBILITY_PUBLIC);

        //mBuilder.setLargeIcon(decodedByte);
        mBuilder.setOngoing(true);

        Intent resultIntent = new Intent(context, LoginActivity.class);
        resultIntent.setFlags(Intent.FLAG_ACTIVITY_SINGLE_TOP | Intent.FLAG_ACTIVITY_CLEAR_TOP);

        TaskStackBuilder stackBuilder = TaskStackBuilder.create(context);
        stackBuilder.addParentStack(MainActivity.class);

        //Adds the Intent that starts the Activity to the top of the stack

        stackBuilder.addNextIntent(resultIntent);

        //if service restart cotinuesly,i think, we must use big random int as below NotificationID
        PendingIntent resultPendingIntent =stackBuilder.getPendingIntent(auth_notificationID, PendingIntent.FLAG_UPDATE_CURRENT);

        mBuilder.setContentIntent(resultPendingIntent);
        NotificationManager mNotificationManager = (NotificationManager) context.getSystemService(Context.NOTIFICATION_SERVICE);

        if(TCPService.toneURI != null){
            mBuilder.setSound(TCPService.toneURI);
        }else{
            mBuilder.setDefaults(Notification.DEFAULT_ALL);
        }

        mBuilder.mNotification.flags |= Notification.FLAG_NO_CLEAR;
        mNotificationManager.notify(auth_notificationID, mBuilder.build());
    }

    public static int connected_notificationID=2000;
    public static void connectionFailureNotification(Context context){

        NotificationCompat.Builder mBuilder = new NotificationCompat.Builder(context);

        //byte[] decodedString = Base64.decode(enroll.getImageString(), Base64.DEFAULT);
        ///Bitmap decodedByte = BitmapFactory.decodeByteArray(decodedString, 0, decodedString.length);

        mBuilder.setContentTitle("Connection Failed!");
        mBuilder.setContentText("Connection failed with server");
        mBuilder.setAutoCancel(true);
        mBuilder.setDefaults(Notification.DEFAULT_LIGHTS | Notification.DEFAULT_VIBRATE);
        mBuilder.setSmallIcon(R.mipmap.ic_logo);
        mBuilder.setVisibility(Notification.VISIBILITY_PUBLIC);

        //mBuilder.setLargeIcon(decodedByte);
        mBuilder.setOngoing(true);

        Intent resultIntent = new Intent(context, LoginActivity.class);
        resultIntent.setFlags(Intent.FLAG_ACTIVITY_SINGLE_TOP | Intent.FLAG_ACTIVITY_CLEAR_TOP);

//        resultIntent.putExtra("pop", "true");
//        resultIntent.putExtra("pop", "true");

        TaskStackBuilder stackBuilder = TaskStackBuilder.create(context);
        stackBuilder.addParentStack(MainActivity.class);

        //Adds the Intent that starts the Activity to the top of the stack

        stackBuilder.addNextIntent(resultIntent);

        //if service restart cotinuesly,i think, we must use big random int as below NotificationID
        PendingIntent resultPendingIntent =stackBuilder.getPendingIntent(connected_notificationID, PendingIntent.FLAG_UPDATE_CURRENT);

        mBuilder.setContentIntent(resultPendingIntent);
        NotificationManager mNotificationManager = (NotificationManager) context.getSystemService(Context.NOTIFICATION_SERVICE);

        if(TCPService.toneURI != null){
            mBuilder.setSound(TCPService.toneURI);
        }else{
            mBuilder.setDefaults(Notification.DEFAULT_ALL);
        }

        mBuilder.mNotification.flags |= Notification.FLAG_NO_CLEAR;
        mNotificationManager.notify(connected_notificationID, mBuilder.build());
    }
}
