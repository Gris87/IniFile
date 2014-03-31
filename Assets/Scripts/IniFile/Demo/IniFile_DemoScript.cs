using UnityEngine;
using System.Collections;

public class IniFile_DemoScript : MonoBehaviour
{
	private IniFile ini;

	private Vector2 scrollPosition=Vector2.zero;

	// Use this for initialization
	void Start()
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

		string[] keys=ini.keys();

		float scrollWidth  = Screen.width*0.9f;
		float scrollHeight = Screen.height*0.8f;
		float rowHeight    = Screen.height*0.025f;
		float rowOffset    = rowHeight+Screen.height*0.005f;

		GUI.BeginGroup(new Rect(Screen.width*0.05f, Screen.height*0.15f, scrollWidth, scrollHeight));
		scrollPosition=GUI.BeginScrollView(new Rect(0, 0, scrollWidth-1, scrollHeight-1), scrollPosition, new Rect(0, 0, scrollWidth*0.95f, rowHeight+(keys.Length-1)*rowOffset));

		for (int i=0; i<keys.Length; ++i)
		{
			string key;
			string value=ini.Get(keys[i]);
			string valueNew;

			if (GUI.Button(new Rect(0, rowOffset*i, scrollWidth*0.05f,  rowHeight), "-"))
			{
				ini.Remove(keys[i]);
			}

			key      = GUI.TextField(new Rect(scrollWidth*0.055f, rowOffset*i, scrollWidth*0.45f,  rowHeight), keys[i]);
			valueNew = GUI.TextField(new Rect(scrollWidth*0.51f, rowOffset*i,  scrollWidth*0.45f,  rowHeight), value);

			if (!key.Equals(keys[i]))
			{
				ini.RenameKey(keys[i], key);
			}
			else
			if (!value.Equals(valueNew))
			{
				ini.Set(key, valueNew);
			}
		}

		GUI.EndScrollView();
		GUI.EndGroup();
	}
}
