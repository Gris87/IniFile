#if UNITY_WEBPLAYER
#define USE_PLAYER_PREFS
#else
//#define USE_PLAYER_PREFS
#endif

using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// <see cref="IniFile"/> allows to create and parse simple INI files
/// </summary>
public class IniFile
{
    /// <summary>
    /// <see cref="IniFile+KeyPair"/> is used in keys map to keep value and comment for a single key
    /// </summary>
    private class KeyPair
    {
        public string key;
        public string value;
        public string comment;

        /// <summary>
        /// Initializes a new instance of the <see cref="IniFile+KeyPair"/> class.
        /// </summary>
        /// <param name="key">Key name.</param>
        /// <param name="value">Value for a key.</param>
        /// <param name="comment">Comment for a key.</param>
        public KeyPair(string key, string value, string comment)
        {
            this.key     = key;
            this.value   = value;
            this.comment = comment;
        }
    }

    private Dictionary<string, KeyPair> mKeysMap;
    private List<KeyPair>               mKeysList;

    /// <summary>
    /// Create a new instance of <see cref="IniFile"/>.
    /// </summary>
    public IniFile()
    {
        init();
    }

    /// <summary>
    /// Create a new instance of <see cref="IniFile"/> and load file.
    /// </summary>
    /// <param name="file">Name of file for loading.</param>
    public IniFile(string file)
    {
        init();
        load(file);
    }

    /// <summary>
    /// Initialization.
    /// </summary>
    private void init()
    {
        mKeysMap  = new Dictionary<string, KeyPair>();
        mKeysList = new List<KeyPair>();
    }

    #region Set functions
    /// <summary>
    /// Set value of property. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
    public void set(string key, int value)
    {
        set(key, value, "");
    }

    /// <summary>
    /// Set value of property and add comment. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
    /// <param name="comment">Comment for property</param>
    public void set(string key, int value, string comment)
    {
        set(key, value.ToString(), comment);
    }

    /// <summary>
    /// Set value of property. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
    public void set(string key, float value)
    {
        set(key, value, "");
    }

    /// <summary>
    /// Set value of property and add comment. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
    /// <param name="comment">Comment for property</param>
    public void set(string key, float value, string comment)
    {
        set(key, value.ToString(), comment);
    }

    /// <summary>
    /// Set value of property. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
    public void set(string key, double value)
    {
        set(key, value, "");
    }

    /// <summary>
    /// Set value of property and add comment. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
    /// <param name="comment">Comment for property</param>
    public void set(string key, double value, string comment)
    {
        set(key, value.ToString(), comment);
    }

    /// <summary>
    /// Set value of property. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
    public void set(string key, bool value)
    {
        set(key, value, "");
    }

    /// <summary>
    /// Set value of property and add comment. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
    /// <param name="comment">Comment for property</param>
    public void set(string key, bool value, string comment)
    {
        set(key, value.ToString(), comment);
    }

    /// <summary>
    /// Set value of property. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
    public void set(string key, string value)
    {
        set(key, value, "");
    }

    /// <summary>
    /// Set value of property and add comment. It will create new property if absent.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="value">New value</param>
    /// <param name="comment">Comment for property</param>
    public void set(string key, string value, string comment)
    {
        KeyPair outKeyPair=null;

        if (mKeysMap.TryGetValue(key, out outKeyPair))
        {
            outKeyPair.value   = value;
            outKeyPair.comment = comment;

            return;
        }

        outKeyPair=new KeyPair(key, value, comment);

        mKeysMap.Add(key, outKeyPair);
        mKeysList.Add(outKeyPair);
    }
    #endregion

    #region Get functions
    /// <summary>
    /// Returns the value of property.
    /// </summary>
    /// <returns>Value of property.</returns>
    /// <param name="key">Name of property</param>
    /// <param name="defaultValue">Default value if property absent</param>
    public int get(string key, int defaultValue)
    {
        string value=get(key);

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
    public float get(string key, float defaultValue)
    {
        string value=get(key);

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
    public double get(string key, double defaultValue)
    {
        string value=get(key);

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
    public bool get(string key, bool defaultValue)
    {
        string value=get(key);

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
    public string get(string key)
    {
        return get(key, "");
    }

    /// <summary>
    /// Returns the value of property.
    /// </summary>
    /// <returns>Value of property.</returns>
    /// <param name="key">Name of property</param>
    /// <param name="defaultValue">Default value if property absent</param>
    public string get(string key, string defaultValue)
    {
        KeyPair outKeyPair=null;

        if (mKeysMap.TryGetValue(key, out outKeyPair))
        {
            return outKeyPair.value;
        }

        return defaultValue;
    }
    #endregion

    /// <summary>
    /// Remove property by name.
    /// </summary>
    /// <param name="key">Name of property</param>
    public void remove(string key)
    {
        KeyPair outKeyPair=null;

        if (mKeysMap.TryGetValue(key, out outKeyPair))
        {
            mKeysList.Remove(outKeyPair);
            mKeysMap.Remove (key);
        }
    }

    /// <summary>
    /// Change name of key. This function may remove existing newKey property.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="newKey">New name of property</param>
    public void renameKey(string key, string newKey)
    {
        if (key.Equals(newKey))
        {
            return;
        }

        KeyPair outKeyPair=null;

        if (mKeysMap.TryGetValue(key, out outKeyPair))
        {
            outKeyPair.key=newKey;

            mKeysMap.Add    (newKey, outKeyPair);
            mKeysMap.Remove (key);
        }
    }

    /// <summary>
    /// Save properties to file.
    /// </summary>
    /// <param name="fileName">Name of file</param>
#if USE_PLAYER_PREFS
    public void save(string fileName)
    {
        PlayerPrefs.SetInt(fileName+"_Count", mKeysList.Count);

        for (int i=0; i<mKeysList.Count; ++i)
        {
            PlayerPrefs.SetString(fileName+"_Key"+i.ToString(),            mKeysList[i].key);
            PlayerPrefs.SetString(fileName+"_Key"+i.ToString()+"_Value",   mKeysList[i].value);
            PlayerPrefs.SetString(fileName+"_Key"+i.ToString()+"_Comment", mKeysList[i].comment);
        }
    }
#else
    public void save(string fileName)
    {
        // Debug.Log("Save properties to file: "+Application.persistentDataPath+"/"+fileName+".ini");

        try
        {
            StreamWriter stream=new StreamWriter(Application.persistentDataPath+"/"+fileName+".ini");

            for (int i=0; i<mKeysList.Count; ++i)
            {
                if (!mKeysList[i].comment.Equals(""))
                {
                    stream.WriteLine("; "+mKeysList[i].comment);
                }

                stream.WriteLine(mKeysList[i].key+"="+mKeysList[i].value);
            }

            stream.Close();
        }
        catch(IOException e)
        {
            Debug.Log("Impossible to save file: "+fileName+".ini");
            Debug.LogWarning(e);
        }
    }
#endif

    /// <summary>
    /// Load properties from file.
    /// </summary>
    /// <param name="fileName">Name of file</param>
#if USE_PLAYER_PREFS
    public void load(string fileName)
    {
        mKeysMap.Clear();
        mKeysList.Clear();

        int count=PlayerPrefs.GetInt(fileName+"_Count", 0);

        for (int i=0; i<count; ++i)
        {
            string key     = PlayerPrefs.GetString(fileName+"_Key"+i.ToString());
            string value   = PlayerPrefs.GetString(fileName+"_Key"+i.ToString()+"_Value");
            string comment = PlayerPrefs.GetString(fileName+"_Key"+i.ToString()+"_Comment");

            set(key, value, comment);
        }
    }
#else
    public void load(string fileName)
    {
        if (File.Exists(Application.persistentDataPath+"/"+fileName+".ini"))
        {
            mKeysMap.Clear();
            mKeysList.Clear();

            string line="";
            string currentComment="";

            try
            {
                StreamReader stream=new StreamReader(Application.persistentDataPath+"/"+fileName+".ini");

                while ((line=stream.ReadLine())!=null)
                {
                    if (line.StartsWith(";"))
                    {
                        currentComment=line.Substring(1).Trim();
                    }
                    else
                    {
                        int index=line.IndexOf("=");

                        if (index>0)
                        {
                            set(line.Substring(0, index), line.Substring(index+1), currentComment);
                            currentComment="";
                        }
                    }
                }

                stream.Close();
            }
            catch(IOException e)
            {
                Debug.Log("Impossible to open file: "+fileName+".ini");
                Debug.LogWarning(e);
            }
        }
    }
#endif

    /// <summary>
    /// Returns the list of keys.
    /// </summary>
    /// <returns>List of keys.</returns>
    public string[] keys()
    {
        string[] res=new string[mKeysList.Count];

        for (int i=0; i<mKeysList.Count; ++i)
        {
            res[i]=mKeysList[i].key;
        }

        return res;
    }

    /// <summary>
    /// Returns amount of properties.
    /// </summary>
    /// <returns>Amount of properties.</returns>
    public int count()
    {
        return mKeysList.Count;
    }
}
