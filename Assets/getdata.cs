using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArRetarget;

public class getdata : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var m_str = FileManagementHelper.GetDateTime();
        Debug.Log(m_str);

        var vStr = FileManagementHelper.GetDay();
        Debug.Log(vStr);

        Debug.Log(Application.persistentDataPath);
    }

}
