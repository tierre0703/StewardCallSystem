package com.example.milanchinthaka.bbcallbutton_android;

import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import java.text.DateFormat;
import android.icu.text.SimpleDateFormat;

import java.util.Calendar;
import java.util.Date;
import java.util.Locale;

/**
 * Created by Milan Chinthaka on 3/16/2018.
 */

public class Database {

    private static  Database instance = new Database();
    public SQLiteDatabase callDB;

    private Database(){

    }

    public void initDatabase(){
        callDB.execSQL("CREATE TABLE IF NOT EXISTS call(id INTEGER PRIMARY KEY AUTOINCREMENT, room_number VARCHAR, " +
                "uniqueId VARCHAR, date_time DATETIME DEFAULT CURRENT_TIMESTAMP, user_action VARCHAR);");
    }

    public void insertCall(Call call){
        DateFormat format = DateFormat.getDateTimeInstance(DateFormat.MEDIUM, DateFormat.MEDIUM);
        callDB.execSQL("INSERT INTO call (room_number, uniqueId, user_action) VALUES('"+ call.roomNumber+"','"+call.uniqueId+"','" +call.userAction+"'" +
                ");");
    }

    public void updateCall(Call call){
        try {
            callDB.execSQL("UPDATE call SET user_action='" + call.userAction + "' WHERE uniqueId='" + call.uniqueId + "';");
        }catch (Exception e){
//            System.out.println("EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            e.printStackTrace();
        }
    }

    public void updateCall(String uniqueId, String userAction){
        callDB.execSQL("UPDATE call SET user_action='"+userAction+ "' WHERE uniqueId='"+uniqueId+"';");
    }

    public void deleteCalls(){
//        DateFormat format = DateFormat.getDateTimeInstance(DateFormat.MEDIUM, DateFormat.MEDIUM);
//        Calendar cal = Calendar.getInstance();
//        cal.add(Calendar.DATE, -30);
//        String date = format.format(cal.getTime());
        callDB.execSQL("DELETE FROM call WHERE date_time NOT BETWEEN datetime('now', '-30 day') AND datetime('now', 'localtime');");
    }

    public Cursor getCalls(){
        Cursor resultSet = callDB.rawQuery("Select * from call ORDER BY id DESC",null);
        return  resultSet;
    }

    public static Database getInstance() {
        return instance;
    }
}
