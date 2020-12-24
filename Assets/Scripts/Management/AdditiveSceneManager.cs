using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using ArRetarget;
using System;

/// <summary>
/// containting scene dictionarys
/// dict strings got to match the scene names
/// all scenes got to be added in the build settings
/// matching dic keys improves crossplatform handling
/// </summary>
public class AdditiveSceneManager : MonoBehaviour
{
    public TrackingDataManager trackingDataManager;

    //depending on the device, different scenes will be available
    #region Device Management
    public enum Device
    {
        iOS,
        Android
    };

    public Device device
    {
        get;
        set;
    }

    //setting device
    private void Awake()
    {
        trackingDataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<TrackingDataManager>();
#if UNITY_IPHONE
        device = Device.iOS;
#endif
#if UNITY_ANDROID
        device = Device.Android;
#endif
#if UNITY_EDITOR
        device = Device.Android;
#endif
    }
    #endregion

    #region Scene Dicts
    public static Dictionary<int, string> AndroidScenes = new Dictionary<int, string>
    {
        //{ 0, "Tutorial"},
        { 1, "Pose Data Tracker" },
        { 2, "Face Mesh Tracker" }
    };

    public static Dictionary<int, string> IOSScenes = new Dictionary<int, string>
    {
        //{ 0, "Tutorial"},
        { 1, "Pose Data Tracker" },
        { 2, "Shape Key Tracker" }
    };
    #endregion

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        string tarScene = GetScene(UserPreferences.Instance.GetIntPref("scene"));

        if (tarScene == scene.name)
            switchingScene = false;
    }

    private bool switchingScene = false;
    //the scenne switch uses an int to receive the scene input, to allow crossplatform handling
    public void SwitchScene(int sceneIndex)
    {
        if (!switchingScene)
        {
            switchingScene = true;
            //unload the previous scene (stored in the user preferences)
            string preScene = GetScene(UserPreferences.Instance.GetIntPref("scene"));
            if (SceneManager.GetSceneByName(preScene).isLoaded)
                SceneManager.UnloadSceneAsync(preScene);

            else
                Debug.Log("User resetted app or uses first time");


            trackingDataManager.ResetTrackerInterfaces();
            //saving reference to the loaded scene (can be received in the userPrefs)
            PlayerPrefs.SetInt("scene", sceneIndex);
            string tarScene = GetScene(sceneIndex);

            //loading the target scene
            SceneManager.LoadSceneAsync(tarScene, LoadSceneMode.Additive);
        }
    }

    public void ReloadScene()
    {
        string preScene = GetScene(UserPreferences.Instance.GetIntPref("scene"));
        var sceneDict = GetDeviceScenes();

        foreach (KeyValuePair<int, string> pair in sceneDict)
        {
            if (pair.Value == preScene)
                SwitchScene(pair.Key);
        }
    }

    public void ResetArScene()
    {
        StartCoroutine(ArRetarget.ARSessionState.EnableAR(enabled: true));
    }

    public Dictionary<int, string> GetDeviceScenes()
    {
        switch (device)
        {
            case Device.Android:
                return AndroidScenes;

            case Device.iOS:
                return IOSScenes;

            default:
                return IOSScenes;
        }
    }

    //getting the scene name by the relative int
    public string GetScene(int sceneKey)
    {
        //getting the scene by dictionary key
        Dictionary<int, string> tmp = GetDeviceScenes();
        string scene = tmp[sceneKey];
        return scene;
    }
}