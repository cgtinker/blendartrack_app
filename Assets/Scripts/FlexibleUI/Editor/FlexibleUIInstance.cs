using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class FlexibleUIInstance : Editor
{
	static GameObject clickedObject;

	[MenuItem("GameObject/FlexibleUI/FlexButton", priority = 0)]
	public static void AddButton()
	{
		var obj = Create("FlexButton");
	}

	[MenuItem("GameObject/FlexibleUI/Text", priority = 0)]
	public static void AddText()
	{
		var obj = Create("Text");
	}

	[MenuItem("GameObject/FlexibleUI/Vertical Layout", priority = 0)]
	public static void AddVerticalLayout()
	{
		var obj = Create("VerticalLayout");
	}

	[MenuItem("GameObject/FlexibleUI/Horizontal Layout", priority = 0)]
	public static void AddHorizontalLayout()
	{
		var obj = Create("HorizontalLayout");
	}

	[MenuItem("GameObject/FlexibleUI/Header Button Parent", priority = 0)]
	public static void AddHeaderButtonParent()
	{
		var obj = Create("HeaderButtonParent");
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
