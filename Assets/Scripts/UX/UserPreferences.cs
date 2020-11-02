using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPreferences : Singleton<UserPreferences>
{
    //key, value
    #region Reference dictionarys for default values
    private Dictionary<string, string> StringPrefDict = new Dictionary<string, string>()
    {
        { "ip", "192.0.1.0"},
        { "port", "9000" }
    };

    //scene - additiveSceneManager
    private Dictionary<string, int> IntPrefDict = new Dictionary<string, int>()
    {
        { "scene", 0 },
    };

    private Dictionary<string, float> FloatPrefDict = new Dictionary<string, float>()
    {
        {"volume", 1.0f },
    };
    #endregion

    #region Get Values
    public float GetFloatPref(string key)
    {
        float defaultValue = FloatPrefDict[key];
        float m_value = PlayerPrefs.GetFloat(key: key, defaultValue: defaultValue);
        return m_value;
    }

    public int GetIntPref(string key)
    {
        int defaultValue = IntPrefDict[key];
        int m_int = PlayerPrefs.GetInt(key: key, defaultValue: defaultValue);
        return m_int;
    }

    public string GetStringPref(string key)
    {
        string defaultValue = StringPrefDict[key];
        string m_string = PlayerPrefs.GetString(key, defaultValue: defaultValue);
        return m_string;
    }
    #endregion

    private void SavePreferences()
    {
        PlayerPrefs.Save();
    }
}
