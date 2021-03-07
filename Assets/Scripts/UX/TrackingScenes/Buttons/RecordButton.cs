using UnityEngine;

namespace ArRetarget
{
	public class RecordButton : MonoBehaviour
	{
		[SerializeField]
		private Animator animator = null;
		private bool recording;


		//play micro interaction and handel tracking
		public void OnToggleRecording()
		{
			recording = !recording;

			if (recording)
			{
				//rec button pressed
				animator.Play("rec");
			}

			else
			{
				//break button pressed
				animator.Play("stop");
			}
		}
	}
}