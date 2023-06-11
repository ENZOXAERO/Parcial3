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
import android.graphics.BitmapFactory;
import android.os.AsyncTask;
import android.os.Bundle;
import android.provider.MediaStore;
import android.util.Base64;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ListAdapter;
import android.widget.ListView;
import android.widget.Spinner;
import android.widget.Toast;

import java.io.ByteArrayOutputStream;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class update_detail extends AppCompatActivity {

    ProgressDialog progressDialog;
    ArrayList<HashMap<String,String>> incidentList = new ArrayList<>();

    private ListView listView;
    List<types> types;

    detail _detail = null;
    List<states> states;

    private static int REQUEST_CODE = 100;

    EditText txtDescription;

    Button btnTake, btnSave, btnBack;

    Bitmap bitmap;

    Spinner ddlState;

    String response = "";

    ImageView img;
    Integer code;

    ListAdapter listAdapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LOCKED);
        setContentView(R.layout.activity_update_detail);

        txtDescription = findViewById(R.id.txtDescription);
        ddlState = findViewById(R.id.ddlState);
        img = findViewById(R.id.img);

        Intent intent = getIntent();
        code = Integer.parseInt(intent.getStringExtra("code"));

        btnTake = findViewById(R.id.btnTake);
        btnSave = findViewById(R.id.btnSave);
        btnBack = findViewById(R.id.btnBack);

        btnSave.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                new update().execute();
            }
        });

        if(ContextCompat.checkSelfPermission(update_detail.this, android.Manifest.permission.CAMERA) != PackageManager.PERMISSION_GRANTED)
            askPermissions();

        btnTake.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
                startActivityForResult(intent, REQUEST_CODE);
            }
        });

        btnBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) { update_detail.super.onBackPressed(); }
        });

        new getStates().execute();
    }

    private void askPermissions() {
        ActivityCompat.requestPermissions(this,new String[]{Manifest.permission.CAMERA},REQUEST_CODE) ;
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        if (requestCode != REQUEST_CODE)
            return;

        if (grantResults.length <= 0 && grantResults[0] != PackageManager.PERMISSION_GRANTED)
            Toast.makeText(this, "Please provide the required permission", Toast.LENGTH_SHORT).show();
    }


    protected void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode != REQUEST_CODE)
            return;

        bitmap = (Bitmap) data.getExtras().get("data");
        img.setImageBitmap(bitmap);
    }

    private String base64Encode(){
        if(bitmap == null){
            return "";
        }
        ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
        bitmap.compress(Bitmap.CompressFormat.PNG, 60, byteArrayOutputStream);
        byte[] byteArray = byteArrayOutputStream.toByteArray();
        return Base64.encodeToString(byteArray, Base64.DEFAULT);
    }

    private void base64Decode(String imgBase){
        byte[] decode = Base64.decode(imgBase, Base64.DEFAULT);
        Bitmap bitmat = BitmapFactory.decodeByteArray(decode, 0, decode.length);
        img.setImageBitmap(bitmat);
    }

    private int getIndex(Spinner spinner, String val){
        for (int x = 0; x < spinner.getCount(); x++){
            if(spinner.getItemAtPosition(x).toString().equalsIgnoreCase(val))
                return x;
        }
        return 0;
    }

    private class getStates extends AsyncTask<Void,Void,Void> {

        @Override
        protected void onPostExecute(Void unused) {
            super.onPostExecute(unused);
            if(progressDialog.isShowing())
                progressDialog.dismiss();

            ddlState.setAdapter(new ArrayAdapter<states>(update_detail.this, android.R.layout.simple_spinner_item, states));
            new getDetail().execute();
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            progressDialog = new ProgressDialog(update_detail.this);
            progressDialog.setMessage("Loading...");
            progressDialog.setCancelable(false);
            progressDialog.show();
        }

        @Override
        protected Void doInBackground(Void... voids) {

            Service service = new Service(update_detail.this);
            states = service.getStates();
            return null;
        }
    }

    private class getDetail extends AsyncTask<Void,Void,Void> {

        @Override
        protected void onPostExecute(Void unused) {
            super.onPostExecute(unused);
            if(progressDialog.isShowing())
                progressDialog.dismiss();

            txtDescription.setText(_detail.getDescription());
            base64Decode(_detail.getImage());
            ddlState.setSelection(getIndex(ddlState,  _detail.getStateDescription()),true);
        }
        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            progressDialog = new ProgressDialog(update_detail.this);
            progressDialog.setMessage("Loading...");
            progressDialog.setCancelable(false);
            progressDialog.show();
        }

        @Override
        protected Void doInBackground(Void... voids) {

            Service service = new Service(update_detail.this);
            _detail = service.getDetail(code);
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
            progressDialog = new ProgressDialog(update_detail.this);
            progressDialog.setMessage("Loading...");
            progressDialog.setCancelable(false);
            progressDialog.show();
        }

        @Override
        protected Void doInBackground(Void... voids) {

            states _states = (states)ddlState.getSelectedItem();

            detail incident = new detail(code, 0, txtDescription.getText().toString().trim(), base64Encode(),_states.getKey(), "",1);

            Service service = new Service(update_detail.this);

            String json = service.updateDetail(incident);

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