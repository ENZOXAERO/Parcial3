package com.example.parcial3;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;

import android.Manifest;
import android.app.ProgressDialog;
import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.os.AsyncTask;
import android.os.Bundle;
import android.provider.MediaStore;
import android.util.Base64;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ListAdapter;
import android.widget.ListView;
import android.widget.SimpleAdapter;
import android.widget.Spinner;
import android.widget.Toast;

import org.json.JSONObject;

import java.io.ByteArrayOutputStream;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class update_incident extends AppCompatActivity {

    ProgressDialog progressDialog;
    ArrayList<HashMap<String,String>> incidentList = new ArrayList<>();

    private ListView listView;
    List<types> types;

    incident _incident;

    private static int REQUEST_CODE = 100;

    EditText txtDescription;

    Button btnSave, btnCreate;

    Spinner ddlType;

    String response = "";

    Integer code;

    String coddet, codinc;

    ListAdapter listAdapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LOCKED);
        setContentView(R.layout.activity_update_incident);

        txtDescription = findViewById(R.id.txtDescription);
        ddlType = findViewById(R.id.ddlType);

        btnSave = findViewById(R.id.btnSave);
        btnCreate = findViewById(R.id.btnCreateDetail);

        Intent intent = getIntent();
        code = Integer.parseInt(intent.getStringExtra("code"));
        codinc = intent.getStringExtra("code");

        btnSave.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                new update_incident.update().execute();
            }
        });

        btnCreate.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                createDetail();
            }
        });

        Button btnBack = findViewById(R.id.btnBack);

        btnBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                update_incident.super.onBackPressed();
            }
        });

        listView = findViewById(R.id.livIncidents);

        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                HashMap<String,String> data = (HashMap)(listView.getItemAtPosition(i));
                coddet = data.get("id");
                getDetail();

            }
        });

        new update_incident.getTypes().execute();

    }



    protected void onRestart() {
        super.onRestart();
        incidentList = new ArrayList<>();
        listAdapter = null;
        listView.setAdapter(null);
        new update_incident.getTypes().execute();
    }

    private void createDetail(){
        Intent intent = new Intent(this, create_detail.class);
        intent.putExtra("codinc", codinc);
        startActivity(intent);
    }

    private void getDetail(){
        Intent intent = new Intent(this, update_detail.class);
        intent.putExtra("code", coddet);
        startActivity(intent);
    }

    private int getIndex(Spinner spinner, String val){
        for (int x = 0; x < spinner.getCount(); x++){
            if(spinner.getItemAtPosition(x).toString().equalsIgnoreCase(val))
                return x;
        }
        return 0;
    }

    private class getIncident extends AsyncTask<Void,Void,Void> {

        @Override
        protected void onPostExecute(Void unused) {
            super.onPostExecute(unused);

            txtDescription.setText(_incident.getDescription());
            listAdapter = new SimpleAdapter(update_incident.this, incidentList, R.layout.item, new String[]{"name"}, new int[]{R.id.incidents});
            listView.setAdapter(listAdapter);
            ddlType.setSelection(getIndex(ddlType, _incident.getTypeDescription()),true);
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
        }

        @Override
        protected Void doInBackground(Void... voids) {
            Service service = new Service(update_incident.this);

            _incident = service.incident(code);

            for(int x = 0; x < _incident.details.size(); x++){
                HashMap<String,String> incident = new HashMap<>();
                incident.put("id",String.valueOf(_incident.details.get(x).getCode()));
                incident.put("name",_incident.details.get(x).getDescription());
                incidentList.add(incident);
            }
            return null;
        }
    }

    private class getTypes extends AsyncTask<Void,Void,Void> {

        @Override
        protected void onPostExecute(Void unused) {
            super.onPostExecute(unused);
            if(progressDialog.isShowing())
                progressDialog.dismiss();
            ddlType.setAdapter(new ArrayAdapter<types>(update_incident.this, android.R.layout.simple_spinner_item, types));
            new update_incident.getIncident().execute();
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            progressDialog = new ProgressDialog(update_incident.this);
            progressDialog.setMessage("Loading...");
            progressDialog.setCancelable(false);
            progressDialog.show();
        }

        @Override
        protected Void doInBackground(Void... voids) {
            Service service = new Service(update_incident.this);
            types = service.getTypes();
            return null;
        }
    }
    private class update extends AsyncTask<Void,Void,Void> {

        @Override
        protected void onPostExecute(Void unused) {
            super.onPostExecute(unused);
            if(progressDialog.isShowing())
                progressDialog.dismiss();

            Toast.makeText(getApplicationContext(), response, Toast.LENGTH_SHORT).show();
            txtDescription.setText("");
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            progressDialog = new ProgressDialog(update_incident.this);
            progressDialog.setMessage("Loading...");
            progressDialog.setCancelable(false);
            progressDialog.show();
        }

        @Override
        protected Void doInBackground(Void... voids) {

            types _type = (types)ddlType.getSelectedItem();

            incident incident = new incident(code, txtDescription.getText().toString().trim(), _type.getKey(), "",1, null, 1);

            Service service = new Service(update_incident.this);

            String json = service.updateIncident(incident);

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