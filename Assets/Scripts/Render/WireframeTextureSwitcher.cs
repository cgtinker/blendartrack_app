using UnityEngine;

namespace ArRetarget
{
	[RequireComponent(typeof(MeshRenderer))]
	public class WireframeTextureSwitcher : MonoBehaviour
	{
		public Texture iosWireframe;
		public Texture androidWireframe;
		Material material;

		//switches face mesh material for iOS / Android on instantiation
		private void Awake()
		{
			material = gameObject.GetComponent<MeshRenderer>().material;
			//material = this.gameObject.GetComponent<Material>();
#if UNITY_IOS
            material.mainTexture = iosWireframe;
#endif
#if UNITY_ANDROID
			material.mainTexture = androidWireframe;
#endif
		}
	}

}
