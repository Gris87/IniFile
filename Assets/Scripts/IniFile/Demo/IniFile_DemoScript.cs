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
        ini = new IniFile("Test");
		Rebuild();
    }

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
