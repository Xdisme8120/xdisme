using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Text;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
using System.Runtime.InteropServices;


public class JsonEvent : MonoBehaviour {

	public Text LUserName;
	public Text LPassword;
	public Text RUserName;
	public Text RPassword;
	public Text Email;
	public Text NickName;

	string url = "XXX.XXX.XXX:XXXX/XXX.php"; // 连接服务器的URL 


	public void Login()
	{
		if (LUserName.text.Length<=0 || LPassword.text.Length<=0) 
		{
			LobbyUiControl.Instance.ShowMessage ("用户名密码错误");
			return;
		}


		LoginInfo lgInfo = new LoginInfo ();
		lgInfo.jsType = "1";
		lgInfo.UserName = LUserName.text;
		lgInfo.Password = LPassword.text;
		string jsonDataPost = JsonMapper.ToJson (lgInfo); //将信息转为Json文件
		WWW www = new WWW (url, Encoding.UTF8.GetBytes (jsonDataPost));
	}

	public void  Register()
	{
		if (RUserName.text.Length <= 0 || RPassword.text.Length <= 0 || Email.text.Length <= 0) 
		{
			LobbyUiControl.Instance.ShowMessage ("请正确填写注册信息!!");
			return;
		}
		RegisterInfo RInfo = new RegisterInfo ();
		RInfo.jsType = "2";
		RInfo.userName = RUserName.text;
		RInfo.passord = RPassword.text;
		RInfo.Email = Email.text;
		string jsonDataPost = JsonMapper.ToJson (RInfo);
		WWW www = new WWW (url, Encoding.UTF8.GetBytes (jsonDataPost));
		StartCoroutine ("ReciveJson", www); // 向服务器发送JSON 
	}

	public void TalentUp(string soliderType,string skillType)
	{

		talentUp TInfo = new talentUp ();
		TInfo.jsType = "3";
		TInfo.userName = PlayerPrefs.GetString ("UserName");
		TInfo.solidertype = soliderType;
		TInfo.skilltype = skillType;

	}


	IEnumerator ReciveJson(WWW www)
	{
		
		while(!www.isDone)
		{
			Debug.Log("wait");
		}
		yield return www; //等待接受Json
		if (www.error != null) {
			Debug.LogError (www.error);
		} else 
		{
			Debug.Log (www.text);
		}


	}




	public class LoginInfo
	{
		public string jsType;
		public string UserName;
		public string Password;
	}

	public class RegisterInfo
	{
		public string jsType;
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
