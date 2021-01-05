using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOverlayAfterDelay : MonoBehaviour
{
    // gets disabled for savety
    void OnEnable()
    {
        StartCoroutine(DisableObject());
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);
    }
}
