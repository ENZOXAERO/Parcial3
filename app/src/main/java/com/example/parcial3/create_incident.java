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
import android.graphics.drawable.BitmapDrawable;
import android.os.AsyncTask;
import android.os.Bundle;
import android.provider.MediaStore;
import android.util.Base64;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.SimpleAdapter;
import android.widget.Spinner;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.ByteArrayOutputStream;
import java.io.OutputStream;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class create_incident extends AppCompatActivity {


    ProgressDialog progressDialog;

    List<types> types;

    private static int REQUEST_CODE = 100;

    ImageView img;

    EditText txtDescription;

    Button btnTake, btnSave;

    Bitmap bitmap;

    Spinner ddlType;

    String response = "";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LOCKED);
        setContentView(R.layout.activity_create_incident);

        txtDescription = findViewById(R.id.txtDescription);
        ddlType = findViewById(R.id.ddlType);
        img = findViewById(R.id.img);


//        byte[] deco = Base64.decode("", Base64.DEFAULT);
//        Bitmap b = BitmapFactory.decodeByteArray(deco, 0, deco.length);
//        img.setImageBitmap(b);

        btnTake = findViewById(R.id.btnTake);
        btnSave = findViewById(R.id.btnSave);

        if(ContextCompat.checkSelfPermission(create_incident.this, android.Manifest.permission.CAMERA) != PackageManager.PERMISSION_GRANTED)
            askPermissions();

        btnTake.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
                startActivityForResult(intent, REQUEST_CODE);
            }
        });


        img.setOnLongClickListener(new View.OnLongClickListener() {
            @Override
            public boolean onLongClick(View view) {
                Intent intent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
                startActivityForResult(intent, REQUEST_CODE);
                return false;
            }
        });

        btnSave.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
               new create().execute();
            }
        });

        Button btnBack = findViewById(R.id.btnBack);

        btnBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) { create_incident.super.onBackPressed(); }
        });

        new getTypes().execute();
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

    @Override
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

    private class create extends AsyncTask<Void,Void,Void> {

        @Override
        protected void onPostExecute(Void unused) {
            super.onPostExecute(unused);
            if(progressDialog.isShowing())
                progressDialog.dismiss();

            Toast.makeText(getApplicationContext(), response, Toast.LENGTH_SHORT).show();
            txtDescription.setText("");
            img = null;
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            progressDialog = new ProgressDialog(create_incident.this);
            progressDialog.setMessage("Loading...");
            progressDialog.setCancelable(false);
            progressDialog.show();
        }

        @Override
        protected Void doInBackground(Void... voids) {

            List<detail> details = new ArrayList<>();
            types _type = (types)ddlType.getSelectedItem();

            details.add(new detail(0,  0,txtDescription.getText().toString().trim(),base64Encode(),0,txtDescription.getText().toString().trim(),1 ));

            incident incident = new incident(0, txtDescription.getText().toString().trim(), _type.getKey(), "", 1, details, 1);

            Service service = new Service(create_incident.this);

            String json = service.saveIncident(incident);

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

    private class getTypes extends AsyncTask<Void,Void,Void> {

        @Override
        protected void onPostExecute(Void unused) {
            super.onPostExecute(unused);
            if(progressDialog.isShowing())
                progressDialog.dismiss();

            ddlType.setAdapter(new ArrayAdapter<types>(create_incident.this, android.R.layout.simple_spinner_item, types));
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            progressDialog = new ProgressDialog(create_incident.this);
            progressDialog.setMessage("Loading...");
            progressDialog.setCancelable(false);
            progressDialog.show();
        }

        @Override
        protected Void doInBackground(Void... voids) {
            Service service = new Service(create_incident.this);
            types = service.getTypes();
            return null;
        }
    }

}