package com.example.milanchinthaka.bbcallbutton_android_tablet;

import android.app.IntentService;
import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.database.sqlite.SQLiteDatabase;
import android.net.Uri;
import android.os.Handler;
import android.os.IBinder;
import android.support.annotation.Nullable;
import android.support.v4.app.NotificationCompat;
import android.support.v4.app.TaskStackBuilder;

import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.util.Date;
import java.util.Iterator;
import java.util.Map;


public class TCPService extends IntentService {

    //private BufferedReader reader;		// to read from the socket
    private PrintWriter writer;		// to write on the socket
    private Socket socket;
    private InputStreamReader isr;
    public static TCPService instance;

    public static String user;
    public static String password;

    public static  Uri toneURI;

    public  boolean connected;

    public boolean authenticated;

    public static boolean externalStart;
    public static boolean forcedStop;

    public static boolean runningReaderThread;

    //Runnable runnableCode = null;
    //Handler handler = new Handler();

//    ConnectionTask connectionTask;

    //public static String Id = null;

    public static String server = "192.168.8.102";
    private int port = 7777;

    final Handler handler = new Handler();
    Runnable runnable = new Runnable() {

        @Override
        public void run() {
            try{
                Date now = new Date();
                new Thread(new Runnable() {
                    @Override
                    public void run() {
                        send("MPING:");
                    }
                }).start();

                if (socket != null && socket.isConnected()){
                    if (!MainActivity.instance.nameTxt.getText().toString().endsWith(" : Connected")){
                        SharedPreferences prefs = getSharedPreferences("MY_PREF", MODE_PRIVATE);
                        String empname = prefs.getString("emp_name", null);
                        MainActivity.instance.nameTxt.setText(empname + " : Connected");

//                        MainActivity.pressPower = false;
                    }
                }else{

                    System.out.println("Server has close the connection");
                    disconnect();

//                    externalStart = true;
//                    ConnectionHandler t = new ConnectionHandler(false);
//                    t.start();

                    reconnectionHandler.postDelayed(reconnectionRunnable, 5000);

                    if (!MainActivity.instance.nameTxt.getText().toString().endsWith(" : Disconnected")) {
                        SharedPreferences prefs = getSharedPreferences("MY_PREF", MODE_PRIVATE);
                        String empname = prefs.getString("emp_name", null);
                        MainActivity.instance.nameTxt.setText(empname + " : Disconnected");
                    }
                }

                for (Iterator<Map.Entry<String, CallNotify>> it = Notifications.map.entrySet().iterator(); it.hasNext(); ) {
                    SharedPreferences prefs = getSharedPreferences("MY_PREF", MODE_PRIVATE);
                    int callIntval = prefs.getInt("callintval", -1);
                    if (callIntval < 0){
                        callIntval = 30000;
                    }else{
                        callIntval*=1000;
                    }
                    final Call call = it.next().getValue().call;
                        if ((now.getTime() - call.datetime.getTime()) > callIntval) {

                            Notifications.displayNotification(TCPService.this, call);

                            if (MainActivity.instance != null && MainActivity.isVisible) {
                                MainActivity.instance.runOnUiThread(new Runnable() {
                                    @Override
                                    public void run() {
//                                    MainActivity.instance.setCall(call);
                                        MainActivity.instance.removeCallWidget(call);
                                        MainActivity.instance.addNewCallLayout(call);
                                    }
                                });
                            }
                            call.datetime = now;
                        } else {
                            continue;
                        }
                    }

            }
            catch (Exception e) {
                // TODO: handle exception
                e.printStackTrace();
            }
            finally{
                //also call the same runnable to call it at regular interval
                handler.postDelayed(this, 3000);
            }
        }
    };

    final Handler reconnectionHandler = new Handler();
    Runnable reconnectionRunnable = new Runnable() {

        @Override
        public void run() {
            try{
                externalStart = false;
                ConnectionHandler t = new ConnectionHandler(false);
                t.start();

            }
            catch (Exception e) {
                // TODO: handle exception
                e.printStackTrace();
            }
            finally{
                //also call the same runnable to call it at regular interval
                reconnectionHandler.removeCallbacksAndMessages(null);
                reconnectionHandler.postDelayed(this, 5000);
            }
        }
    };

    public TCPService() {
        super("TCPService");
    }

    public void stopAllServices(){
        handler.removeCallbacksAndMessages(null);
        reconnectionHandler.removeCallbacksAndMessages(null);
        disconnect();
    }

    @Override
    protected void onHandleIntent(Intent intent) {
        if (intent != null) {
            final String action = intent.getAction();

        }
    }

    @Override
    public int onStartCommand(@Nullable Intent intent, int flags, int startId) {
        instance = this;

//        System.out.println("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR  - 1");
        // try to connect to the server
        //final Intent intentF = intent;

//        final File ipAddress = new File(getFilesDir(),"IPAddress.txt");
        //final File credential = new File(getFilesDir(),"Credential.txt");
        //Toast.makeText(getApplicationContext(), "HHHHHHHHHHHHHHH", Toast.LENGTH_SHORT);

//        forcedStop = intent.getBooleanExtra("forcedStop", false);
//        externalStart = intent.getBooleanExtra("externalStart", true);

        SQLiteDatabase callDB = openOrCreateDatabase("call_db",MODE_PRIVATE,null);
        Database.getInstance().callDB = callDB;
        Database.getInstance().initDatabase();

//        System.out.println("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR  - 2");

        SharedPreferences prefs = getSharedPreferences("MY_PREF", MODE_PRIVATE);
        String ip = prefs.getString("ipaddr", null);
        String uname = prefs.getString("username", null);
        String pword = prefs.getString("password", null);

//        System.out.println("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR  - 3");

        String tone = prefs.getString("tone", null);
        if(tone != null){
            toneURI = Uri.parse(tone);
        }

//        System.out.println("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR  - 4");

        final String power = prefs.getString("power", null);
        if(power == null || power.equals("off")){
//            System.out.println("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR  - 5");
        }else {
            handler.postDelayed(runnable, 3000);

//            System.out.println("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR  - 6");
            if (ip != null) {
                server = ip;
            } else {
                Notifications.connectionFailureNotification(TCPService.this);
                return START_STICKY;
            }

            if (uname != null && pword != null) {
                user = uname;
                password = pword;
            } else {
                Notifications.authenticationFailureNotification(TCPService.this);
                return START_STICKY;
            }


//            System.out.println("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR  - 7");
            ConnectionHandler t = new ConnectionHandler(true);
            t.start();
//            System.out.println("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR  - 8");
        }

//        Bitmap icon = BitmapFactory.decodeResource(getResources(),
//                R.drawable.truiton_short);
//        System.out.println("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR  - 9");
        Intent resultIntent = new Intent(getApplicationContext(), MainActivity.class);
        resultIntent.setFlags(Intent.FLAG_ACTIVITY_SINGLE_TOP | Intent.FLAG_ACTIVITY_CLEAR_TOP);

        TaskStackBuilder stackBuilder = TaskStackBuilder.create(getApplicationContext());
        stackBuilder.addParentStack(MainActivity.class);

        //Adds the Intent that starts the Activity to the top of the stack

        stackBuilder.addNextIntent(resultIntent);

        //if service restart cotinuesly,i think, we must use big random int as below NotificationID
        PendingIntent resultPendingIntent =stackBuilder.getPendingIntent(1222, PendingIntent.FLAG_UPDATE_CURRENT);

        Notification notification = new NotificationCompat.Builder(this)
                .setContentTitle("Steward Call System")

                .setContentText("Running!")
                .setSmallIcon(R.mipmap.ic_launcher)
//                .setLargeIcon(
//                        Bitmap.createScaledBitmap(icon, 128, 128, false))
                .setContentIntent(resultPendingIntent)
                .setOngoing(true)
                .build();

        startForeground(1333, notification);

//        System.out.println("RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR  - 10");
//        runnableCode = new Runnable() {
//            @Override
//            public void run() {
//                // Do something here on the main thread
//                connectionTask = new ConnectionTask();
//                connectionTask.execute();
//                Log.d("Handlers", "Called on main thread");
//                handler.postDelayed(runnableCode, 5000);
//                // Repeat this the same runnable code block again another 2 second
//            }
//        };
//
//        handler.post(runnableCode);

        return START_STICKY;
    }


    @Override
    public void onDestroy() {
        Database.getInstance().callDB.close();
        handler.removeCallbacksAndMessages(null);
        reconnectionHandler.removeCallbacksAndMessages(null);
        disconnect();
        stopForeground(true);
        super.onDestroy();
//        MainActivity.pressPower = false;
    }

    public void disconnect() {
        runningReaderThread = false;
        try {
            if(isr != null) isr.close();
        }
        catch(Exception e) {} // not much else I can do
        try {
            if(writer != null) writer.close();
        }
        catch(Exception e) {} // not much else I can do
        try{
            if(socket != null) socket.close();
        }
        catch(Exception e) {} // not much else I can do
    }

    public void send(String msg){
        if(writer != null) {
            writer.print(msg);
            writer.flush();
        }
    }

    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

//    public class ConnectionTask extends AsyncTask<Void, Void, String> {
//
//        public ConnectionTask(){
//
//        }
//        @Override
//        protected String doInBackground(Void... params) {
//            System.out.println("############################################");
//            try {
//                send("PING");
//            }catch (Exception e){
//                System.out.println("EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
//            }
//            return null;
//        }
//    }

    class ConnectionHandler extends Thread{

        private boolean isFirstTime;

        ConnectionHandler (boolean isft){
            externalStart = isft;
            Database.getInstance().deleteCalls();
        }
        @Override
        public void run() {
            try {
                //server = intentF.getExtras().get("IP").toString();
                socket = new Socket();
                socket.connect(new InetSocketAddress(server, port), 2000);
                String msg = "Connection accepted " + socket.getInetAddress() + ":" + socket.getPort();
                System.out.println(msg);

		/* Creating both Data Stream */
                isr = new InputStreamReader(socket.getInputStream());
                //reader  = new BufferedReader(isr);
                writer = new PrintWriter(socket.getOutputStream(), true);

                new ListenFromServer().start();

                send(CommunicationStrings.NEW_CONNECTION);

                connected= true;
                reconnectionHandler.removeCallbacksAndMessages(null);
                // Send our username to the server this
            }
            catch (IOException eIO) {
                //Toast.makeText(getApplicationContext(), "Couldn't establish the connection with server", Toast.LENGTH_SHORT);
                connected= false;
                if(MainActivity.instance != null) {
                    MainActivity.instance.runOnUiThread(new Runnable() {
                        @Override
                        public void run() {
                            SharedPreferences prefs = getSharedPreferences("MY_PREF", MODE_PRIVATE);
                            String empname = prefs.getString("emp_name", null);
                            MainActivity.instance.nameTxt.setText(empname + " : Disconnected");
                        }
                    });
                }
                if(externalStart){
                    externalStart=true;

                    Notifications.connectionFailureNotification(TCPService.this);
                }
                System.out.println("Exception creating new Input/output Streams: " + eIO);
                if(!forcedStop) {
//                    ConnectionHandler t = new ConnectionHandler(false);
//                    t.start();
                    reconnectionHandler.removeCallbacksAndMessages(null);
                    reconnectionHandler.postDelayed(reconnectionRunnable, 5000);
                }
                return;
            }finally {

            }
        }
    }

    class ListenFromServer extends Thread {


        ListenFromServer(){
            //Toast.makeText(getApplicationContext(), "Server has close the connection: " , Toast.LENGTH_SHORT);
        }
        public void run() {
            authenticated = false;
            runningReaderThread = true;
            while(runningReaderThread) {
                try {
                    char[] array = new char[100];
                    final int size = isr.read(array) ;
                    if (size == 0){
                        continue;
                    }
                    if (size < 0){
                        throw new IOException();
                    }
                    final String msg = String.valueOf(array, 0, size);
                    System.out.println("SIZE - " + size + " Message - " + msg);

                    if(msg.contains("CONESTB:")){
                        send(CommunicationStrings.AUTHENTICATION + "U-" + user + " P-" + password + ":");
                        NotificationManager notificationManager = (NotificationManager) getApplicationContext().getSystemService(Context.NOTIFICATION_SERVICE);
                        notificationManager.cancel(Notifications.connected_notificationID);
                    }else if(msg.contains("AUTH_Y")) {
//                        if(MainActivity.instance != null) {
//                            MainActivity.instance.runOnUiThread(new Runnable() {
//                                @Override
//                                public void run() {
//                                    MainActivity.instance.headerTxt.setText("You are now connected");
//                                }
//                            });
//                        }
                        NotificationManager notificationManager = (NotificationManager) getApplicationContext().getSystemService(Context.NOTIFICATION_SERVICE);
                        notificationManager.cancel(Notifications.auth_notificationID);
                    }else if(msg.contains("AUTH_N")) {
                        authenticated = false;
                        Notifications.authenticationFailureNotification(TCPService.this);

                    }else if(msg.contains("ENAME:")) {
                        authenticated = true;

                        int index = msg.indexOf("ENAME:");
                        index = (index < 0) ? 0:index;
                        final String [] details = msg.substring(index + "ENAME:".length()).split(":");

                        SharedPreferences.Editor editor = getSharedPreferences("MY_PREF", MODE_PRIVATE).edit();
                        editor.putString("emp_id", details[0]);
                        editor.putString("emp_name", details[1]);
                        editor.commit();

                        if(MainActivity.instance != null) {
                            MainActivity.instance.runOnUiThread(new Runnable() {
                                @Override
                                public void run() {
                                    MainActivity.instance.nameTxt.setText(details[1] + " : Connected");
                                }
                            });
                        }
                    }else if(msg.contains("CALL:")) {
                        int index = msg.indexOf("CALL:");
                        index = (index < 0) ? 0:index;
                        final String[] callArr = msg.substring(index + "CALL:".length()).split(":");
                        final Call call = new Call(callArr[0], callArr[1], new Date(), "P");

                        boolean found = false;
                        for (Iterator<Map.Entry<String, CallNotify>> it = Notifications.map.entrySet().iterator(); it.hasNext(); ) {
                            final Call oldCall = it.next().getValue().call;
                            if (call.roomNumber.equals(oldCall.roomNumber)) {
                                if ((call.datetime.getTime() - oldCall.datetime.getTime()) > 60000) {
                                    if (MainActivity.instance != null && MainActivity.isVisible) {

                                        MainActivity.instance.runOnUiThread(new Runnable() {
                                            @Override
                                            public void run() {
                                                MainActivity.instance.setCallCancel(oldCall);
                                            }
                                        });
                                    }
                                    it.remove();
                                } else {
                                    found = true;
                                    break;
                                }
                            }
                        }
                        if (!found) {
                            Notifications.displayNotification(TCPService.this, call);
                            Database.getInstance().insertCall(call);

                            //System.out.println("MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM");
                            System.out.println(MainActivity.instance);
                            System.out.println(MainActivity.isVisible);
                            //System.out.println("MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM");
                            if (MainActivity.instance != null && MainActivity.isVisible) {

                                MainActivity.instance.runOnUiThread(new Runnable() {
                                    @Override
                                    public void run() {
//                                    MainActivity.instance.setCall(call);
                                        MainActivity.instance.addNewCallLayout(call);
                                        MainActivity.instance.addCallHistory(call);
                                    }
                                });
                            }
                        }
                    }else if(msg.contains("RECALL:")) {
                        int index = msg.indexOf("RECALL:");
                        index = (index < 0) ? 0:index;
                        String [] callArr = msg.substring(index + "RECALL:".length()).split(":");
                        final Call call =new Call(callArr[0], callArr[1], new Date(), "P");

                        boolean ok = false;
                        if(Notifications.map.containsKey(call.uniqueId)){
                            Call oldCall = Notifications.map.get(call.uniqueId).call;
                            if ((call.datetime.getTime() - oldCall.datetime.getTime()) > 30000) {
                                ok = true;
                            }
                            Notifications.map.remove(call.uniqueId);
                        }

                        if (ok) {
                            Notifications.displayNotification(TCPService.this, call);
                            Database.getInstance().insertCall(call);

                            if (MainActivity.instance != null && MainActivity.isVisible) {
                                MainActivity.instance.runOnUiThread(new Runnable() {
                                    @Override
                                    public void run() {
//                                    MainActivity.instance.setCall(call);
                                        MainActivity.instance.setCallCancel(call);
                                        MainActivity.instance.addNewCallLayout(call);
                                        MainActivity.instance.addCallHistory(call);
                                    }
                                });
                            }
                        }
                    }else if(msg.contains("APPROVED:")){
                        int index = msg.indexOf("APPROVED:");
                        index = (index < 0) ? 0:index;
                        final String[] uniqueId = msg.substring(index + "APPROVED:".length()).split(":");
                        if(MainActivity.instance != null){
                            MainActivity.instance.runOnUiThread(new Runnable() {
                                @Override
                                public void run() {
                                    if(MainActivity.instance != null) {
                                        //Toast.makeText(getApplicationContext(), msg, Toast.LENGTH_SHORT);
                                        MainActivity.instance.setCallAccept(uniqueId[0]);
                                    }
                                }
                            });
                        }
                    }else if(msg.contains("REJECTED:")){
                        int index = msg.indexOf("REJECTED:");
                        index = (index < 0) ? 0:index;
                        final String[] uniqueId = msg.substring(index + "REJECTED:".length()).split(":");
//                        Toast.makeText(getApplicationContext(), "The call is already accepted!", Toast.LENGTH_SHORT);
//                        if(MainActivity.instance != null){
//                            MainActivity.instance.finish();
//                        }
                        if(Notifications.map.get(uniqueId[0]) != null) {
                            final Call call = Notifications.map.get(uniqueId[0]).call;
                            if (MainActivity.instance != null && MainActivity.isVisible) {

                                MainActivity.instance.runOnUiThread(new Runnable() {
                                    @Override
                                    public void run() {
                                        //Toast.makeText(getApplicationContext(), msg, Toast.LENGTH_SHORT);
                                        MainActivity.instance.setCallCancel(call);
                                    }
                                });
                            }

                            Database.getInstance().updateCall(uniqueId[0], "R");
                        }
                    }else if(msg.contains("CANCELLED:")) {
                        //System.out.println("VVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVVV");
                        int index = msg.indexOf("CANCELLED:");
                        index = (index < 0) ? 0:index;
                        final String[] uniqueId = msg.substring(index + "CANCELLED:".length()).split(":");

                        if (Notifications.map.get(uniqueId[0]) != null) {
                            final Call call = Notifications.map.get(uniqueId[0]).call;
                            if (MainActivity.instance != null && MainActivity.isVisible) {

                                MainActivity.instance.runOnUiThread(new Runnable() {
                                    @Override
                                    public void run() {
                                        //Toast.makeText(getApplicationContext(), msg, Toast.LENGTH_SHORT);
                                        MainActivity.instance.setCallCancel(call);
                                    }
                                });
                            }
                        }
                        if(Notifications.map.containsKey(uniqueId[0])){
                            NotificationManager notificationManager = (NotificationManager) getApplicationContext().getSystemService(Context.NOTIFICATION_SERVICE);
                            notificationManager.cancel(Notifications.map.get(uniqueId[0]).notificationID);
                            Notifications.map.remove(uniqueId[0]);
                            Database.getInstance().updateCall(uniqueId[0], "R");
                        }
                    }else if(msg.contains("ERROR:")){
                        if(MainActivity.instance != null){
                            MainActivity.instance.runOnUiThread(new Runnable() {
                                @Override
                                public void run() {
                                    if(MainActivity.instance != null) {
                                        //Toast.makeText(getApplicationContext(), msg, Toast.LENGTH_SHORT);
                                        MainActivity.instance.setErrorLabel();
                                    }
                                }
                            });
                        }
                    }else if(msg.contains("DISCONNECT:")){
                        throw new IOException();
                    }
                    //Toast.makeText(getApplicationContext(), " Message: " + msg, Toast.LENGTH_SHORT);
                }
                catch(IOException e) {
                    connected= false;
                    if(MainActivity.instance != null) {
                        MainActivity.instance.runOnUiThread(new Runnable() {
                            @Override
                            public void run() {
                                SharedPreferences prefs = getSharedPreferences("MY_PREF", MODE_PRIVATE);
                                String empname = prefs.getString("emp_name", null);
                                MainActivity.instance.nameTxt.setText(empname + " : Disconnected");
                            }
                        });
                    }
                    e.printStackTrace();
                    System.out.println("Server has close the connection");
                    disconnect();
                    if(!forcedStop){
//                    ConnectionHandler t = new ConnectionHandler(false);
//                    t.start();
                        reconnectionHandler.removeCallbacksAndMessages(null);
                        reconnectionHandler.postDelayed(reconnectionRunnable, 5000);
                    }
                    break;
                }
            }
        }
    }
}
