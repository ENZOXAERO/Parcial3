package com.example.parcial3;

import androidx.appcompat.app.AppCompatActivity;

import android.app.ProgressDialog;
import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ListAdapter;
import android.widget.ListView;
import android.widget.SimpleAdapter;

import java.util.ArrayList;
import java.util.HashMap;

public class incident_list_update extends AppCompatActivity {

    private ListView listView;

    ArrayList<HashMap<String,String>> incidentList = new ArrayList<>();

    ProgressDialog progressDialog;

    ListAdapter listAdapter;

    Integer code;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LOCKED);
        setContentView(R.layout.activity_incident_list_update);

        listView = findViewById(R.id.livIncidents);

        Intent intent = getIntent();
        code = Integer.parseInt(intent.getStringExtra("code"));

        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                HashMap<String,String> code = (HashMap)(listView.getItemAtPosition(i));
                //updateIncident(code.get("id"));
            }
        });

        new getIncident().execute();
    }

    private class getIncident extends AsyncTask<Void,Void,Void> {

        @Override
        protected void onPostExecute(Void unused) {
            super.onPostExecute(unused);
            if(progressDialog.isShowing())
                progressDialog.dismiss();

            listAdapter = new SimpleAdapter(incident_list_update.this, incidentList, R.layout.item, new String[]{"name"}, new int[]{R.id.incidents});
            listView.setAdapter(listAdapter);
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            progressDialog = new ProgressDialog(incident_list_update.this);
            progressDialog.setMessage("Loading...");
            progressDialog.setCancelable(false);
            progressDialog.show();
        }

        @Override
        protected Void doInBackground(Void... voids) {
            Service service = new Service(incident_list_update.this);
            incidentList = service.getIncident(code);
            return null;

        }
    }
}