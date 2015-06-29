#if UNITY_WEBPLAYER
#define USE_PLAYER_PREFS
#else
//#define USE_PLAYER_PREFS
#endif



using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using UnityEngine;



/// <summary>
/// <see cref="IniFile"/> allows to create and parse simple INI files
/// </summary>
public class IniFile
{
    /// <summary>
    /// <see cref="IniFile+KeyPair"/> is used in keys map to keep value and comment for a single key
    /// </summary>
    public class KeyPair
    {
		public string key;
		public string value;
		public string comment;



        /// <summary>
        /// Initializes a new instance of the <see cref="IniFile+KeyPair"/> class.
        /// </summary>
        /// <param name="key">Key name.</param>
        /// <param name="value">Value of key.</param>
        /// <param name="comment">Comment of key.</param>
		public KeyPair(string key, string value, string comment)
        {
			this.key     = key;
			this.value   = value;
			this.comment = comment;
        }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="IniFile+KeyPair"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="IniFile+KeyPair"/>.</returns>
		public override string ToString()
		{
			return string.Format("[KeyPair: key={0}, value={1}, comment={2}]", key, value, comment);
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="IniFile+KeyPair"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="IniFile+KeyPair"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current <see cref="IniFile+KeyPair"/>;
		/// otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			
			if (obj == this)
			{
				return true;
			}
			
			KeyPair another = obj as KeyPair;

			if (another == null)
			{
				return false;
			}

			if (key != another.key)
			{
				return false;
			}

			if (value != another.value)
			{
				return false;
			}

			if (comment != another.comment)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="IniFile+KeyPair"/> object.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode()
		{
			return key.GetHashCode();
		}
    }



	/// <summary>
	/// Gets list of keys in current group.
	/// </summary>
	/// <value>List of keys in current group.</returns>
	public ReadOnlyCollection<string> keys
	{
		get
		{
			List<string> res = new List<string>();

			if (mCurrentGroup == "")
			{
				for (int i = 0; i < mKeysList.Count; ++i)
				{
					res.Add(mKeysList[i].key);
				}
			}
			else
			{
				int groupNameLength = mCurrentGroup.Length;

				for (int i = 0; i < mKeysList.Count; ++i)
				{
					if (mKeysList[i].key.StartsWith(mCurrentGroup))
					{
						res.Add(mKeysList[i].key.Substring(groupNameLength));
					}
				}
			}
			
			return res.AsReadOnly();
		}
	}

	/// <summary>
	/// Gets list of values in current group.
	/// </summary>
	/// <value>List of values in current group.</returns>
	public ReadOnlyCollection<KeyPair> values
	{
		get
		{
			if (mCurrentGroup == "")
			{
				return mKeysList.AsReadOnly();
			}
			else
			{
				List<KeyPair> res = new List<KeyPair>();

				int groupNameLength = mCurrentGroup.Length;
				
				for (int i = 0; i < mKeysList.Count; ++i)
				{
					if (mKeysList[i].key.StartsWith(mCurrentGroup))
					{
						res.Add(new KeyPair(mKeysList[i].key.Substring(groupNameLength), mKeysList[i].value, mKeysList[i].comment));
					}
				}

				return res.AsReadOnly();
			}
		}
	}

	/// <summary>
	/// Gets amount of properties in current group.
	/// </summary>
	/// <value>Amount of properties in current group.</value>
	public int count
	{
		get 
		{
			if (mCurrentGroup == "")
			{
				return mKeysList.Count; 
			}
			else
			{
				int res = 0;

				for (int i = 0; i < mKeysList.Count; ++i)
				{
					if (mKeysList[i].key.StartsWith(mCurrentGroup))
					{
						++res;
					}
				}

				return res;
			}
		}
	}

	/// <summary>
	/// Gets the current group.
	/// </summary>
	/// <value>The current group.</value>
	public string currentGroup
	{
		get { return mCurrentGroup; }
	}



    private Dictionary<string, KeyPair> mKeysMap;
    private List<KeyPair>               mKeysList;
	private List<string>                mUsedGroupsList;
	private string                      mCurrentGroup;



    /// <summary>
    /// Initializes a new instance of the <see cref="IniFile"/> class.
    /// </summary>
    public IniFile()
    {
        Init();
    }

    /// <summary>
	/// Initializes a new instance of the <see cref="IniFile"/> class and load from file.
    /// </summary>
    /// <param name="file">Name of file for loading.</param>
    public IniFile(string file)
    {
        Init();
        Load(file);
    }

	/// <summary>
	/// Initializes a new instance of the <see cref="IniFile"/> class and load from text asset.
	/// </summary>
	/// <param name="asset">Text asset for loading.</param>
	public IniFile(TextAsset asset)
	{
		Init();
		Load(asset);
	}

    /// <summary>
    /// Initialization.
    /// </summary>
    private void Init()
    {
        mKeysMap        = new Dictionary<string, KeyPair>();
        mKeysList       = new List<KeyPair>();
		mUsedGroupsList = new List<string>();
		mCurrentGroup   = "";
    }

	/// <summary>
	/// Opens group with specified name.
	/// </summary>
	/// <param name="group">Group name.</param>
	public void BeginGroup(string group)
	{
		mUsedGroupsList.Add(mCurrentGroup);
		mCurrentGroup += group + "/";
	}

	/// <summary>
	/// Close the latest openned group.
	/// </summary>
	public void EndGroup()
	{
		if (mUsedGroupsList.Count > 0)
		{
			mCurrentGroup = mUsedGroupsList[mUsedGroupsList.Count - 1];
			mUsedGroupsList.RemoveAt(mUsedGroupsList.Count - 1);
		}
		else
		{
			Debug.LogError("Failed to close group. There is no more openned group");
		}
	}

    #region Set functions
    /// <summary>
    /// Set value of property. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
    public void Set(string key, int value)
    {
        Set(key, value, "");
    }

    /// <summary>
    /// Set value of property and add comment. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
    /// <param name="comment">Comment for property</param>
    public void Set(string key, int value, string comment)
    {
        Set(key, value.ToString(), comment);
    }

    /// <summary>
    /// Set value of property. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
	public void Set(string key, float value)
    {
        Set(key, value, "");
    }

    /// <summary>
    /// Set value of property and add comment. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
    /// <param name="comment">Comment for property</param>
	public void Set(string key, float value, string comment)
    {
        Set(key, value.ToString(), comment);
    }

    /// <summary>
    /// Set value of property. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
	public void Set(string key, double value)
    {
        Set(key, value, "");
    }

    /// <summary>
    /// Set value of property and add comment. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
    /// <param name="comment">Comment for property</param>
	public void Set(string key, double value, string comment)
    {
        Set(key, value.ToString(), comment);
    }

    /// <summary>
    /// Set value of property. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
	public void Set(string key, bool value)
    {
        Set(key, value, "");
    }

    /// <summary>
    /// Set value of property and add comment. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
    /// <param name="comment">Comment for property</param>
	public void Set(string key, bool value, string comment)
    {
        Set(key, value.ToString(), comment);
    }

    /// <summary>
    /// Set value of property. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
	public void Set(string key, string value)
    {
        Set(key, value, "");
    }

    /// <summary>
    /// Set value of property and add comment. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
    /// <param name="comment">Comment for property</param>
	public void Set(string key, string value, string comment)
    {
		if (!key.Contains("="))
		{
			KeyPair outKeyPair = null;
			
			if (mKeysMap.TryGetValue(mCurrentGroup + key, out outKeyPair))
			{
				outKeyPair.value   = value;
				outKeyPair.comment = comment;
				
				return;
			}
			
			outKeyPair = new KeyPair(mCurrentGroup + key, value, comment);
			
			mKeysMap.Add(mCurrentGroup + key, outKeyPair);
			mKeysList.Add(outKeyPair);
		}
		else
		{
			Debug.LogError("Invalid key name: " + key);
		}
    }
    #endregion

    #region Get functions
    /// <summary>
    /// Returns the value of property.
    /// </summary>
    /// <returns>Value of property.</returns>
    /// <param name="key">Name of property</param>
    /// <param name="defaultValue">Default value if property absent</param>
	public int Get(string key, int defaultValue)
    {
        string value = Get(key);

        try
        {
            return Convert.ToInt32(value);
        }
        catch(Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Returns the value of property.
    /// </summary>
    /// <returns>Value of property.</returns>
    /// <param name="key">Name of property</param>
    /// <param name="defaultValue">Default value if property absent</param>
	public float Get(string key, float defaultValue)
    {
        string value = Get(key);

        try
        {
            return Convert.ToSingle(value);
        }
        catch(Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Returns the value of property.
    /// </summary>
    /// <returns>Value of property.</returns>
    /// <param name="key">Name of property</param>
    /// <param name="defaultValue">Default value if property absent</param>
	public double Get(string key, double defaultValue)
    {
        string value = Get(key);

        try
        {
            return Convert.ToDouble(value);
        }
        catch(Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Returns the value of property.
    /// </summary>
    /// <returns>Value of property.</returns>
    /// <param name="key">Name of property</param>
    /// <param name="defaultValue">Default value if property absent</param>
	public bool Get(string key, bool defaultValue)
    {
        string value = Get(key);

        try
        {
            return Convert.ToBoolean(value);
        }
        catch(Exception)
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Returns the value of property.
    /// </summary>
    /// <returns>Value of property.</returns>
    /// <param name="key">Name of property</param>
	public string Get(string key)
    {
        return Get(key, "");
    }

    /// <summary>
    /// Returns the value of property.
    /// </summary>
    /// <returns>Value of property.</returns>
    /// <param name="key">Name of property</param>
    /// <param name="defaultValue">Default value if property absent</param>
	public string Get(string key, string defaultValue)
    {
		if (!key.Contains("="))
		{
			KeyPair outKeyPair = null;
			
			if (mKeysMap.TryGetValue(mCurrentGroup + key, out outKeyPair))
			{
				return outKeyPair.value;
			}
		}
		else
		{
			Debug.LogError("Invalid key name: " + key);
		}       

        return defaultValue;
    }
    #endregion

	/// <summary>
	/// Remove all properties.
	/// </summary>
	public void Clear()
	{
		mKeysMap.Clear();
		mKeysList.Clear();
		mUsedGroupsList.Clear();
		mCurrentGroup = "";
	}

	/// <summary>
	/// Remove property by name.
	/// </summary>
	/// <param name="key">Name of property</param>
	public bool Remove(string key)
    {
        KeyPair outKeyPair = null;

		if (mKeysMap.TryGetValue(mCurrentGroup + key, out outKeyPair))
        {
			bool res = true;

			if (!mKeysList.Remove(outKeyPair))
			{
				res = false;
			}

			if (!mKeysMap.Remove(mCurrentGroup + key))
			{
				res = false;
			}

			if (!res)
			{
				Debug.LogError("Failed to remove key: " + key);
			}

			return res;
        }

		return true;
    }

    /// <summary>
    /// Change name of key. This function may remove existing newKey property.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="newKey">New name of property</param>
	public bool RenameKey(string key, string newKey)
    {
        if (key.Equals(newKey))
        {
            return true;
        }

		if (!newKey.Contains("="))
		{
			KeyPair outKeyPair = null;
			
			if (mKeysMap.TryGetValue(mCurrentGroup + key, out outKeyPair))
			{
				if (mKeysMap.Remove(mCurrentGroup + key))
				{
					outKeyPair.key = mCurrentGroup + newKey;
					
					mKeysMap.Add(mCurrentGroup + newKey, outKeyPair);

					return true;
				}
				else
				{
					Debug.LogError("Failed to remove key: " + key);
				}
			}
			else
			{
				Debug.LogError("Failed to rename key. There is no key with name: " + key);
			}
		}
		else
		{
			Debug.LogError("Invalid key name: " + newKey);
		}

		return false;
    }

    /// <summary>
    /// Save properties to file.
    /// </summary>
    /// <param name="fileName">Name of file</param>
#if USE_PLAYER_PREFS
	public void Save(string fileName)
    {
        PlayerPrefs.SetInt(fileName + "_Count", mKeysList.Count);

        for (int i = 0; i < mKeysList.Count; ++i)
        {
            PlayerPrefs.SetString(fileName + "_Key" + i.ToString(),              mKeysList[i].key);
            PlayerPrefs.SetString(fileName + "_Key" + i.ToString() + "_Value",   mKeysList[i].value);
            PlayerPrefs.SetString(fileName + "_Key" + i.ToString() + "_Comment", mKeysList[i].comment);
        }
    }
#else
	public void Save(string fileName)
    {
        // Debug.Log("Save properties to file: " + Application.persistentDataPath + "/" + fileName + ".ini");

        try
        {
            StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/" + fileName + ".ini");
			Save(writer);
            stream.Close();
        }
        catch(IOException e)
        {
            Debug.Log("Impossible to save file: " + fileName + ".ini");
            Debug.LogWarning(e);
        }
    }
#endif

	/// <summary>
	/// Save properties with specified text writer.
	/// </summary>
	/// <param name="writer">Text writer.</param>
	private void Save(TextWriter writer)
	{
		// TODO: Implement groups
		
		for (int i = 0; i < mKeysList.Count; ++i)
		{
			if (!mKeysList[i].comment.Equals(""))
			{
				writer.WriteLine("; " + mKeysList[i].comment);
			}
			
			if (
				mKeysList[i].value.Contains(" ")
				||
				mKeysList[i].value.Contains("\t")
				)
			{
				writer.WriteLine(mKeysList[i].key + " = \"" + mKeysList[i].value + "\"");
			}
			else
			{
				writer.WriteLine(mKeysList[i].key + " = " + mKeysList[i].value);
			}
		}
	}

    /// <summary>
    /// Load properties from file.
    /// </summary>
    /// <param name="fileName">Name of file</param>
#if USE_PLAYER_PREFS
	public void Load(string fileName)
    {
		Clear();

        int count = PlayerPrefs.GetInt(fileName + "_Count", 0);

        for (int i = 0; i < count; ++i)
        {
            string key     = PlayerPrefs.GetString(fileName + "_Key" + i.ToString());
            string value   = PlayerPrefs.GetString(fileName + "_Key" + i.ToString() + "_Value");
            string comment = PlayerPrefs.GetString(fileName + "_Key" + i.ToString() + "_Comment");

            Set(key, value, comment);
        }
    }
#else
    public void Load(string fileName)
    {
        if (File.Exists(Application.persistentDataPath + "/" + fileName + ".ini"))
        {
            try
            {
                StreamReader reader = new StreamReader(Application.persistentDataPath + "/" + fileName + ".ini");
				Load(reader);
                reader.Close();
            }
            catch(IOException e)
            {
                Debug.Log("Impossible to open file: " + fileName + ".ini");
                Debug.LogWarning(e);
            }
        }
    }
#endif

	/// <summary>
	/// Load properties with specified text reader.
	/// </summary>
	/// <param name="reader">Text reader.</param>
	private void Load(TextReader reader)
	{
		Clear();
		
		string line           = "";
		string currentComment = "";

		// TODO: Implement groups

		while ((line = reader.ReadLine()) != null)
		{
			line = line.Trim();

			if (line.StartsWith(";"))
			{
				currentComment = line.Substring(1).Trim();
			}
			else
			{
				int index = line.IndexOf("=");
				
				if (index > 0)
				{
					string key   = line.Substring(0, index).Trim();
					string value = line.Substring(index + 1).Trim();

					if (value.Length >= 2 && value[0] == '\"' && value[value.Length - 1] == '\"')
					{
						value = value.Substring(1, value.Length - 2);
					}

					Set(key, value, currentComment);
					currentComment = "";
				}
			}
		}
	}

	/// <summary>
	/// Load properties from text asset.
	/// </summary>
	/// <param name="asset">Text asset for loading.</param>
	public void Load(TextAsset asset)
	{
		Parse(asset.text);
	}

	/// <summary>
	/// Load properties from text.
	/// </summary>
	/// <param name="text">Text.</param>
	public void Parse(string text)
	{
		StringReader reader = new StringReader(text);
		Load(reader);
		reader.Close();
	}

	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="IniFile"/>.
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="IniFile"/>.</returns>
	public override string ToString()
	{
		string res;

		StringWriter writer = new StringWriter();
		writer.NewLine = "\n";
		Save(writer);
		res = writer.ToString();
		writer.Close();

		return res;
	}

	/// <summary>
	/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="IniFile"/>.
	/// </summary>
	/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="IniFile"/>.</param>
	/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current <see cref="IniFile"/>; otherwise, <c>false</c>.</returns>
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		
		if (obj == this)
		{
			return true;
		}
		
		IniFile another = obj as IniFile;
		
		if (another == null)
		{
			return false;
		}

		if (mKeysMap.Count == another.mKeysMap.Count)
		{
			foreach (KeyValuePair<string, KeyPair> keyPair in mKeysMap)
			{
				KeyPair outKeyPair = null;
				
				if (another.mKeysMap.TryGetValue(keyPair.Key, out outKeyPair))
				{
					if (!keyPair.Value.Equals(outKeyPair))
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}

			return true;
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// Serves as a hash function for a <see cref="IniFile"/> object.
	/// </summary>
	/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
	public override int GetHashCode()
	{
		int res = 0;

		foreach (KeyValuePair<string, KeyPair> keyPair in mKeysMap)
		{
			res = res << 1 + keyPair.Value.GetHashCode();
		}

		return res;
	}
}
