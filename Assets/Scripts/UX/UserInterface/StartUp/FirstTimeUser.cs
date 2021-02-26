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
			PlayerPrefs.SetInt("firstTime", 1);
			PlayerPrefs.SetInt("scene", 1);
			PlayerPrefs.SetInt("tutorial", 1);
			PlayerPrefs.SetInt("hints", 1);
			PlayerPrefs.SetInt("reference", 1);
			PlayerPrefs.SetInt("recordCam", 1);
			PlayerPrefs.SetInt("vidzip", 1);
		}
	}
}
