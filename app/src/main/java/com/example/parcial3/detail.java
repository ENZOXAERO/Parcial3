package com.example.parcial3;

public class detail {

    public int code;

    public int codinc;

    public String description;

    public String image;

    public int codsta;

    public String stateDescription;

    public int codusr;

    public detail(int code, int codinc, String description, String image, int codsta, String stateDescription, int codusr) {
        this.code = code;
        this.codinc = codinc;
        this.description = description;
        this.image = image;
        this.codsta = codsta;
        this.stateDescription = stateDescription;
        this.codusr = codusr;
    }

    public Integer getCode() {
        return code;
    }

    public void setCode(int code) {
        this.code = code;
    }

    public int getCodinc() {
        return codinc;
    }

    public void setCodinc(int codinc) {
        this.codinc = codinc;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public String getImage() {
        return image;
    }

    public void setImage(String image) {
        this.image = image;
    }

    public int getCodsta() {
        return codsta;
    }

    public void setCodsta(int codsta) {
        this.codsta = codsta;
    }

    public String getStateDescription() {
        return stateDescription;
    }

    public void setStateDescription(String stateDescription) {
        this.stateDescription = stateDescription;
    }

    public int getCodusr() {
        return codusr;
    }

    public void setCodusr(int codusr) {
        this.codusr = codusr;
    }
}
