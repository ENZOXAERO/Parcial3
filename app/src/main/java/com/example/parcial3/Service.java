package com.example.parcial3;

import android.content.Context;
import android.content.SharedPreferences;

import com.google.gson.Gson;
import com.google.gson.JsonArray;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.net.URL;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.ProtocolException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;


public class Service {

    private String api = "http://hellsing-001-site1.btempurl.com/v1.0/";
    private static final String TOKEN = "token";
    private static final String CODUSR = "codusr";

    SharedPreferences sharedpreferences;
    SharedPreferences.Editor editor;

    public Service(Context context){
        sharedpreferences = context.getSharedPreferences(TOKEN, Context.MODE_PRIVATE);
        sharedpreferences = context.getSharedPreferences(CODUSR, Context.MODE_PRIVATE);
        editor = sharedpreferences.edit();
    }

    public String logIng(String user, String password){
        String response = "";
        try {
            URL url = new URL(api + "auth/login" );
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            connection.setRequestMethod("POST");
            connection.setRequestProperty("Content-Type", "application/json");
            connection.setRequestProperty("Accept", "application/json");
            connection.setDoOutput(true);

            JSONObject model = new JSONObject();
            model.put("user",user);
            model.put("password",password);

            try(OutputStream os = connection.getOutputStream()) {
                os.write(model.toString().getBytes("utf-8"));
            }
            InputStreamReader reader = new InputStreamReader(connection.getInputStream(), "utf-8");
            int data = reader.read();
            while (data != -1) {
                response += (char) data;
                data = reader.read();
            }

            JSONObject json =  new JSONObject(response);
            if(Integer.parseInt(json.getString("status")) == 200){
                editor.putString(TOKEN,json.getString("result")).commit();
                JSONObject userData = json.getJSONObject("user");
                editor.putString(CODUSR,userData.getString("code")).commit();

            }
            return json.getString("message");
        }catch (MalformedURLException e){
            e.printStackTrace();
        } catch (ProtocolException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (JSONException e) {
            throw new RuntimeException(e);
        }

        return  response;
    }

    public List<types> getTypes(){
        String response = "";
        String token = sharedpreferences.getString(TOKEN,"");
        List<types> types = new ArrayList<>();
        try {
            URL url = new URL(api + "incident/getTypes" );
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            connection.setRequestMethod("GET");
            connection.setRequestProperty("Authorization", "Bearer " + token);
            InputStreamReader reader = new InputStreamReader( connection.getInputStream());
            int data = reader.read();
            while (data != -1) {
                response += (char) data;
                data = reader.read();
            }
            JSONObject json =  new JSONObject(response);

            JSONArray list = json.getJSONArray("reponse");

            for(int x = 0; x < list.length(); x++){
                JSONObject jsonObject = list.getJSONObject(x);
                types.add(new types(Integer.parseInt(jsonObject.getString("code")), jsonObject.getString("description")));
            }

            return types;
        }catch (MalformedURLException e){
            e.printStackTrace();
        } catch (ProtocolException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (JSONException e) {
            throw new RuntimeException(e);
        }

        return types;
    }

    public List<states> getStates(){
        String response = "";
        String token = sharedpreferences.getString(TOKEN,"");
        List<states> _states = new ArrayList<>();
        try {
            URL url = new URL(api + "incident/getStates" );
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            connection.setRequestMethod("GET");
            connection.setRequestProperty("Authorization", "Bearer " + token);
            InputStreamReader reader = new InputStreamReader( connection.getInputStream());
            int data = reader.read();
            while (data != -1) {
                response += (char) data;
                data = reader.read();
            }
            JSONObject json =  new JSONObject(response);

            JSONArray list = json.getJSONArray("reponse");

            for(int x = 0; x < list.length(); x++){
                JSONObject jsonObject = list.getJSONObject(x);
                _states.add(new states(Integer.parseInt(jsonObject.getString("code")), jsonObject.getString("description")));
            }

            return _states;
        }catch (MalformedURLException e){
            e.printStackTrace();
        } catch (ProtocolException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (JSONException e) {
            throw new RuntimeException(e);
        }

        return _states;
    }

    public String saveIncident(incident model){

        Integer codusr = Integer.parseInt(sharedpreferences.getString(CODUSR,""));
        model.setCodusr(codusr);
        model.details.get(0).codusr = codusr;
        Gson gson = new Gson();
        String js = gson.toJson(model);
        String token = sharedpreferences.getString(TOKEN,"");
        String response = "";
        try {
            URL url = new URL(api + "incident/create" );
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            connection.setRequestMethod("POST");
            connection.setRequestProperty("Content-Type", "application/json");
            connection.setRequestProperty("Accept", "application/json");
            connection.setRequestProperty("Authorization", "Bearer " + token);
            connection.setDoOutput(true);

            try(OutputStream os = connection.getOutputStream()) {
                os.write(js.getBytes("utf-8"));
            }

            InputStreamReader reader = new InputStreamReader(connection.getInputStream(), "utf-8");
            int data = reader.read();
            while (data != -1) {
                response += (char) data;
                data = reader.read();
            }

            JSONObject json =  new JSONObject(response);
            return json.getString("message");
        }catch (MalformedURLException e){
            e.printStackTrace();
        } catch (ProtocolException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (JSONException e) {
            throw new RuntimeException(e);
        }
        return  response;
    }

    public String updateIncident(incident model){

        Gson gson = new Gson();

        String js = gson.toJson(model);
        String token = sharedpreferences.getString(TOKEN,"");
        model.setCodusr(Integer.parseInt(sharedpreferences.getString(CODUSR,"")));
        String response = "";

        try {
            URL url = new URL(api + "incident/update" );
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            connection.setRequestMethod("POST");
            connection.setRequestProperty("Content-Type", "application/json");
            connection.setRequestProperty("Accept", "application/json");
            connection.setRequestProperty("Authorization", "Bearer " + token);
            connection.setDoOutput(true);

            try(OutputStream os = connection.getOutputStream()) {
                os.write(js.getBytes("utf-8"));
            }

            InputStreamReader reader = new InputStreamReader(connection.getInputStream(), "utf-8");
            int data = reader.read();
            while (data != -1) {
                response += (char) data;
                data = reader.read();
            }

            JSONObject json =  new JSONObject(response);
            return json.getString("message");
        }catch (MalformedURLException e){
            e.printStackTrace();
        } catch (ProtocolException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (JSONException e) {
            throw new RuntimeException(e);
        }
        return  response;
    }

    public String saveDetail(detail model){

        Gson gson = new Gson();

        String js = gson.toJson(model);
        String token = sharedpreferences.getString(TOKEN,"");
        model.setCodusr(Integer.parseInt(sharedpreferences.getString(CODUSR,"")));
        String response = "";

        try {
            URL url = new URL(api + "incident/createDetail" );
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            connection.setRequestMethod("POST");
            connection.setRequestProperty("Content-Type", "application/json");
            connection.setRequestProperty("Accept", "application/json");
            connection.setRequestProperty("Authorization", "Bearer " + token);
            connection.setDoOutput(true);

            try(OutputStream os = connection.getOutputStream()) {
                os.write(js.getBytes("utf-8"));
            }

            InputStreamReader reader = new InputStreamReader(connection.getInputStream(), "utf-8");
            int data = reader.read();
            while (data != -1) {
                response += (char) data;
                data = reader.read();
            }

            JSONObject json =  new JSONObject(response);
            return json.getString("message");
        }catch (MalformedURLException e){
            e.printStackTrace();
        } catch (ProtocolException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (JSONException e) {
            throw new RuntimeException(e);
        }
        return  response;
    }

    public String updateDetail(detail model){

        Gson gson = new Gson();

        String js = gson.toJson(model);
        String token = sharedpreferences.getString(TOKEN,"");
        model.setCodusr(Integer.parseInt(sharedpreferences.getString(CODUSR,"")));
        String response = "";

        try {
            URL url = new URL(api + "incident/updateDetail" );
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            connection.setRequestMethod("POST");
            connection.setRequestProperty("Content-Type", "application/json");
            connection.setRequestProperty("Accept", "application/json");
            connection.setRequestProperty("Authorization", "Bearer " + token);
            connection.setDoOutput(true);

            try(OutputStream os = connection.getOutputStream()) {
                os.write(js.getBytes("utf-8"));
            }

            InputStreamReader reader = new InputStreamReader(connection.getInputStream(), "utf-8");
            int data = reader.read();
            while (data != -1) {
                response += (char) data;
                data = reader.read();
            }

            JSONObject json =  new JSONObject(response);
            return json.getString("message");
        }catch (MalformedURLException e){
            e.printStackTrace();
        } catch (ProtocolException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (JSONException e) {
            throw new RuntimeException(e);
        }
        return  response;
    }

    public ArrayList<HashMap<String,String>> getIncidents(){
        String response = "";
        String token = sharedpreferences.getString(TOKEN,"");
        String codusr = sharedpreferences.getString(CODUSR,"");
        ArrayList<HashMap<String,String>> incidentList = new ArrayList<>();
        try {
            URL url = new URL(api + "incident/getAll/" + codusr + "/1/''" );
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            connection.setRequestMethod("GET");
            connection.setRequestProperty("Authorization", "Bearer " + token);
            InputStreamReader reader = new InputStreamReader( connection.getInputStream());
            int data = reader.read();
            while (data != -1) {
                response += (char) data;
                data = reader.read();
            }
            JSONObject json =  new JSONObject(response);

            JSONArray list = json.getJSONArray("reponse");

            for(int x = 0; x < list.length(); x++){
                JSONObject jsonObject = list.getJSONObject(x);
                HashMap<String,String> incident = new HashMap<>();
                incident.put("id",jsonObject.getString("code"));
                incident.put("name",jsonObject.getString("description"));
                incidentList.add(incident);
            }

            return incidentList;
        }catch (MalformedURLException e){
            e.printStackTrace();
        } catch (ProtocolException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (JSONException e) {
            return incidentList;
        }

        return incidentList;
    }

    public ArrayList<HashMap<String,String>> getIncident(int code){
        String response = "";
        String token = sharedpreferences.getString(TOKEN,"");
        ArrayList<HashMap<String,String>> incidentList = new ArrayList<>();
        try {
            URL url = new URL(api + "incident/get/" + code );
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            connection.setRequestMethod("GET");
            connection.setRequestProperty("Authorization", "Bearer " + token);
            InputStreamReader reader = new InputStreamReader( connection.getInputStream());
            int data = reader.read();
            while (data != -1) {
                response += (char) data;
                data = reader.read();
            }
            JSONObject json =  new JSONObject(response);
            JSONObject ss = json.getJSONObject("reponse");
            JSONArray list = ss.getJSONArray("details");

            for(int x = 0; x < list.length(); x++){
                JSONObject jsonObject = list.getJSONObject(x);
                HashMap<String,String> incident = new HashMap<>();
                incident.put("id",jsonObject.getString("code"));
                incident.put("name",jsonObject.getString("description"));
                incidentList.add(incident);
            }
            return incidentList;
        }catch (MalformedURLException e){
            e.printStackTrace();
        } catch (ProtocolException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (JSONException e) {
            throw new RuntimeException(e);
        }

        return incidentList;
    }

    public detail getDetail(int code){

        String response = "";
        String token = sharedpreferences.getString(TOKEN,"");
        ArrayList<HashMap<String,String>> incidentList = new ArrayList<>();
        detail _detail = null;
        try {
            URL url = new URL(api + "incident/getDetail/" + code );
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            connection.setRequestMethod("GET");
            connection.setRequestProperty("Authorization", "Bearer " + token);
            InputStreamReader reader = new InputStreamReader( connection.getInputStream());
            int data = reader.read();
            while (data != -1) {
                response += (char) data;
                data = reader.read();
            }
            JSONObject json =  new JSONObject(response);

            JSONObject det = json.getJSONObject("reponse");
            _detail = new detail(
                    det.getInt("code"),
                    det.getInt("codinc"),
                    det.getString("description"),
                    det.getString("image"),
                    det.getInt("codsta"),
                    det.getString("stateDescription"),
                    det.getInt("codusr")
            );

            return _detail;
        }catch (MalformedURLException e){
            e.printStackTrace();
        } catch (ProtocolException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (JSONException e) {
            throw new RuntimeException(e);
        }
        return _detail;
    }

    public incident incident(int code){
        String response = "";
        String token = sharedpreferences.getString(TOKEN,"");
        ArrayList<HashMap<String,String>> incidentList = new ArrayList<>();
        try {
            URL url = new URL(api + "incident/get/" + code );
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            connection.setRequestMethod("GET");
            connection.setRequestProperty("Authorization", "Bearer " + token);
            InputStreamReader reader = new InputStreamReader( connection.getInputStream());
            int data = reader.read();
            while (data != -1) {
                response += (char) data;
                data = reader.read();
            }
            List<detail> details = new ArrayList<>();

            JSONObject json =  new JSONObject(response);
            JSONObject detail = json.getJSONObject("reponse");
            JSONArray list = detail.getJSONArray("details");

            for(int x = 0; x < list.length(); x++){
                JSONObject jsonObject = list.getJSONObject(x);
                HashMap<String,String> incident = new HashMap<>();
                incident.put("id",jsonObject.getString("code"));
                incident.put("name",jsonObject.getString("description"));
                incidentList.add(incident);

                details.add(
                        new detail(
                                jsonObject.getInt("code"), jsonObject.getInt("codinc"),
                                jsonObject.getString("description"),jsonObject.getString("image"),
                                jsonObject.getInt("codsta"),jsonObject.getString("stateDescription"), jsonObject.getInt("codusr")
                        )
                );
            }

            incident _incident = new incident(
                    detail.getInt("code"),
                    detail.getString("description"),
                    detail.getInt("type"),
                    detail.getString("typeDescription"),
                    detail.getInt("state"),
                    details,
                    1
            );

            return _incident;
        }catch (MalformedURLException e){
            e.printStackTrace();
        } catch (ProtocolException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (JSONException e) {
            throw new RuntimeException(e);
        }

        return null;
    }

    public String deleteIncident(String code){
        String response = "";

        try {
            URL url = new URL(api + "incident/delete" );
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            String token = sharedpreferences.getString(TOKEN,"");
            connection.setRequestMethod("POST");
            connection.setRequestProperty("Authorization", "Bearer " + token);
            connection.setRequestProperty("Content-Type", "application/json");
            connection.setRequestProperty("Accept", "application/json");
            connection.setDoOutput(true);

            JSONObject model = new JSONObject();
            model.put("code",code);

            try(OutputStream os = connection.getOutputStream()) {
                os.write(model.toString().getBytes("utf-8"));
            }

            InputStreamReader reader = new InputStreamReader(connection.getInputStream(), "utf-8");
            int data = reader.read();
            while (data != -1) {
                response += (char) data;
                data = reader.read();
            }

            JSONObject json =  new JSONObject(response);
            return json.getString("message");
        }catch (MalformedURLException e){
            e.printStackTrace();
        } catch (ProtocolException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (JSONException e) {
            throw new RuntimeException(e);
        }
        return  response;
    }

//    public String saveContact(String name, String phone){
//
//        String response = "";
//        try {
//            URL url = new URL(api + "method=save&args0=" + name + "&args1=" + phone);
//            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
//            connection.setRequestMethod("POST");
//
//            OutputStreamWriter writer = new OutputStreamWriter( connection.getOutputStream());
//
//            writer.write("");
//            writer.flush();
//
//            InputStreamReader reader = new InputStreamReader( connection.getInputStream());
//            int data = reader.read();
//            while (data != -1) {
//                response += (char) data;
//                data = reader.read();
//            }
//            return response;
//        }catch (MalformedURLException e){
//            e.printStackTrace();
//        } catch (ProtocolException e) {
//            e.printStackTrace();
//        } catch (IOException e) {
//            e.printStackTrace();
//        }
//
//        return  response;
//    }


}



