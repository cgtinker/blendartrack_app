using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetUI : Singleton<NetUI>
{
    public GameObject startMenu;
    public InputField userName;

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        userName.interactable = false;
        NetClient.Instance.ConnectToServer();
    }
}
