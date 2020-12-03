package com.example.milanchinthaka.bbcallbutton_android;

import android.content.Intent;
import android.content.SharedPreferences;
import android.database.Cursor;
import android.database.DataSetObserver;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageButton;
import android.widget.ListAdapter;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import java.util.Vector;

public class CallListActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_call_list);

        final ListView callListVw = (ListView) findViewById(R.id.callListView);

        final TextView nameTxt = (TextView) findViewById(R.id.txtName);

        ImageButton btnHistory = (ImageButton) findViewById(R.id.historyBtnHistory);
        ImageButton settingbtn = (ImageButton) findViewById(R.id.btnSettingsHistory);
        ImageButton configbtn = (ImageButton) findViewById(R.id.configBtnHistory);

        configbtn.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent credit = new Intent(getApplicationContext(), ConfigActivity.class);
                startActivity(credit);

                finish();
            }

        });

        settingbtn.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent credit = new Intent(getApplicationContext(), LoginActivity.class);
                startActivity(credit);

                finish();
            }

        });

        btnHistory.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent credit = new Intent(getApplicationContext(), CallListActivity.class);
                startActivity(credit);

                finish();
            }

        });

        SharedPreferences prefs = getSharedPreferences("MY_PREF", MODE_PRIVATE);
        String empname = prefs.getString("emp_name", null);

        if (empname != null){
            nameTxt.setText(empname);
        }

        Cursor cursor = Database.getInstance().getCalls();
        Vector<String> calls=new Vector<String>();
        final Vector<String> callActions=new Vector<String>();
        while (cursor.moveToNext()){
            String room = cursor.getString(1);
            String datetime = cursor.getString(3);
            String action = cursor.getString(4);
            //CallView call = new CallView(room, datetime);
            calls.add(room + "\n" + datetime);
            callActions.add(action);
        }

        ArrayAdapter<Object> adapter = new ArrayAdapter<Object>(this,
                android.R.layout.simple_list_item_1, calls.toArray()){

            @Override
            public View getView(int position, View convertView, ViewGroup parent){
                // Get the Item from ListView
                View view = super.getView(position, convertView, parent);

                // Initialize a TextView for ListView each Item
                TextView tv = (TextView) view.findViewById(android.R.id.text1);

                // Set the text color of TextView (ListView Item)

                ColorDrawable cd = new ColorDrawable();

                String action = callActions.get(position);

                if(action != null) {
                    if (action.equals("A")) {
                        cd.setColor(getResources().getColor(R.color.yellowlight));
                        tv.setTextColor(getResources().getColor(R.color.textBlue));
                    } else if (action.equals("R")) {
                        cd.setColor(getResources().getColor(R.color.lightbluebacklist));
                        tv.setTextColor(getResources().getColor(R.color.whiteText));
                    } else if (action.equals("P")) {
                        cd.setColor(getResources().getColor(R.color.redlight));
                        tv.setTextColor(getResources().getColor(R.color.whiteText));
                    }
                }else{
                    cd.setColor(getResources().getColor(R.color.redlight));
                    tv.setTextColor(getResources().getColor(R.color.whiteText));

                }

                tv.setBackground(cd);

                // Generate ListView Item using TextView
                return view;
            }
        };
        callListVw.setAdapter(adapter);
    }


}
