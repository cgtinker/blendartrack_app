using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

using ArRetarget;

public class SceneButton : MonoBehaviour, IPointerDownHandler
{
    //ref
    public TextMeshProUGUI mainMenuSceneTitle;
    //public Image buttonImage;
    AdditiveSceneManager sceneManager;
    TrackingDataManager trackingDataManager;
    public InputHandler inputHandler;

    //buttons
    public SceneButtonData CameraTracking;
    public SceneButtonData FaceTracking;

    private List<SceneButtonData> sceneReferences = new List<SceneButtonData>();

    private void Awake()
    {
        trackingDataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<TrackingDataManager>();
        sceneReferences.Add(FaceTracking);
        sceneReferences.Add(CameraTracking);
    }

    private void Start()
    {
        //ref + load previous scene
        sceneManager = GameObject.FindGameObjectWithTag("manager").GetComponent<AdditiveSceneManager>();
        int sceneIndex = UserPreferences.Instance.GetIntPref("scene");

        //adjust ui
        foreach (SceneButtonData data in sceneReferences)
        {
            if (data.index == sceneIndex)
            {
                mainMenuSceneTitle.text = data.titel;
                //buttonImage.sprite = data.sprite;
            }
        }

        switch (DeviceManager.Instance.device)
        {
            case DeviceManager.Device.iOS:
                if (this.gameObject.name == "swapButton")
                {
                    this.gameObject.SetActive(false);
                }

                this.gameObject.GetComponent<SceneButton>().enabled = false;
                return;
            default:
                break;
        }
    }

    //only works for face + camera tracking
    public void OnPointerDown(PointerEventData eventData)
    {
        if (trackingDataManager._recording)
        {
            string tmp = FileManagement.GetParagraph();

            inputHandler.GeneratedFilePopup("failed to switch camera" + tmp, "please finish recording");
            return;
        }

        int sceneIndex = UserPreferences.Instance.GetIntPref("scene");
        foreach (SceneButtonData data in sceneReferences)
        {
            if (data.index != sceneIndex)
            {
                //switch scene
                StartCoroutine(LoadTargetScene(data.index));
                //adjust ui
                mainMenuSceneTitle.text = data.titel;
                //buttonImage.sprite = data.sprite;
            }
        }
    }

    private IEnumerator LoadTargetScene(int index)
    {
        inputHandler.PurgeOrphanPopups();
        sceneManager.ResetArScene();
        yield return new WaitForEndOfFrame();

        sceneManager.SwitchScene(index);
        //StartCoroutine(ARSessionState.EnableAR(enabled: true));
    }

    /*
     * Depreciated method - instanting scene buttons based on addive scene manger dicts
     * 
    public string sceneName
    {
        get; private set;
    }
    public int sceneKey
    {
        get; private set;
    }

    TextMeshProUGUI title;
    public GameObject mainMenu;
    GameObject sceneMenu;


    //button gets instantiated by the inputHandler and is based on the dicts in the addaptive scene manager
    public void Init(string name, int key, AdditiveSceneManager sceneManager, GameObject mainMenu, GameObject sceneMenu, TextMeshProUGUI mainMenuSceneTitle)
    {
        //assigning properties based on the dict in the addive scene manager
        sceneName = name;
        sceneKey = key;

        this.sceneManager = sceneManager;
        this.sceneMenu = sceneMenu;
        this.mainMenu = mainMenu;
        this.mainMenuSceneTitle = mainMenuSceneTitle;

        title = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        title.text = sceneName;
    }
    */
}

[System.Serializable]
public class SceneButtonData
{
    /// <summary>
    /// titel in the main menu
    /// </summary>
    public string titel;
    /// <summary>
    /// sprite display
    /// </summary>
    public Sprite sprite;
    /// <summary>
    /// compare index to addive scene manager
    /// </summary>
    public int index;
}