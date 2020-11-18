using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PopUpDisplay : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI tmp_text;
    public RectTransform rectTransform;
    public GameObject desitionation;

    public string text;
    public float staticDuration;
    public float travelDuration;
    private bool traveling;

    public enum PopupType
    {
        Travel,
        Notification,
        ButtonEvent
    }

    public PopupType type;

    public void DisplayPopup(Transform parent)
    {
        tmp_text.text = text;
        SetTransform(parent);

        switch (type)
        {
            case PopupType.Travel:
                Debug.Log("start");
                StartCoroutine(LerpToObject(desitionation));
                break;

            case PopupType.Notification:
                StartCoroutine(DisablePopupTimer(staticDuration));
                break;

            case PopupType.ButtonEvent:
                break;
        }
    }

    private IEnumerator DisablePopupTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        Debug.Log("Removed Popup");
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (!traveling)
            return;

        currentTravelTime += Time.deltaTime;
    }

    private void SetTransform(Transform parent)
    {
        //setting popup position
        /*
        var rect = this.GetComponent<RectTransform>();
        rect.transform.position = Vector3.zero;
        rect.anchoredPosition = new Vector2(0.5f, 0.5f);
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        */
        this.transform.SetParent(parent);
    }

    public float currentTravelTime = 0; // actual floating time 
    private IEnumerator LerpToObject(GameObject target)
    {
        yield return new WaitForSeconds(staticDuration);
        Camera cam = GameObject.FindGameObjectWithTag("UI_Camera").GetComponent<Camera>();
        //receiving positions
        var rectStart = this.gameObject.GetComponent<RectTransform>();
        var startPos = Camera.main.WorldToScreenPoint(rectStart.transform.position);

        //var startPos = rectStart.transform.TransformVector(rectStart.transform.position);

        // var startPos = rectStart.TransformVector(rectStart.transform.position);
        //var startPos = this.gameObject.GetComponent<RectTransform>().transform.position;

        var rectDest = target.GetComponent<RectTransform>();
        var destination = Camera.main.WorldToScreenPoint(rectDest.transform.position);

        Debug.Log(startPos);
        Debug.Log(destination);
        //var destination = rectDest.TransformVector(rectDest.transform.position);
        // var destination = rectDest.transform.TransformVector(rectDest.transform.position);
        //var destination = target.GetComponent<RectTransform>().transform.position;



        //disableing popup after travel
        StartCoroutine(DisablePopupTimer(travelDuration));

        //traveling
        while (currentTravelTime <= travelDuration)
        {
            traveling = true;

            var normalizedValue = currentTravelTime / travelDuration; // normalize traveltime
            //rectTransform.transform.localScale = new Vector3(1 / normalizedValue, 1 / normalizedValue, 1 / normalizedValue); //scale

            rectTransform.anchoredPosition = Vector3.Lerp(startPos, destination, normalizedValue);
            yield return null;
        }

    }
}
