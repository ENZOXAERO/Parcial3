package com.example.parcial3;

import java.util.List;

public class incident {

    public int code;

    public String description;

    public int type;

    public String typeDescription;

    public int state;

    public List<detail> details;

    public int codusr;

    public incident(int code, String description, int type, String typeDescription, int state, List<detail> details, int codusr) {
        this.code = code;
        this.description = description;
        this.type = type;
        this.typeDescription = typeDescription;
        this.state = state;
        this.details = details;
        this.codusr = codusr;
    }

    public int getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public int getType() {
        return type;
    }

    public void setType(int type) {
        this.type = type;
    }

    public String getTypeDescription() {
        return typeDescription;
    }

    public void setTypeDescription(String typeDescription) {
        this.typeDescription = typeDescription;
    }

    public int getState() {
        return state;
    }

    public void setState(byte state) {
        this.state = state;
    }

    public List<detail> getDetails() {
        return details;
    }

    public void setDetails(List<detail> details) {
        this.details = details;
    }

    public int getCodusr() {
        return codusr;
    }

    public void setCodusr(int codusr) {
        this.codusr = codusr;
    }
}
