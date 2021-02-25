using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArRetarget
{
	public class StartUpAnimation : MonoBehaviour
	{
		private Animator animator;

		// Start is called before the first frame update
		void Start()
		{
			animator = this.gameObject.GetComponent<Animator>();
		}

		public void IntroAnimEnd()
		{
			Debug.Log("Test");
		}
	}
}

