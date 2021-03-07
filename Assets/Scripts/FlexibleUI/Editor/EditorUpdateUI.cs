using UnityEngine;
using UnityEditor;

public class EditorUpdateUI : EditorWindow
{
	[MenuItem("Window/Update Flexible UI Data")]
	public static void ShowWindow()
	{
		GetWindow<EditorUpdateUI>("Update Flex UI");
	}

	private void OnGUI()
	{
		if (GUILayout.Button("Flex Button Update"))
		{
			UpdateFlexButtons();
		}

		if (GUILayout.Button("Flex Layout Update"))
		{
			UpdateFlexLayouts();
		}

		if (GUILayout.Button("Flex Prop Update"))
		{
			UpdateFlexProps();
		}

		if (GUILayout.Button("Flex Text Update"))
		{
			UpdateFlexText();
		}

		if (GUILayout.Button("Flex Padding Update"))
		{
			UpdateFlexPadding();
		}

		if (GUILayout.Button("Update all"))
		{
			UpdateFlexButtons();
			UpdateFlexLayouts();
			UpdateFlexProps();
			UpdateFlexText();
			UpdateFlexPadding();
		}
	}

	#region update flex
	private void UpdateFlexButtons()
	{
		var flexButtons = GameObject.FindObjectsOfType<FlexButtonOverride>();

		foreach (FlexButtonOverride x in flexButtons)
		{
			x.UpdateUI();
		}
	}

	private void UpdateFlexLayouts()
	{
		var flexButtons = GameObject.FindObjectsOfType<FlexLayoutOverride>();

		foreach (FlexLayoutOverride x in flexButtons)
		{
			x.UpdateUI();
		}
	}

	private void UpdateFlexText()
	{
		var flexButtons = GameObject.FindObjectsOfType<FlexTextOverride>();

		foreach (FlexTextOverride x in flexButtons)
		{
			x.UpdateUI();
		}
	}

	private void UpdateFlexProps()
	{
		var flexButtons = GameObject.FindObjectsOfType<FlexPropsOverride>();

		foreach (FlexPropsOverride x in flexButtons)
		{
			x.UpdateUI();
		}
	}

	private void UpdateFlexPadding()
	{
		var flexPadding = GameObject.FindObjectsOfType<FlexPaddingOverride>();

		foreach (FlexPaddingOverride x in flexPadding)
		{
			x.UpdateUI();
		}
	}
	#endregion
}