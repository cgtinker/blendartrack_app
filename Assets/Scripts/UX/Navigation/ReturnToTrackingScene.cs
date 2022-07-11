using UnityEngine;
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
