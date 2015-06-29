using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;



public class IniFile_DemoScript : MonoBehaviour
{
	public Button                removeButton     = null;
	public HorizontalLayoutGroup horizontalLayout = null;
	public InputField            inputField       = null;



    private IniFile ini;



    // Use this for initialization
    void Start()
    {
		Test();

        ini = new IniFile("Test");
		Rebuild();
    }

	#region Testing functionality
	private void AssertEqual(object obj1, object obj2)
	{
		if (!obj1.Equals(obj2))
		{
			Debug.LogError("Test failed: " + obj1 + " != " + obj2);
		}
	}

	private void Test()
	{
		IniFile.KeyPair                     testPair = new IniFile.KeyPair("1", "2", "3");
		IniFile                             testIni1 = new IniFile();
		IniFile                             testIni2 = new IniFile();
		ReadOnlyCollection<string>          keys;
		ReadOnlyCollection<IniFile.KeyPair> values;



		#region IniFile.KeyPair
		#region IniFile.KeyPair.Equals
		AssertEqual(testPair.Equals(null),                               false);
		AssertEqual(testPair.Equals(testPair),                           true);
		AssertEqual(testPair.Equals("Hello World"),                      false);
		AssertEqual(testPair.Equals(new IniFile.KeyPair("1", "2", "3")), true);
		AssertEqual(new IniFile.KeyPair("1", "2", "3").Equals(testPair), true);

		AssertEqual(new IniFile.KeyPair("1", "2", "3"), new IniFile.KeyPair("1", "2", "3"));
		#endregion

		// ---------------------------------------------------------------------------------

		#region IniFile.KeyPair.ToString
		AssertEqual(new IniFile.KeyPair("1", "2", "3").ToString(), "[KeyPair: key=1, value=2, comment=3]");
		#endregion
		#endregion

		// ===================================================================================

		#region IniFile
		#region IniFile constructor
		AssertEqual(testIni1.count,        0);
		AssertEqual(testIni1.keys.Count,   0);
		AssertEqual(testIni1.values.Count, 0);
		AssertEqual(testIni1.currentGroup, "");

		AssertEqual(testIni1.ToString(), "");
		AssertEqual(testIni1, testIni2);
		#endregion

		// ---------------------------------------------------------------------------------

		#region IniFile Set function
		testIni1.Set("Key 1",  0.1);
		testIni1.Set("Key 2",  0.2,     "Comment 2");
		testIni1.Set("Key 3",  true);
		testIni1.Set("Key 4",  false,   "Comment 4");
		testIni1.Set("Key 5",  "Hello");
		testIni1.Set("Key 6",  "World", "Comment 6");
		testIni1.Set("Key 7",  1);
		testIni1.Set("Key 8",  2,       "Comment 8");
		testIni1.Set("Key 9",  0.1f);
		testIni1.Set("Key 10", 0.2f,    "Comment 10");

		AssertEqual(testIni1.count,        10);
		AssertEqual(testIni1.keys.Count,   10);
		AssertEqual(testIni1.values.Count, 10);
		AssertEqual(testIni1.currentGroup, "");

		keys   = testIni1.keys;
		values = testIni1.values;

		for (int i = 0; i < 10; ++i)
		{
			AssertEqual(keys[i],       "Key " + (i + 1));
			AssertEqual(values[i].key, "Key " + (i + 1));
		}

		AssertEqual(values[0].value, "0.1");
		AssertEqual(values[1].value, "0.2");
		AssertEqual(values[2].value, "True");
		AssertEqual(values[3].value, "False");
		AssertEqual(values[4].value, "Hello");
		AssertEqual(values[5].value, "World");
		AssertEqual(values[6].value, "1");
		AssertEqual(values[7].value, "2");
		AssertEqual(values[8].value, "0.1");
		AssertEqual(values[9].value, "0.2");

		AssertEqual(values[0].comment, "");
		AssertEqual(values[1].comment, "Comment 2");
		AssertEqual(values[2].comment, "");
		AssertEqual(values[3].comment, "Comment 4");
		AssertEqual(values[4].comment, "");
		AssertEqual(values[5].comment, "Comment 6");
		AssertEqual(values[6].comment, "");
		AssertEqual(values[7].comment, "Comment 8");
		AssertEqual(values[8].comment, "");
		AssertEqual(values[9].comment, "Comment 10");
		
		AssertEqual(testIni1.ToString(), "Key 1 = 0.1\n"   +
		                                 "; Comment 2\n"   +
							             "Key 2 = 0.2\n"   +
							             "Key 3 = True\n"  +
							             "; Comment 4\n"   +
							             "Key 4 = False\n" +
							             "Key 5 = Hello\n" +
							             "; Comment 6\n"   +
							             "Key 6 = World\n" +
							             "Key 7 = 1\n"     +
							             "; Comment 8\n"   +
							             "Key 8 = 2\n"     +
							             "Key 9 = 0.1\n"   +
							             "; Comment 10\n"  +
							             "Key 10 = 0.2\n");

		testIni2.Parse(testIni1.ToString());
		AssertEqual(testIni1, testIni2);
		#endregion
		#endregion
	}
	#endregion

	public void Reload()
	{
		ini.Load("Test");
		Rebuild();
	}

	public void Save()
	{
		ini.Save("Test");
	}

	public void Add()
	{
		ini.Set("Key " + ini.count.ToString(), "");
		Rebuild();
	}

	private void Rebuild()
	{
		for (int i = 0; i < transform.childCount; ++i)
		{
			UnityEngine.Object.DestroyObject(transform.GetChild(i).gameObject);
		}

		float contentHeight = 4f;

		ReadOnlyCollection<string> keys = ini.keys;

		for (int i = 0; i < keys.Count; ++i) 
		{
			string key   = keys[i];
			string value = ini.Get(key);

			// ---------------------------------------------------------------------------------------

			GameObject removeButtonObject = UnityEngine.Object.Instantiate<GameObject>(removeButton.gameObject);
			removeButtonObject.transform.SetParent(transform);
			RectTransform removeButtonTransform = removeButtonObject.transform as RectTransform;

			removeButtonTransform.offsetMin = new Vector2(4f,  -contentHeight - 30f);
			removeButtonTransform.offsetMax = new Vector2(44f, -contentHeight);

			Button removeBtn = removeButtonObject.GetComponent<Button>();
			removeBtn.onClick.AddListener(() => RemoveKey(key));

			// ---------------------------------------------------------------------------------------
			
			GameObject layoutObject = UnityEngine.Object.Instantiate<GameObject>(horizontalLayout.gameObject);
			layoutObject.transform.SetParent(transform);
			RectTransform layoutTransform = layoutObject.transform as RectTransform;

			layoutTransform.offsetMin = new Vector2(48f, -contentHeight - 30f);
			layoutTransform.offsetMax = new Vector2(-4f,  -contentHeight);

			// ---------------------------------------------------------------------------------------

			GameObject keyInputFieldObject = UnityEngine.Object.Instantiate<GameObject>(inputField.gameObject);
			keyInputFieldObject.transform.SetParent(layoutObject.transform);

			InputField keyInputField = keyInputFieldObject.GetComponent<InputField>();
			keyInputField.text = key;
			keyInputField.onEndEdit.AddListener((newKey) => RenameKey(key, newKey));

			// ---------------------------------------------------------------------------------------

			GameObject valueInputFieldObject = UnityEngine.Object.Instantiate<GameObject>(inputField.gameObject);
			valueInputFieldObject.transform.SetParent(layoutObject.transform);

			InputField valueInputField = valueInputFieldObject.GetComponent<InputField>();
			valueInputField.text = value;
			valueInputField.onEndEdit.AddListener((newValue) => ChangeValue(key, newValue));

			// ---------------------------------------------------------------------------------------

			contentHeight += 34f;
		}

		RectTransform rectTransform = transform as RectTransform;
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, contentHeight);
	}

	private void RemoveKey(string key)
	{
		ini.Remove(key);
		Rebuild();
	}

	private void RenameKey(string key, string newKey)
	{
		ini.RenameKey(key, newKey);
		Rebuild();
	}

	private void ChangeValue(string key, string value)
	{
		ini.Set(key, value);
	}
}
