using UnityEngine;
using System.Collections.Generic;
using System;

namespace ArRetarget
{
	public class GenerateJsonDirectoryButtons : MonoBehaviour
	{
		[SerializeField]
		private GameObject JsonFileButtonPrefab = null;
		[SerializeField]
		private Transform JsonFileButtonParent = null;

		/// <summary>
		/// generating the buttons on opening file browser
		/// </summary>
		public bool GenerateButtons(List<JsonDirectory> JsonDirectories)
		{
			Debug.Log("Generating preview buttons");
			if (JsonDirectories.Count != 0)
			{
				InstantiatingButtons(JsonDirectories);
				return true;
			}

			return false;
		}

		private void InstantiatingButtons(List<JsonDirectory> JsonDirectories)
		{
			//create buttons
			for (int i = 0; i < JsonDirectories.Count; i++)
			{
				//set file data index
				JsonDirectories[i].index = i;
				//set json file data obj
				var jsonFileBtnObj = Instantiate(JsonFileButtonPrefab, Vector3.zero, Quaternion.identity);
				JsonDirectories[i].obj = jsonFileBtnObj;
				jsonFileBtnObj.name = JsonDirectories[i].dirName;
				//setting parent
				jsonFileBtnObj.transform.SetParent(JsonFileButtonParent);
				//setting scale
				jsonFileBtnObj.transform.localScale = Vector3.one;

				//passing data to the button script
				var fileButtonScript = jsonFileBtnObj.GetComponent<JsonFileButton>();
				fileButtonScript.InitFileButton(JsonDirectories[i], gameObject.GetComponent<FileBrowserEventManager>());
			}
		}
	}
}
