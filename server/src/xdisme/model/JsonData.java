package xdisme.model;

public class JsonData {
    private String type;
    private String data;
    private String code;

    public String getType() {
        return type;
    }

    public void setType(String type) {
        this.type = type;
    }

    public String getData() {
        return data;
    }

    public void setData(String data) {
        this.data = data;
    }

    public String getCode() {
        return code;
    }

    public void setCode(String code) {
        this.code = code;
    }

    @Override
    public String toString() {
        return "JsonData{" +
                "type='" + type + '\'' +
                ", data='" + data + '\'' +
                ", code='" + code + '\'' +
                '}';
    }
}
