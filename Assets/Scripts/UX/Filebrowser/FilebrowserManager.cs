using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ArRetarget
{
	[RequireComponent(typeof(FilebrowserEventListener))]
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
		public void Awake()
		{
			persistentPath = Application.persistentDataPath;
		}

		// generating buttons
		public void Start()
		{
			FileManager.JsonDirectories = GetUpdatedDirectories(persistentPath);

			if (FileManager.JsonDirectories.Count == 0)
				filesAvailablePopup.InstantiatePopup();

			generateJsonDirectoryButtons.GenerateButtons(FileManager.JsonDirectories);
		}

		// getting stored and active json directories
		public static List<JsonDirectory> GetUpdatedDirectories(string persistentPath)
		{
			JsonDirectoryHandler jsonDirectoryHandler = new JsonDirectoryHandler();

			var recentJsonDirectories = FileManager.GetRecentDirectories;
			var currentJsonDirectories = jsonDirectoryHandler.GetDirectories(persistentPath);

			var updatedDirectories = CompareToRecentlyStoredDirectories(
					recentJsonDirectories, currentJsonDirectories);

			return updatedDirectories;
		}

		// updating json directories
		private static List<JsonDirectory> CompareToRecentlyStoredDirectories(
			List<JsonDirectory> recentJsonDirectories,
			List<JsonDirectory> currentJsonDirectories)
		{

			for (int i = 0; i < currentJsonDirectories.Count; i++)
			{
				for (int x = 0; x < recentJsonDirectories.Count; x++)
				{
					if (currentJsonDirectories[i].jsonFilePath != null &&
						currentJsonDirectories[i].jsonFilePath == recentJsonDirectories[x].jsonFilePath)
					{
						currentJsonDirectories[i] = recentJsonDirectories[x];
						break;
					}
				}
			}

			return currentJsonDirectories;
		}

		/// clean up after leaving the file browser
		private void OnDisable()
		{
			PurgeOrphans.PurgeOrphanZips();
			PurgeOrphans.PurgeErrorMessages();
			PurgeOrphans.DestroyOrphanButtons(FileManager.JsonDirectories);
		}
	}
}
