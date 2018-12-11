using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class JsonTest : MonoBehaviour {
    string url = "http://134.175.218.82:8080/api/login.action;"; // 连接服务器的URL 
    //string url = "http://127.0.0.1:8080/login.action"; // 连接服务器的URL 
                                                               // Use this for initialization
    void Start () {
        Debug.Log("123");
        LoginInfo lgInfo = new LoginInfo();
        lgInfo.jsType = "1";
        //lgInfo.UserName = LUserName.text;
        //lgInfo.Password = LPassword.text;
        lgInfo.UserName = "123";
        lgInfo.Password = "123";
        string path = Application.persistentDataPath + "/LoginMessage.Json";
        string jsonDataPost = JsonMapper.ToJson(lgInfo);//将信息转为Json文件
        StreamWriter sw = new StreamWriter(path);
        sw.Write(jsonDataPost);
        sw.Close();

        StartCoroutine ("ReciveJson", lgInfo);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public class LoginInfo
    {
        public string jsType;
        public string UserName;
        public string Password;
    }
    IEnumerator ReciveJson(LoginInfo obj)
    {
        //WWW www = new WWW(url, Encoding.UTF8.GetBytes("account=123&password=123"));
        string jsonstr = JsonMapper.ToJson(obj);
        Debug.Log(jsonstr + "");
        WWW www = new WWW(url, Encoding.UTF8.GetBytes(jsonstr));
            
        while (!www.isDone)
        {
            Debug.Log("wait");
        }
        yield return www; //等待接受Json
        if (www.error != null)
        {
            Debug.Log("3");
        }
        else
        {
            JsonData json = JsonMapper.ToObject(www.text);
            Debug.Log("" + json.ToJson());
            JsonDataFilter(json, json["type"].ToString());
        }
    }
    string JsonBoolRes(JsonData json)
    {
        return json["data"].ToString();
    }

    //对返回的Json数据进行处理
    void JsonDataFilter(JsonData json, string jsonType)
    {

        switch (jsonType)
        {
            case "1":
                if (JsonBoolRes(json).Equals("1"))
                    Debug.Log("finishlogin");
                else
                    Debug.Log(json["code"].ToString());
                break;
            case "2":
                if (JsonBoolRes(json).Equals("1"))
                {
                    Debug.Log("finishRegist");
                    Debug.Log("9001");
                    //LUserName.text = RUserName.text;
                    //LPassword.text = RPassword.text;
                }
                else
                    Debug.Log(json["code"].ToString());
                break;
        }
    }
}
