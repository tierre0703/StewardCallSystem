package com.example.milanchinthaka.bbcallbutton_android;

/**
 * Created by Milan Chinthaka on 3/27/2018.
 */

public class CallNotify {

    public int notificationID;
    public Call call;

    public CallNotify(int notificationID, Call call){
        this.call = call;
        this.notificationID = notificationID;
    }
}
