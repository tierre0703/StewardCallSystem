package com.example.milanchinthaka.bbcallbutton_android;

import android.app.Activity;
import android.database.Cursor;
import android.graphics.drawable.ColorDrawable;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.wearable.view.WatchViewStub;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.TextView;

import java.util.Vector;

public class CallListActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_call_list);

        final WatchViewStub stub = (WatchViewStub) findViewById(R.id.watch_view_stub_call_list);
//        stub.setOnLayoutInflatedListener(new WatchViewStub.OnLayoutInflatedListener() {
//            @Override
//            public void onLayoutInflated(WatchViewStub stub) {
//                final ListView callListVw = (ListView) stub.findViewById(R.id.callListView);

//                Cursor cursor = Database.getInstance().getCalls();
//                Vector<String> calls=new Vector<String>();
//                final Vector<String> callActions=new Vector<String>();
//                while (cursor.moveToNext()){
//                    String room = cursor.getString(1);
//                    String datetime = cursor.getString(3);
//                    String action = cursor.getString(4);
//                    //CallView call = new CallView(room, datetime);
//                    calls.add(room + "\n" + datetime);
//                    callActions.add(action);
//                }



//                ArrayAdapter<Object> adapter = new ArrayAdapter<Object>(CallListActivity.this, android.R.layout.simple_list_item_1, calls.toArray()){
//
//                    @Override
//                    public View getView(int position, View convertView, ViewGroup parent){
//                        // Get the Item from ListView
//                        View view = super.getView(position, convertView, parent);
//
//                        // Initialize a TextView for ListView each Item
//                        TextView tv = (TextView) view.findViewById(android.R.id.text1);
//
//                        // Set the text color of TextView (ListView Item)
//
//                        ColorDrawable cd = new ColorDrawable();
//
//                        String action = callActions.get(position);
//
//                        if(action != null) {
//                            if (action.equals("A")) {
//                                cd.setColor(getResources().getColor(R.color.lightbluebacklist));
//                                tv.setTextColor(getResources().getColor(R.color.whiteText));
//                            } else if (action.equals("R")) {
//                                cd.setColor(getResources().getColor(R.color.redlight));
//                                tv.setTextColor(getResources().getColor(R.color.whiteText));
//                            } else if (action.equals("P")) {
//                                cd.setColor(getResources().getColor(R.color.yellowlight));
//                                tv.setTextColor(getResources().getColor(R.color.textBlue));
//                            }
//                        }else{
//                            cd.setColor(getResources().getColor(R.color.yellowlight));
//                            tv.setTextColor(getResources().getColor(R.color.textBlue));
//
//                        }
//
//                        tv.setBackground(cd);
//
//                        // Generate ListView Item using TextView
//                        return view;
//                    }
//                };
//                callListVw.setAdapter(adapter);
//            }});


    }
}
