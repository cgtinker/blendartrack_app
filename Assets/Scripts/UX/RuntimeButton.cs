using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class RuntimeButton : MonoBehaviour, IPointerDownHandler
{
    TextMeshProUGUI title;

    public string sceneName
    {
        get; private set;
    }
    public int sceneKey
    {
        get; private set;
    }

    public void Init(string name, int key)
    {
        //assigning properties based on the dict in the addive scene manager
        sceneName = name;
        sceneKey = key;

        title = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        title.text = sceneName;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AdditiveSceneManager.Instance.SwitchScene(sceneKey);
    }
}
