using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace ArRetarget
{
	public class AsyncSceneManager : MonoBehaviour
	{
		private static string previousLoadedScene;

		#region SceneTypes
		public enum SceneTypes
		{
			Tracking,
			UI,
			Persistent
		}

		private static SceneTypes sceneType;

		private static Dictionary<string, SceneTypes> SceneTypeDict = new Dictionary<string, SceneTypes>()
		{
			{ "Pose Data Tracker", SceneTypes.Tracking },
			{ "Face Mesh Tracker", SceneTypes.Tracking },
			{ "StartUp", SceneTypes.UI },
			{ "Filebrowser", SceneTypes.UI },
			{ "Settings", SceneTypes.UI },
			{ "Tutorial", SceneTypes.UI },
			{ "PersistentData", SceneTypes.Persistent }
		};

		private static SceneTypes CheckSceneType(string sceneName)
		{
			sceneType = SceneTypeDict[sceneName];
			return sceneType;
		}
		#endregion

		#region on load scene store name
		private void OnEnable()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			Debug.Log($"Scene {mode} loaded: {scene.name}");
			int sceneCount = GetSceneCount();
			switch (sceneType)
			{
				case SceneTypes.Tracking:
				PlayerPrefs.SetString("scene", "scene.name");
				previousLoadedScene = scene.name;
				break;

				case SceneTypes.UI:
				previousLoadedScene = scene.name;
				break;

				case SceneTypes.Persistent:
				// only loads once at start
				break;

				default:
				previousLoadedScene = scene.name;
				break;
			}
		}
		#endregion

		public static void LoadSceneBeforeQuit()
		{
			string sceneBeforeQuit = PlayerPrefs.GetString("scene", "Pose Data Tracker");
			LoadScene(sceneBeforeQuit);
		}

		public static void LoadScene(string sceneName)
		{
			if (CheckSceneType(sceneName) == SceneTypes.Persistent)
				return;

			if (SceneIsLoaded(previousLoadedScene))
				UnloadSceneAsync(previousLoadedScene);

			LoadSceneAsync(sceneName);
		}

		public static int GetSceneCount()
		{
			int count = SceneManager.sceneCount;
			if (count > 3)
				Debug.LogError("Scene Count: " + count);

			return count;
		}

		private static bool SceneIsLoaded(string sceneName)
		{
			if (SceneManager.GetSceneByName(sceneName).isLoaded)
				return true;

			else
				return false;
		}

		private static void UnloadSceneAsync(string sceneName)
		{
			SceneManager.UnloadSceneAsync(sceneName);
		}

		private static void LoadSceneAsync(string sceneName)
		{
			SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		}
	}
}