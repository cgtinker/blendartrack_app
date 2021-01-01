using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ArRetarget
{
    public class WireframeTextureSwitcher : MonoBehaviour
    {
        public Texture iosWireframe;
        public Texture androidWireframe;
        Material material;

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
