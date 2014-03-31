using UnityEngine;
using System;
using System.Collections;
using System.IO;

/// <summary>
/// IniFile allows to create and parse simple INI files
/// </summary>
public class IniFile
{
    private ArrayList mKeys;
    private ArrayList mValues;
    private ArrayList mComments;

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
    /// <param name="file">Name of the file for loading.</param>
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
        mKeys     = new ArrayList();
        mValues   = new ArrayList();
        mComments = new ArrayList();
    }

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
        for (int i=0; i<mKeys.Count; ++i)
        {
            if (mKeys[i].Equals(key))
            {
                mValues[i]   = value;
                mComments[i] = comment;

                return;
            }
        }

        mKeys.Add    (key);
        mValues.Add  (value);
        mComments.Add(comment);
    }

    /// <summary>
    /// Returns the value of property.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="defaultValue">Default value if property absent</param>
    public int Get(string key, int defaultValue)
    {
        string value=Get(key);

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
    /// <param name="key">Name of property</param>
    /// <param name="defaultValue">Default value if property absent</param>
    public float Get(string key, float defaultValue)
    {
        string value=Get(key);

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
    /// <param name="key">Name of property</param>
    /// <param name="defaultValue">Default value if property absent</param>
    public double Get(string key, double defaultValue)
    {
        string value=Get(key);

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
    /// <param name="key">Name of property</param>
    /// <param name="defaultValue">Default value if property absent</param>
    public bool Get(string key, bool defaultValue)
    {
        string value=Get(key);
        
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
    /// <param name="key">Name of property</param>
    public string Get(string key)
    {
        return Get(key, "");
    }

    /// <summary>
    /// Returns the value of property.
    /// </summary>
    /// <param name="key">Name of property</param>
    /// <param name="defaultValue">Default value if property absent</param>
    public string Get(string key, string defaultValue)
    {
        for (int i=0; i<mKeys.Count; ++i)
        {
            if (mKeys[i].Equals(key))
            {
                return (string)mValues[i];
            }
        }

        return defaultValue;
    }

    /// <summary>
    /// Remove property by name.
    /// </summary>
    /// <param name="key">Name of property</param>
    public void Remove(string key)
    {
        for (int i=0; i<mKeys.Count; ++i)
        {
            if (mKeys[i].Equals(key))
            {
                mKeys.RemoveAt    (i);
                mValues.RemoveAt  (i);
                mComments.RemoveAt(i);

                return;
            }
        }
    }

	/// <summary>
	/// Change name of key.
	/// </summary>
	/// <param name="key">Name of property</param>
	/// <param name="newKey">New name of property</param>
	public void RenameKey(string key, string newKey)
	{
		Remove(newKey);

		for (int i=0; i<mKeys.Count; ++i)
		{
			if (mKeys[i].Equals(key))
			{
				mKeys[i]=newKey;

				return;
			}
		}
	}

    /// <summary>
    /// Save properties to file.
    /// </summary>
    /// <param name="fileName">Name of file</param>
    public void save(string fileName)
    {
        Debug.Log("Save properties to file: "+Application.persistentDataPath+"/"+fileName+".ini");

        try
        {
            StreamWriter stream=new StreamWriter(Application.persistentDataPath+"/"+fileName+".ini");

            for (int i=0; i<mKeys.Count; ++i)
            {
                if (!mComments[i].Equals(""))
                {
                    stream.WriteLine("; "+mComments[i]);
                }

                stream.WriteLine(mKeys[i]+"="+mValues[i]);
            }

            stream.Close();
        }
        catch(IOException e)
        {
            Debug.Log("Impossible to save file: "+fileName+".ini");
            Debug.LogWarning(e);
        }
    }

    /// <summary>
    /// Load properties from file.
    /// </summary>
    /// <param name="fileName">Name of file</param>
    public void load(string fileName)
    {
        if (File.Exists(Application.persistentDataPath+"/"+fileName+".ini"))
        {
            mKeys.Clear();
            mValues.Clear();
            mComments.Clear();

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
                            Set(line.Substring(0, index), line.Substring(index+1), currentComment);
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

	/// <summary>
	/// Returns the list of keys.
	/// </summary>
	public string[] keys()
	{
		string[] res=new string[mKeys.Count];

		for (int i=0; i<mKeys.Count; ++i)
		{
			res[i]=(string)mKeys[i];
		}

		return res;
	}

    /// <summary>
    /// Amount of properties.
    /// </summary>
    public int Count()
    {
        return mKeys.Count;
    }
}
