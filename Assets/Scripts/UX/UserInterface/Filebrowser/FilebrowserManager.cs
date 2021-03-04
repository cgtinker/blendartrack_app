using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ArRetarget
{
	public class FilebrowserManager : MonoBehaviour
	{
		public static string persistentPath;

		//[SerializeField]
		//private JsonDirectoryHandler jsonDirectoryHandler = null;
		[SerializeField]
		private GenerateJsonDirectoryButtons generateJsonDirectoryButtons = null;
		[SerializeField]
		private FilebrowserNoFilesAvailablePopup filesAvailablePopup = null;

		// referencing all stored directories containing (valid) .jsons
		private void Awake()
		{
			persistentPath = Application.persistentDataPath;
			List<JsonDirectory> currentJsonDirectories = GetDirectories();
			FileManager.JsonDirectories = currentJsonDirectories;

			if (FileManager.JsonDirectories.Count == 0)
				filesAvailablePopup.InstantiatePopup();
		}

		// generating buttons
		private void Start()
		{
			generateJsonDirectoryButtons.GenerateButtons(FileManager.JsonDirectories);
		}

		// getting stored and active json directories
		private static List<JsonDirectory> GetDirectories()
		{
			JsonDirectoryHandler jsonDirectoryHandler = new JsonDirectoryHandler();

			var recentJsonDirectories = FileManager.GetRecentDirectories;
			var currentJsonDirectories = jsonDirectoryHandler.GetDirectories(persistentPath);

			if (recentJsonDirectories.Count != currentJsonDirectories.Count)
			{
				var updatedDirectories = CompareToRecentlyStoredDirectories(recentJsonDirectories, currentJsonDirectories);

				return updatedDirectories;
			}

			for (int i = 0; i < currentJsonDirectories.Count; i++)
			{
				if (recentJsonDirectories[i].dirPath != currentJsonDirectories[i].dirPath)
				{
					return currentJsonDirectories;
				}
			}

			return recentJsonDirectories;
		}

		// updating json directories
		private static List<JsonDirectory> CompareToRecentlyStoredDirectories(List<JsonDirectory> recentJsonDirectories, List<JsonDirectory> currentJsonDirectories)
		{
			int diff = currentJsonDirectories.Count - recentJsonDirectories.Count;

			for (int i = diff; i < currentJsonDirectories.Count; i++)
			{
				currentJsonDirectories[i] = recentJsonDirectories[i - 3];
			}

			return currentJsonDirectories;
		}

		/// clean up after leaving the file browser
		private void OnDisable()
		{
			PurgeOrphans.PurgeOrphanZips();
			PurgeOrphans.PurgeErrorMessages();
			PurgeOrphans.DestroyOrphanButtons(FileManager.JsonDirectories);
			//FileManager.JsonDirectories.Clear();
		}
	}
}
