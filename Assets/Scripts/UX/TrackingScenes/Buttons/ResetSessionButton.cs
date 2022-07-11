using UnityEngine;

namespace ArRetarget
{
	public class ResetSessionButton : MonoBehaviour
	{
		public void ResetArSession()
		{
			StateMachine.Instance.SetState(StateMachine.State.RecentTracking);
		}
	}
}
