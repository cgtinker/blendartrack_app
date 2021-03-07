using System;
using UnityEngine;
using System.Collections;

namespace ArRetarget
{
	public class DirectJsonPreview : MonoBehaviour
	{
		public void OpenJsonPreview()
		{
			FileManager.JsonDirectories = FilebrowserManager.GetUpdatedDirectories(Application.persistentDataPath);

			if (FileManager.JsonDirectories[0].jsonSize < 65 && !String.IsNullOrEmpty(FileManager.JsonDirectories[0].jsonFilePath))
			{
				StartCoroutine(OpenPreview(FileManager.JsonDirectories[0]));
			}

			else if (FileManager.JsonDirectories[0].jsonSize > 65)
			{
				LogManager.Instance.Log("The recording size is to large for the in app viewer. But you can still import the data in blender!", LogManager.Message.Warning);
			}

			else
			{
				LogManager.Instance.Log("The filepath seems to be corupted. Try to import it in Blender, if that doesn't work drop me a message.", LogManager.Message.Error);
			}
		}

		private IEnumerator OpenPreview(JsonDirectory dir)
		{
			dir.viewed = true;
			FileManager.ChangeDirectoryProperties(dir);
			FileManager.JsonPreview = dir;
			FileManager.InstantPreview = true;
			yield return new WaitForSeconds(0.3f);
			StateMachine.Instance.SetState(StateMachine.State.JsonViewer);
		}
	}
}
