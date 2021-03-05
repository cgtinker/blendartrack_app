using System;
using UnityEngine;
using System.Collections;

namespace ArRetarget
{
	public class DirectJsonPreview : MonoBehaviour
	{
		public void OpenJsonPreview()
		{
			JsonDirectoryHandler jsonDirectoryHandler = new JsonDirectoryHandler();
			var dirs = jsonDirectoryHandler.GetDirectories(Application.persistentDataPath);

			if (dirs[0].jsonSize < 65 && !String.IsNullOrEmpty(dirs[0].jsonFilePath))
			{
				StartCoroutine(OpenPreview(dirs[0]));
			}

			else if (dirs[0].jsonSize > 65)
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
			FileManager.JsonPreview = dir;
			FileManager.InstantPreview = true;
			yield return new WaitForSeconds(0.2f);
			StateMachine.Instance.SetState(StateMachine.State.JsonViewer);
		}
	}
}
