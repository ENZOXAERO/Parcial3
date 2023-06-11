package com.example.parcial3;

import androidx.appcompat.app.AppCompatActivity;

import android.app.ProgressDialog;
import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.ListAdapter;
import android.widget.ListView;
import android.widget.SimpleAdapter;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.HashMap;

public class incident_list extends AppCompatActivity {

    private ListView listView;

    ArrayList<HashMap<String,String>> incidentList = new ArrayList<>();

    ProgressDialog progressDialog;

    ListAdapter listAdapter;

    private String code;

    private String response;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LOCKED);
        setContentView(R.layout.activity_incident_list);

        Button btnNew = findViewById(R.id.btnNew);
        btnNew.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                createIncident();
            }
        });

        listView = findViewById(R.id.livIncidents);

        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                HashMap<String,String> code = (HashMap)(listView.getItemAtPosition(i));
                updateIncident(code.get("id"));
            }
        });

        listView.setOnItemLongClickListener(new AdapterView.OnItemLongClickListener() {
            @Override
            public boolean onItemLongClick(AdapterView<?> adapterView, View view, int i, long l) {
                HashMap<String,String> data = (HashMap)(listView.getItemAtPosition(i));
                code = data.get("id");
                new deleteIncident().execute();
                return true;
            }
        });

        new getIncidents().execute();
    }

    protected void onRestart() {
        super.onRestart();
        incidentList = new ArrayList<>();
        listAdapter = null;
        listView.setAdapter(null);
        new getIncidents().execute();
    }

    private void createIncident(){
        Intent intent = new Intent(this, create_incident.class);
        startActivity(intent);
    }

    private void updateIncident(String code){
        Intent intent = new Intent(this, update_incident.class);
        intent.putExtra("code", code);
        startActivity(intent);
    }

    private class getIncidents extends AsyncTask<Void,Void,Void> {

        @Override
        protected void onPostExecute(Void unused) {
            super.onPostExecute(unused);
            if(progressDialog.isShowing())
                progressDialog.dismiss();

            listAdapter = new SimpleAdapter(incident_list.this, incidentList, R.layout.item, new String[]{"name"}, new int[]{R.id.incidents});
            listView.setAdapter(listAdapter);
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            progressDialog = new ProgressDialog(incident_list.this);
            progressDialog.setMessage("Loading...");
            progressDialog.setCancelable(false);
            progressDialog.show();
        }

        @Override
        protected Void doInBackground(Void... voids) {
            Service service = new Service(incident_list.this);
            incidentList = service.getIncidents();
            return null;

        }
    }

    private class deleteIncident extends AsyncTask<Void,Void,Void> {

        @Override
        protected void onPostExecute(Void unused) {
            super.onPostExecute(unused);
            if(progressDialog.isShowing())
                progressDialog.dismiss();

            Toast.makeText(getApplicationContext(), response, Toast.LENGTH_SHORT).show();
            incidentList = new ArrayList<>();
            listAdapter = null;
            listView.setAdapter(null);
            new getIncidents().execute();
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            progressDialog = new ProgressDialog(incident_list.this);
            progressDialog.setMessage("Loading...");
            progressDialog.setCancelable(false);
            progressDialog.show();
        }


        @Override
        protected Void doInBackground(Void... voids) {
            Service service = new Service(incident_list.this);

            String json = service.deleteIncident(code);

            if(json != null){
                response = json;
            }else{
                Toast.makeText(getApplicationContext(), "Server Error", Toast.LENGTH_LONG).show();
                runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        Toast.makeText(getApplicationContext(), "Server Error", Toast.LENGTH_LONG).show();
                    }
                });
            }
            return null;
        }
    }
}