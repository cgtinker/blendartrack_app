using UnityEngine;
using System.Collections;

namespace ArRetarget
{
    public class CameraPoseImporter : MonoBehaviour
    {
        public UpdateViewerDataHandler viewHandler;

        //generating the necessary obj for viewing
        public IEnumerator Init(CameraPoseContainer data)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.parent = this.gameObject.transform;

            yield return new WaitForEndOfFrame();
            StartCoroutine(UpdatePose(obj, data));
        }

        //updating based on the jsonViewerHandler
        private IEnumerator UpdatePose(GameObject obj, CameraPoseContainer data)
        {
            var m_ref = data.cameraPoseList[viewHandler.frame];

            var pos = new Vector3(m_ref.pos.x, m_ref.pos.y, m_ref.pos.z);
            var rot = new Vector3(m_ref.rot.x, m_ref.rot.y, m_ref.rot.z);

            obj.transform.position = pos;
            obj.transform.rotation = Quaternion.Euler(rot);

            yield return new WaitForEndOfFrame();

            StartCoroutine(UpdatePose(obj, data));
        }
    }
}
