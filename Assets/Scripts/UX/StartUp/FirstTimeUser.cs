using UnityEngine;
using System.Collections;

namespace ArRetarget
{
	public class FirstTimeUser
	{
		/// <summary>
		/// activate settings
		/// </summary>
		public static void SetPlayerPrefs()
		{
			PlayerPrefs.DeleteAll();

			PlayerPrefs.SetInt("firstTimeUser", 1);
			PlayerPrefs.SetInt("default", 1);

			PlayerPrefs.SetString("scene", "Camera Tracker");
			PlayerPrefs.SetInt("tutorial", 1);

			PlayerPrefs.SetInt("hints", 1);
			PlayerPrefs.SetInt("recordCam", 1);
			PlayerPrefs.SetInt("autofocus", 1);

			PlayerPrefs.SetInt("bitrate", 8);
			PlayerPrefs.SetInt("vidzip", 1);
		}
	}
}
