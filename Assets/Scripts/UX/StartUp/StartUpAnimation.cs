using UnityEngine;

namespace ArRetarget
{
	public class StartUpAnimation : MonoBehaviour
	{
		public void IntroAnimEnd()
		{
			LoadTargetScene();
		}

		private void LoadTargetScene()
		{
			StateMachine.Instance.SetState(StateMachine.State.PostStartUp);
		}
	}
}

