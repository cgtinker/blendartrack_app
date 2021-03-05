using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ArRetarget
{
	public class ReturnToTrackingScene : MonoBehaviour
	{
		public void ReturnToTracking()
		{
			StateMachine.Instance.SetState(StateMachine.State.RecentTracking);
		}
	}
}
