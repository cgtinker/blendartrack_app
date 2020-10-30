using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneManager : MonoBehaviour
{

    #region SINGLETON PATTERN
    private static AdditiveSceneManager _instance;
    public static AdditiveSceneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<AdditiveSceneManager>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("SceneManager");
                    _instance = container.AddComponent<AdditiveSceneManager>();
                }
            }

            return _instance;
        }
    }

    #endregion

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
        Debug.Log($"Device: {device}");
    }
    #endregion

    private static int curScene
    {
        get;
        set;
    }

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
        curScene = sceneIndex;
        string tarScene = GetScene(sceneIndex);
        Debug.Log("switching: " + tarScene + " || " + curScene);

        LoadScene(tarScene);
    }

    public void ReloadScene()
    {
        Debug.Log("fetching string");
        string tarScene = GetScene(curScene);
        Debug.Log("reloading: " + tarScene + " || " + curScene);
        LoadScene(tarScene);
    }

    private void LoadScene(string tarScene)
    {
        //loading the target scene and adding the ui
        SceneManager.LoadSceneAsync(tarScene);
        SceneManager.LoadSceneAsync("UserInterface", LoadSceneMode.Additive);
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

    private string GetScene(int sceneKey)
    {
        //getting the scene by dictionary key
        Dictionary<int, string> tmp = GetDeviceScenes();
        string scene = tmp[sceneKey];
        return scene;
    }
}