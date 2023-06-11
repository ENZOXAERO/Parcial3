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
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.Spinner;
import android.widget.Toast;

import java.io.ByteArrayOutputStream;
import java.util.ArrayList;
import java.util.List;

public class create_detail extends AppCompatActivity {

    ProgressDialog progressDialog;

    List<types> types;

    private static int REQUEST_CODE = 100;

    ImageView img;

    EditText txtDescription;

    Button btnTake, btnSave;

    Bitmap bitmap;

    Integer code = null;

    String response = "";


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LOCKED);
        setContentView(R.layout.activity_create_detail);

        txtDescription = findViewById(R.id.txtDescription);
        img = findViewById(R.id.img);

        Intent intent = getIntent();
        String a = intent.getStringExtra("codinc");
        code = Integer.parseInt(intent.getStringExtra("codinc"));

        btnTake = findViewById(R.id.btnTake);
        btnSave = findViewById(R.id.btnSave);

        if(ContextCompat.checkSelfPermission(create_detail.this, android.Manifest.permission.CAMERA) != PackageManager.PERMISSION_GRANTED)
            askPermissions();

        btnTake.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
                startActivityForResult(intent, REQUEST_CODE);
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
            public void onClick(View view) { create_detail.super.onBackPressed(); }
        });
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
            progressDialog = new ProgressDialog(create_detail.this);
            progressDialog.setMessage("Loading...");
            progressDialog.setCancelable(false);
            progressDialog.show();
        }

        @Override
        protected Void doInBackground(Void... voids) {

            detail detail = new detail(0, code, txtDescription.getText().toString().trim(), base64Encode(), 1, "",  1);

            Service service = new Service(create_detail.this);

            String json = service.saveDetail(detail);

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