using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class RuntimeButton : MonoBehaviour, IPointerDownHandler
{
    TextMeshProUGUI title;
    TextMeshProUGUI mainMenuSceneTitle;
    AdditiveSceneManager sceneManager;
    GameObject mainMenu;
    GameObject sceneMenu;

    public string sceneName
    {
        get; private set;
    }
    public int sceneKey
    {
        get; private set;
    }

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

    public void OnPointerDown(PointerEventData eventData)
    {
        sceneManager.SwitchScene(sceneKey);
        sceneManager.ResetScene();
        mainMenuSceneTitle.text = sceneName;

        sceneMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
