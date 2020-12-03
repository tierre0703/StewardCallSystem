package com.example.milanchinthaka.bbcallbutton_android;

/**
 * Created by Milan Chinthaka on 3/16/2018.
 */

public class CallView {

    public String roomNumber;
    public String datetime;

    public CallView(){}

    public CallView(String rn, String dt){
        roomNumber = rn;
        datetime = dt;
    }

    public String getDatetime() {
        return datetime;
    }

    public void setDatetime(String datetime) {
        this.datetime = datetime;
    }

    public String getRoomNumber() {
        return roomNumber;
    }

    public void setRoomNumber(String roomNumber) {
        this.roomNumber = roomNumber;
    }
}
