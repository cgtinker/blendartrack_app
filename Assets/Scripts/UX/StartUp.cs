using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUp : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(false);

        AdditiveSceneManager.Instance.SwitchScene(0);
    }
}
