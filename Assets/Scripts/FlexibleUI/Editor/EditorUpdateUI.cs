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
		if (GUILayout.Button("Check For Old Layout Elements"))
		{
			CheckForOldElements();
		}

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

	#region old ui elemetns
	private void CheckForOldElements()
	{
		Debug.Log("");
		bool oldElementsActive = false;
		oldElementsActive = ContainsOldLayouts(oldElementsActive);
		oldElementsActive = ContainsOldLayoutProps(oldElementsActive);
		oldElementsActive = ContainsOldButtons(oldElementsActive);
		oldElementsActive = ContainsOldTexts(oldElementsActive);

		if (!oldElementsActive)
		{
			Debug.Log("Non old ui elements active.");
		}
	}

	private static bool ContainsOldTexts(bool oldElementsActive)
	{
		var flexText = GameObject.FindObjectsOfType<FlexibleUITextOverride>();
		if (flexText.Length > 0)
		{
			oldElementsActive = true;

			Debug.Log("old text elements" + flexText.Length);
			foreach (FlexibleUITextOverride x in flexText)
			{
				Debug.Log(x.gameObject.name + ", parent: " + x.gameObject.transform.parent.name);
			}
		}

		return oldElementsActive;
	}

	private static bool ContainsOldButtons(bool oldElementsActive)
	{
		var flexButtons = GameObject.FindObjectsOfType<FlexibleUIButtonOverride>();
		if (flexButtons.Length > 0)
		{
			oldElementsActive = true;

			Debug.Log("old button elements: " + flexButtons.Length);
			foreach (FlexibleUIButtonOverride x in flexButtons)
			{
				Debug.Log(x.gameObject.name + ", parent: " + x.gameObject.transform.parent.name);
			}
		}

		return oldElementsActive;
	}

	private static bool ContainsOldLayoutProps(bool oldElementsActive)
	{
		var flexProps = GameObject.FindObjectsOfType<FlexibleUILayoutPropsOverride>();
		if (flexProps.Length > 0)
		{
			oldElementsActive = true;
			Debug.Log("old flex prop elements: " + flexProps.Length);
			foreach (FlexibleUILayoutPropsOverride x in flexProps)
			{
				Debug.Log(x.gameObject.name + ", parent: " + x.gameObject.transform.parent.name);
			}
		}

		return oldElementsActive;
	}

	private static bool ContainsOldLayouts(bool oldElementsActive)
	{
		var flexLayouts = GameObject.FindObjectsOfType<FlexibleUILayoutOverride>();
		if (flexLayouts.Length > 0)
		{
			oldElementsActive = true;
			Debug.Log("old layout elements: " + flexLayouts.Length);
			foreach (FlexibleUILayoutOverride x in flexLayouts)
			{
				Debug.Log(x.gameObject.name + ", parent: " + x.gameObject.transform.parent.name);
			}
		}

		return oldElementsActive;
	}
	#endregion

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