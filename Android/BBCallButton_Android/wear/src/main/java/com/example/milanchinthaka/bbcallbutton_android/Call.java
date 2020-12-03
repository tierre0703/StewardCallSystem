package com.example.milanchinthaka.bbcallbutton_android;

import java.io.Serializable;
import java.util.Date;

/**
 * Created by Milan Chinthaka on 2/24/2018.
 */

public class Call implements Serializable{

    public int id;
    public String roomNumber;
    public String uniqueId;
    public Date datetime;
    public String userAction;

    public Call(int id, String uniqueId , String roomNumber, Date dt){
        this.id = id;
        this.roomNumber = roomNumber;
        this.uniqueId = uniqueId;
        this.datetime = dt;
    }

    public Call(String uniqueId , String roomNumber, Date dt, String userAction){
        this.roomNumber = roomNumber;
        this.uniqueId = uniqueId;
        this.datetime = dt;
        this.userAction = userAction;
    }
}
