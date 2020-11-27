using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace ArRetarget
{
    public static class ARSessionState
    {
        public static IEnumerator EnableAR(bool enabled)
        {
            var m_arSession = GameObject.FindGameObjectWithTag("arSession");
            var arSessionOrigin = GameObject.FindGameObjectWithTag("arSessionOrigin");

            if (m_arSession != null)
            {
                var arSession = m_arSession.GetComponent<ARSession>();
                var inputManager = m_arSession.GetComponent<ARInputManager>();

                if (enabled)
                {
                    arSession.Reset();
                }

                yield return new WaitForEndOfFrame();

                if (arSessionOrigin != null)
                {
                    arSessionOrigin.SetActive(enabled);
                }

                arSession.enabled = enabled;
                inputManager.enabled = enabled;

            }

            else
                Debug.LogError("ArSession getting called and cannot be found");
        }
    }
}
