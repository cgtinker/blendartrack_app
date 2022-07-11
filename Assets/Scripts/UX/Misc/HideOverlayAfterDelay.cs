using System.Collections;
using UnityEngine;

namespace ArRetarget
{
	public class HideOverlayAfterDelay : MonoBehaviour
	{
		// gets disabled for savety
		void OnEnable()
		{
			StartCoroutine(DisableObject());
		}

		IEnumerator DisableObject()
		{
			yield return new WaitForSeconds(3f);
			this.gameObject.SetActive(false);
		}
	}
}
