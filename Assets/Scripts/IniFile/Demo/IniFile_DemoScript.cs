using UnityEngine;
using System.Collections;

public class IniFile_DemoScript : MonoBehaviour
{
	private IniFile ini;

	private Vector2 scrollPosition=Vector2.zero;

	// Use this for initialization
	void Start ()
	{
		ini=new IniFile("Test");
	}

	void OnGUI()
	{
		if (GUI.Button(new Rect(Screen.width*0.05f, Screen.height*0.05f, Screen.width*0.265f, Screen.height*0.05f), "Reload"))
		{
			ini.load("Test");
		}

		if (GUI.Button(new Rect(Screen.width*0.365f, Screen.height*0.05f, Screen.width*0.265f, Screen.height*0.05f), "Save"))
		{
			ini.save("Test");
		}

		if (GUI.Button(new Rect(Screen.width*0.680f, Screen.height*0.05f, Screen.width*0.265f, Screen.height*0.05f), "Add"))
		{
			ini.Set("Key "+ini.Count().ToString(), "");
		}

		scrollPosition=GUI.BeginScrollView(new Rect(Screen.width*0.05f, Screen.height*0.15f, Screen.width*0.9f, Screen.height*0.8f), scrollPosition, new Rect(0, 0, Screen.width*0.9f, Screen.height*0.05f*ini.Count()));

		GUI.EndScrollView();
	}
}
