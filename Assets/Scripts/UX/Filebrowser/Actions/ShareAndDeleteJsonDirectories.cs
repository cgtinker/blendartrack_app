using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace ArRetarget
{
	public class ShareAndDeleteJsonDirectories : MonoBehaviour
	{
		//delete selected files
		public void OnDeleteSelectedFiles(List<JsonDirectory> JsonDirectories)
		{
			List<string> selectedFiles = GetSelectedDirectories(JsonDirectories);

			if (selectedFiles.Count <= 0)
				return;

			else
				FileManagement.DeleteDirectories(selectedFiles);
		}

		//native share event for selected files
		public void OnShareSelectedFiles(
			GameObject LoadingScreen, TextMeshProUGUI LoadingMessage, List<JsonDirectory> JsonDirectories)
		{
			//ref selected file
			List<string> selectedDirNames = GetSelectedDirectoryNames(JsonDirectories);
			List<string> selectedDirPaths = GetSelectedDirectories(JsonDirectories);

			if (selectedDirNames.Count <= 0)
			{
				LogManager.Instance.Log(
					"No Files selected. <br>Please select a file and try again.",
					LogManager.Message.Warning);
				return;
			}

			else
			{
				LoadingScreen.SetActive(true);
				LoadingMessage.text = "zipping files...";
				StartCoroutine(CompressDataForceLoadingScreen(selectedDirNames, selectedDirPaths, LoadingScreen));
			}
		}

		private IEnumerator CompressDataForceLoadingScreen(
			List<string> selectedDirNames, List<string> selectedDirPaths,
			GameObject LoadingScreen)
		{
			LoadingScreen.SetActive(true);

			yield return new WaitForSeconds(0.2f);
			//date time reference
			string localDate = FileManagement.GetDateTimeText();

			//path to generated or existing zip
			string zip = FileManagement.CompressDirectories(selectedDirPaths);

			//listing files to transfer for subject message
			string filenames = "";
			var paragraph = FileManagement.GetParagraph();
			foreach (string filename in selectedDirNames)
			{
				var curFilename = filename + paragraph;
				filenames += curFilename;
			}

			//setting up share message / text
			string subject = "blendartrack " + localDate;
			string text = "" + "blendartrack " + localDate + paragraph + paragraph +
				"Attached Files: " + paragraph + filenames;
			LoadingScreen.SetActive(false);

			//share data
			FileManagement.ShareZip(zip, subject, text);
		}

		#region get selected files
		/// <summary>
		/// get all selected dir names
		/// </summary>
		/// <returns></returns>
		private List<string> GetSelectedDirectoryNames(List<JsonDirectory> JsonDirectories)
		{
			List<string> tmp_list = new List<string>();

			foreach (JsonDirectory data in JsonDirectories)
			{
				if (data.active)
				{
					tmp_list.Add(data.dirName);
				}
			}

			return tmp_list;
		}

		/// <summary>
		/// get all selected directory paths
		/// </summary>
		/// <returns></returns>
		private List<string> GetSelectedDirectories(List<JsonDirectory> JsonDirectories)
		{
			List<string> tmp_list = new List<string>();

			foreach (JsonDirectory data in JsonDirectories)
			{
				if (data.active)
				{
					tmp_list.Add(data.dirPath);
				}
			}

			return tmp_list;
		}
		#endregion
	}
}