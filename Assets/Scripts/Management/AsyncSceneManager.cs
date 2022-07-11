using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace ArRetarget
{
	public class AsyncSceneManager : MonoBehaviour
	{
		public static readonly string CameraTracking = "Camera Tracker";
		public static readonly string FaceTracking = "Face Mesh Tracker";
		public static readonly string StartUp = "StartUp";
		public static readonly string Filebrowser = "Filebrowser";
		public static readonly string Settings = "Settings";
		public static readonly string Tutorial = "Tutorial";
		public static readonly string JsonViewer = "JsonViewer";
		public static readonly string ArCoreSupport = "ArCoreSupport";
		public static readonly string Main = "Main";

		private static string previousLoadedScene;
		public static string loadedScene
		{
			get;
			private set;
		}

		#region SceneTypes
		private enum SceneTypes
		{
			Tracking,
			UI,
			Persistent
		}

		private static SceneTypes sceneType;

		// names have to match scene names in build settings
		private static Dictionary<string, SceneTypes> SceneTypeDict = new Dictionary<string, SceneTypes>()
		{
			{ CameraTracking, SceneTypes.Tracking },
			{ FaceTracking, SceneTypes.Tracking },
			{ StartUp, SceneTypes.UI },
			{ Filebrowser, SceneTypes.UI },
			{ Settings, SceneTypes.UI },
			{ Tutorial, SceneTypes.UI },
			{ JsonViewer, SceneTypes.UI },
			{ ArCoreSupport, SceneTypes.UI },
			{ Main, SceneTypes.Persistent }
		};
		#endregion


		public static void LoadScene(string sceneName)
		{
			if (GetSceneType(sceneName) == SceneTypes.Persistent)
				return;

			if (SceneIsLoaded(previousLoadedScene))
				UnloadSceneAsync(previousLoadedScene);

			LoadSceneAsync(sceneName);
		}

		#region Private methods

		#region Keep track on loaded Scenes
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
			int loadedScenes = GetSceneCount();
			sceneType = GetSceneType(scene.name);
			Debug.Log($"Loaded {scene.name} successfully. {loadedScenes} loaded scenes active");
			switch (sceneType)
			{
				case SceneTypes.Tracking:
				PlayerPrefsHandler.Instance.SetString("scene", scene.name);
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

			loadedScene = scene.name;
		}
		#endregion

		private static int GetSceneCount()
		{
			// keep consistently 2 scenes running (avoid overload)
			int count = SceneManager.sceneCount;
			if (count > 2)
				Debug.LogError("Scene Count: " + count);

			return count;
		}

		private static SceneTypes GetSceneType(string sceneName)
		{
			return SceneTypeDict[sceneName];
		}

		private static bool SceneIsLoaded(string sceneName)
		{
			return SceneManager.GetSceneByName(sceneName).isLoaded;
		}

		private static void UnloadSceneAsync(string sceneName)
		{
			SceneManager.UnloadSceneAsync(sceneName);
		}

		private static void LoadSceneAsync(string sceneName)
		{
			SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		}
		#endregion
	}
}