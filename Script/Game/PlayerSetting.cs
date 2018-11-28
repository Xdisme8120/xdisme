using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using LitJson;
public class PlayerSetting : MonoBehaviour {

	public Toggle Bloom;
	public Toggle Blur;
	public Toggle BlackSide;
	public bool ok = true;
	public Text text;
	void Start () {
		LoadImageJsonSetting ();
	}

	public void SaveImageToJson()//配置存储
	{
		Save save = CerateSaveGo ();
		string filePath = Application.persistentDataPath+"/Setting.Json";
		string saveJsonStr = JsonMapper.ToJson (save);
		StreamWriter sw = new StreamWriter (filePath);
		sw.Write (saveJsonStr);
		sw.Close ();
		text.text = "SaveOk";
		Debug.Log ("SaveOk");

	}

	void LoadImageJsonSetting()//配置文件读取
	{
		string filePath = Application.persistentDataPath+"/Setting.Json";
		if (!File.Exists (filePath)) 
		{
			SaveImageToJson ();
			return;
		}

		StreamReader sr = new StreamReader (filePath);
		string jsonStr = sr.ReadToEnd ();
		Save save = JsonMapper.ToObject<Save> (jsonStr);

		Bloom.isOn = save.BloomSet;
		Blur.isOn = save.BlurSet;
		BlackSide.isOn = save.BlackSideSet;
	}

	Save CerateSaveGo()
	{
		Save save = new Save ();
		save.BloomSet = Bloom.isOn;
		save.BlurSet = Blur.isOn;
		save.BlackSideSet = BlackSide.isOn;

		return save;
	}
}
