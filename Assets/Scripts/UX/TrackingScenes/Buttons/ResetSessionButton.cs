using UnityEngine;

namespace ArRetarget
{
	public class ResetSessionButton : MonoBehaviour
	{
		public void ResetArSession()
		{
			ARSessionState.EnableAR(false);
			ARSessionState.EnableAR(true);
		}
	}
}
