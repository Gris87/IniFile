IniFile
=======

Class that allow to create and parse simple ini files.

Demo:              http://gris.ucoz.ru/UnityModules/IniFile/Web/IniFile.html
Unity Asset Store: http://u3d.as/7AU

Description:

The result ini file will be saved in Application.persistentDataPath.
IniFile will use PlayerPrefs to store data in case of WebPlayer build.
You can also force IniFile to use PlayerPrefs by defining USE_PLAYER_PREFS macro at the beginning of "IniFile.cs" file.

Example:

void Start()
{
    IniFile ini = new IniFile("TestFile"); // ini extension appends to file name here

    ini.Set("StringKey",  "Hello, World!!!");
    ini.Set("IntKey",     123);
    ini.Set("FloatKey",   0.1f);
    ini.Set("DoubleKey",  0.1647321);
    ini.Set("BooleanKey", true);

    Debug.Log(ini.Get("StringKey"));
    Debug.Log(ini.Get("IntKey",     321));
    Debug.Log(ini.Get("FloatKey",   0.6f));
    Debug.Log(ini.Get("DoubleKey",  0.6589123));
    Debug.Log(ini.Get("BooleanKey", false));

    ini.Save("TestFile"); // ini extension appends to file name here
}



Links:

Site:              http://gris.ucoz.ru/index/inifile/0-9
Unity Asset Store: http://u3d.as/7AU
GitHub:            https://github.com/Gris87/IniFile
