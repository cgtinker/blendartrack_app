using UnityEngine;

namespace ArRetarget
{
	public class RecordButton : MonoBehaviour
	{
		[SerializeField]
		private Animator animator;

		[SerializeField]
		private GameObject Timer;
		private TimeCounter counter;

		private bool recording;

		// Start is called before the first frame update
		void Start()
		{
			counter = Timer.GetComponent<TimeCounter>();
			recording = false;
		}

		//play micro interaction and handel tracking
		public void OnToggleRecording()
		{
			recording = !recording;

			if (recording)
			{
				//rec button pressed
				animator.Play("rec");
				Timer.SetActive(true);
				counter.StartTimer();
			}

			else
			{
				//break button pressed
				animator.Play("stop");
				Timer.SetActive(false);
				counter.StopTimer();
			}
		}
	}
}