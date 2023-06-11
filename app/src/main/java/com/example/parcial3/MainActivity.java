package com.example.parcial3;

import androidx.appcompat.app.AppCompatActivity;

import android.app.ProgressDialog;
import android.content.Intent;
import android.content.pm.ActivityInfo;
import android.os.AsyncTask;
import android.os.Bundle;
import android.text.TextUtils;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class MainActivity extends AppCompatActivity {

    ProgressDialog progressDialog;

    EditText user;
    EditText password;

    String response = "";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setRequestedOrientation(ActivityInfo.SCREEN_ORIENTATION_LOCKED);
        setContentView(R.layout.activity_main);

        Button btnLogin = findViewById(R.id.btnLogin);
        user = findViewById(R.id.txtUser);
        password = findViewById(R.id.txtPassword);

         btnLogin.setOnClickListener(new View.OnClickListener() {
             @Override
             public void onClick(View view) {
                 if(TextUtils.isEmpty(user.getText().toString()) || TextUtils.isEmpty(password.getText().toString())){
                     Toast.makeText(getApplicationContext(), "User and password are required.", Toast.LENGTH_LONG).show();
                     return;
                 }
                 new LogIng().execute();
             }
         });
    }

    private void logIn(){
        Intent intent = new Intent(this, incident_list.class);
        startActivity(intent);
    }

    private class LogIng extends AsyncTask<Void, Void, Void>{

        protected void onPostExecute(Void unused) {
            super.onPostExecute(unused);
            if(progressDialog.isShowing())
                progressDialog.dismiss();

            if(response.equals("success")){
                user.setText("");
                password.setText("");
                logIn();
            }else{
                if(response == ""){
                    Toast.makeText(getApplicationContext(), "The server is shutdown", Toast.LENGTH_SHORT).show();
                    return;
                }
                Toast.makeText(getApplicationContext(), response, Toast.LENGTH_SHORT).show();
            }
        }

        protected void onPreExecute() {
            super.onPreExecute();
            progressDialog = new ProgressDialog(MainActivity.this);
            progressDialog.setMessage("Sending data...");
            progressDialog.setCancelable(false);
            progressDialog.show();
        }

        @Override
        protected Void doInBackground(Void... voids) {

            Service service = new Service(MainActivity.this);

            String json = service.logIng(user.getText().toString(),password.getText().toString());

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