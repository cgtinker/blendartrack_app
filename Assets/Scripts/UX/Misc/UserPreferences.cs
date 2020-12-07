using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// player prefs handler to set defaults and reset values
/// </summary>
public class UserPreferences : Singleton<UserPreferences>
{
    #region Reference dictionarys for default values
    private Dictionary<string, string> StringPrefDict = new Dictionary<string, string>()
    {
        { "ip", "192.0.1.0"},
        { "port", "9000" }
    };

    private Dictionary<string, int> IntPrefDict = new Dictionary<string, int>()
    {
        { "scene", 1 },     //sets startup scene and saves last loaded scene
        //tracker references
        { "record", 0 },    //must be 1 to be used (recording video)
        { "intrinsics", 0 },//must be 1 to be used (recording intrinsic data)
        { "cloud", 0 },     //must be 1 to be used (recording point cloud data)
        { "face", 1 },      //always used if available
        { "camera", 1 },    //always used if available
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
