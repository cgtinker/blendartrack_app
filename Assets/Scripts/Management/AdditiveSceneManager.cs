using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class AdditiveSceneManager : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_IPHONE
            device = Device.iOS;
#endif
#if UNITY_ANDROID
        device = Device.Android;
#endif
        //Debug.Log($"Device: {device}");
    }
    #endregion

    public static Dictionary<int, string> AndroidScenes = new Dictionary<int, string>
    {
        { 0, "Tutorial"},
        { 1, "Pose Data Tracker" },
        { 2, "Face Mesh Tracker" }
    };

    public static Dictionary<int, string> IOSScenes = new Dictionary<int, string>
    {
        { 0, "Tutorial"},
        { 1, "Pose Data Tracker" },
        { 2, "Shape Key Tracker" }
    };

    public void SwitchScene(int sceneIndex)
    {
        //unload the previous scene
        string preScene = GetScene(UserPreferences.Instance.GetIntPref("scene"));
        if (SceneManager.GetSceneByName(preScene).isLoaded)
            SceneManager.UnloadSceneAsync(preScene);

        //saving reference to the loaded scene
        PlayerPrefs.SetInt("scene", sceneIndex);
        string tarScene = GetScene(sceneIndex);

        //LoadScene(tarScene);
        SceneManager.LoadSceneAsync(tarScene, LoadSceneMode.Additive);
    }

    public void ResetScene()
    {
        var obj = GameObject.FindGameObjectWithTag("arSession");

        if (obj != null)
        {
            var arSession = obj.GetComponent<ARSession>();
            var inputManager = obj.GetComponent<ARInputManager>();

            arSession.Reset();
            arSession.enabled = true;
            inputManager.enabled = true;
        }


        //getting scene to reload from user preferences
        //int curScene = UserPreferences.Instance.GetIntPref("scene");
        //string tarScene = GetScene(curScene);

        //LoadScene(tarScene);
    }

    private void LoadScene(string tarScene)
    {
        //loading the target scene and adding the ui
        //SceneManager.LoadSceneAsync(tarScene, LoadSceneMode.Additive);
        //SceneManager.LoadScene("UserInterface", LoadSceneMode.Additive);

        Debug.Log("Loading Scene additive: " + tarScene);
    }

    public Dictionary<int, string> GetDeviceScenes()
    {
        //receiving scene dict depending on ui
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

    public string GetScene(int sceneKey)
    {
        //getting the scene by dictionary key
        Dictionary<int, string> tmp = GetDeviceScenes();
        string scene = tmp[sceneKey];
        return scene;
    }
}