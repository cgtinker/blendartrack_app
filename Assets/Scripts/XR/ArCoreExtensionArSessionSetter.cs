using UnityEngine;
using Google.XR.ARCoreExtensions;
using UnityEngine.XR.ARFoundation;

namespace ArRetarget
{

	public class ArCoreExtensionArSessionSetter : MonoBehaviour
	{
		private void Awake()
		{
			ARCoreExtensions manager = this.gameObject.GetComponent<ARCoreExtensions>();
			ARSession session = GameObject.FindGameObjectWithTag("arSession").GetComponent<ARSession>();
			manager.Session = session;
		}
	}
}

