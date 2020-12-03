package com.example.milanchinthaka.bbcallbutton_android;

import android.content.Intent;
import android.content.SharedPreferences;
import android.media.RingtoneManager;
import android.net.Uri;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ImageButton;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

public class ConfigActivity extends AppCompatActivity {

    TextView fileNameTxt;

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent intent) {
        // Check which request we're responding to
        if (requestCode == 5) {
            // Make sure the request was successful
            if (resultCode == RESULT_OK) {
                Uri uri = intent.getParcelableExtra(RingtoneManager.EXTRA_RINGTONE_PICKED_URI);
                fileNameTxt.setText(RingtoneManager.getRingtone(getApplicationContext(), uri).getTitle(getApplicationContext()));

                TCPService.toneURI = uri;
                SharedPreferences.Editor editor = getSharedPreferences("MY_PREF", MODE_PRIVATE).edit();
                editor.putString("tone", uri.toString());
                editor.commit();
            }
        }
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_config);

        final ImageButton btnTone = (ImageButton) findViewById(R.id.toneSelectBtn);
        fileNameTxt = (TextView) findViewById(R.id.fileNameTxt);

        ImageButton btnHistory = (ImageButton) findViewById(R.id.historyBtnConfig);
        ImageButton settingbtn = (ImageButton) findViewById(R.id.btnSettingsConfig);
        ImageButton configbtn = (ImageButton) findViewById(R.id.configBtnConfig);
        final Spinner timeIntvalSpinner = (Spinner) findViewById(R.id.timeIntvalSpinner);

        Integer[] items = new Integer[]{5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60};
        ArrayAdapter<Integer> adapter = new ArrayAdapter<>(this, android.R.layout.simple_spinner_dropdown_item, items);
        timeIntvalSpinner.setAdapter(adapter);

        timeIntvalSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                Integer value = (Integer) timeIntvalSpinner.getSelectedItem();
                SharedPreferences.Editor editor = getSharedPreferences("MY_PREF", MODE_PRIVATE).edit();
                editor.putInt("callintval", value);
                editor.commit();
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });

        SharedPreferences prefs = getSharedPreferences("MY_PREF", MODE_PRIVATE);
        int callIntval = prefs.getInt("callintval", -1);

        if (callIntval < 0){
            timeIntvalSpinner.setSelection(0);
        }else{
            int position = adapter.getPosition(callIntval);
            timeIntvalSpinner.setSelection(position);
        }

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

        btnTone.setOnClickListener(new View.OnClickListener() {

            @Override
            public void onClick(View v) {
                Intent intent = new Intent(RingtoneManager.ACTION_RINGTONE_PICKER);
                intent.putExtra(RingtoneManager.EXTRA_RINGTONE_TYPE, RingtoneManager.TYPE_NOTIFICATION);
                intent.putExtra(RingtoneManager.EXTRA_RINGTONE_TITLE, "Select Tone");
                intent.putExtra(RingtoneManager.EXTRA_RINGTONE_EXISTING_URI, (Uri) null);
                startActivityForResult(intent, 5);
            }

        });

        //SharedPreferences prefs = getSharedPreferences("MY_PREF", MODE_PRIVATE);
        String tone = prefs.getString("tone", null);
        if(tone != null){
            fileNameTxt.setText(RingtoneManager.getRingtone(getApplicationContext(), Uri.parse(tone)).getTitle(getApplicationContext()));
        }
    }
}
