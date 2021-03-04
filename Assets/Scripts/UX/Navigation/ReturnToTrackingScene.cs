using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ArRetarget
{
	[RequireComponent(typeof(Button))]
	public class ReturnToTrackingScene : MonoBehaviour
	{
		public void ReturnToTracking()
		{
			StateMachine.Instance.SetState(StateMachine.State.RecentTracking);
		}
	}
}
