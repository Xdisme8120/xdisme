using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Text;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.Net;

//与服务器json数据传递
public class JsonEvent : MonoBehaviour {

	public InputField LUserName;
	public InputField LPassword;
	public InputField RUserName;
	public InputField RPassword;
	public InputField Email;
	public InputField NickName;

	string url = "http://212.64.12.155:8080/miniwar/user/login"; // 连接服务器的URL 
	string urlReg = "http://212.64.12.155:8080/miniwar/user/reg"; // 连接服务器的URL 


	public void Login()
	{
		
		if (LUserName.text.Length<=0 || LPassword.text.Length<=0) 
		{
			LobbyUiControl.Instance.ShowMessage (Message("1"));
			return;
		}


		LoginInfo lgInfo = new LoginInfo ();
		lgInfo.UserName = LUserName.text;
		lgInfo.Password = LPassword.text;
		string path = Application.persistentDataPath+"/LoginMessage.Json";
		string jsonDataPost = JsonMapper.ToJson (lgInfo);//将信息转为Json文件
		StreamWriter sw = new StreamWriter(path);
		sw.Write (jsonDataPost);
		sw.Close ();

	    	StartCoroutine ("ReciveJson",1);
	}

	public void  Register()
	{
		if (RUserName.text.Length <= 0 || RPassword.text.Length <= 0 || Email.text.Length <= 0) 
		{
			LobbyUiControl.Instance.ShowMessage (Message("2"));
			return;
		}
		StartCoroutine ("ReciveJson", 2); // 向服务器发送JSON 
	}

	public void TalentUp(string soliderType,string skillType)
	{

		talentUp TInfo = new talentUp ();
		TInfo.jsType = "3";
		TInfo.userName = PlayerPrefs.GetString ("UserName");
		TInfo.solidertype = soliderType;
		TInfo.skilltype = skillType;

	}

	//获取Json返回数据
	IEnumerator ReciveJson(int what)
	{
	    switch (what)
	    {
	        case 1:
	            //登录
	            LoginInfo lgInfo = new LoginInfo();
	            lgInfo.UserName = LUserName.text;
	            lgInfo.Password = LPassword.text;
	            string jsonDataPost = JsonMapper.ToJson(lgInfo);//将信息转为Json文件
	            Debug.Log("json:" + jsonDataPost);
	            // WWW www = new WWW (url, Encoding.UTF8.GetBytes ("username=admin&password=123"));
	            WWWForm form = new WWWForm();
	            form.AddField("username", lgInfo.UserName);
	            form.AddField("password", lgInfo.Password);
	            WWW www = new WWW(url, form);
	            while (!www.isDone)
	            {
	                Debug.Log("wait");
	            }
	            yield return www; //等待接受Json
	            if (www.error != null)
	            {
	                LobbyUiControl.Instance.ShowMessage(Message("4"));
	            }
	            else
	            {
	                Debug.Log(www.text);
	                JsonData json = JsonMapper.ToObject(www.text);
	                string jsonTxt = www.text;
	                JsonDataFilter(json, "1", jsonTxt);
	            }
              
                break;
            case 2:
                //注册
                RegisterInfo registerInfo = new RegisterInfo();
                registerInfo.Email = Email.text;
                registerInfo.nickName = NickName.text;
                registerInfo.passord = RPassword.text;
                registerInfo.userName = RUserName.text;
                string jsonReg = JsonMapper.ToJson(registerInfo);//将信息转为Json文件
                Debug.Log("json:" + jsonReg);
                WWWForm formReg = new WWWForm();
                formReg.AddField("username", registerInfo.userName);
                formReg.AddField("uPassword", registerInfo.passord);
                formReg.AddField("uEmail", registerInfo.Email);
                formReg.AddField("uNickname", registerInfo.nickName);
                WWW wwwReg = new WWW(urlReg, formReg);
                while (!wwwReg.isDone)
                {
                    Debug.Log("wait");
                }
                yield return wwwReg; //等待接受Json
                if (wwwReg.error != null)
                {
                    LobbyUiControl.Instance.ShowMessage(Message("4"));
                }
                else
                {
                    Debug.Log(wwwReg.text);
                    JsonData json = JsonMapper.ToObject(wwwReg.text);
                    string jsonTxt = wwwReg.text;
                    JsonDataFilter(json, "2", jsonTxt);
                }
                break;
	    }
	 

	}
	//对返回的Json数据进行处理
	void JsonDataFilter(JsonData json,string jsonType, string jsonTxt)
	{
		
		switch(jsonType)
		{
		case "1":
		    if (JsonBoolRes(json).Equals("0"))
		    {
                    //将数据保存到本地
		        string path = Application.persistentDataPath + "/user.json";
		        StreamWriter sw = new StreamWriter(path);
		        sw.Write(jsonTxt);
		        sw.Close();
                LobbyUiControl.Instance.FinishLOgin();
            }
                else
				LobbyUiControl.Instance.ShowMessage (Message (json ["code"].ToString ()));
			break;
		case "2":
		    if (JsonBoolRes(json).Equals("0"))
		    {
		        LobbyUiControl.Instance.FinishRegister();
		        LobbyUiControl.Instance.ShowMessage(Message("9001"));
		        LUserName.text = RUserName.text;
		        LPassword.text = RPassword.text;
		    }else
		    LobbyUiControl.Instance.ShowMessage (Message ("1003"));
			break;
		}
	}



	string JsonBoolRes(JsonData json)
	{
		return json ["status"].ToString();
	}

	string Message(string messageCode)
	{
		switch (messageCode) {
		////////////////////////////////本地消息
		case "1":
			return "用户名或密码错误";
		
		case "2":
			return "请正确填写注册信息";

		case "3":
			return "两次密码不一致";

		case "4":
			return "无法连接至服务器";

		///////////////////////////////服务器失败消息		
		case "1002":
			return "用户名或密码错误";
		
		case "1003":
			return "用户名已存在";

		case "1004":
			return "昵称已存在";
		
		///////////////////////////////服务器成功消息
		case "9001" :
			return "注册成功";
		default:
			return "0";
		}
	}



	public class LoginInfo
	{
		public string UserName;
		public string Password;
	}

    public class UserData
    {
        public int uid;
        public string username;
        public string uPassword;
        public string uEmail;
        public string uNickname;
        public int uGold;
        public int uLevel;
        public int uBlackZz;
        public int uWhiteZz;
        public int uExp;
    }
	public class RegisterInfo
	{
		public string Email;
		public string userName;
		public string passord;
		public string nickName;
	}
		
	public class talentUp
	{
		public string jsType;
		public string userName;
		public string solidertype;
		public string skilltype;
	}
}
