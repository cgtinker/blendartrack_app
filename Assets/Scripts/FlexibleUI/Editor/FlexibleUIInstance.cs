using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class FlexibleUIInstance : Editor
{
    static GameObject clickedObject;

    [MenuItem("GameObject/FlexibleUI/Button", priority = 0)]
    public static void AddButton()
    {
        var obj = Create("Button");
    }

    public static GameObject Create(string objectName)
    {
        GameObject instance = Instantiate(Resources.Load<GameObject>(objectName));
        instance.name = objectName;
        clickedObject = UnityEditor.Selection.activeGameObject as GameObject;

        if (clickedObject != null)
        {
            instance.transform.SetParent(clickedObject.transform, false);
        }

        return instance;
    }
}
