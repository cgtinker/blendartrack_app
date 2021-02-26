using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArRetarget
{
	public class StartUpAnimation : MonoBehaviour
	{
		private void Start()
		{
			if (PlayerPrefs.GetInt("firstTime", -1) == -1)
			{
				FirstTimeUser.SetPlayerPrefs();
			}
		}

		public void IntroAnimEnd()
		{
			LoadTargetScene();
		}

		private void LoadTargetScene()
		{
			if (PlayerPrefs.GetInt("tutorial", -1) == 1)
				StateMachine.Instance.SetState(StateMachine.State.Tutorial);

			else
				StateMachine.Instance.SetState(StateMachine.State.RecentTracking);
		}

		private void CheckArCoreSupport()
		{
			//check if ar core is installed
			if (DeviceManager.Instance.device == DeviceManager.Device.Android)
			{
				//SupportChecker.SetActive(true);
			}

			else
			{
				//SupportChecker.SetActive(false);
			}
		}
	}
}

