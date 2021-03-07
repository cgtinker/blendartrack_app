using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ArRetarget
{
    public class CameraPoseImporter : MonoBehaviour, IInitViewer<CameraPoseContainer>, IUpdate<GameObject, CameraPoseContainer>
    {
        public UpdateViewerData updateViewerData;

        //generating the necessary obj for viewing
        public IEnumerator InitViewer(CameraPoseContainer data)
        {
            //generating the obj
            GameObject obj = GenerateGismos();
            obj.transform.parent = this.gameObject.transform;

            if (data.cameraPoseList.Count > 0)
            {
                //setting the endframe of the animation
                updateViewerData.SetFrameEnd(data.cameraPoseList.Count);

                yield return new WaitForEndOfFrame();
                StartCoroutine(UpdateData(obj, data));
                obj.SetActive(true);
            }

            else
            {
                Debug.LogWarning("recording doesn't have contents");
            }
        }

        private GameObject GenerateGismos()
        {
            var obj = Instantiate(Resources.Load("TrailGizmos", typeof(GameObject)) as GameObject);
            obj.SetActive(false);

            return obj;
        }

        //updating the pose data based on the jsonViewerHandler
        public IEnumerator UpdateData(GameObject obj, CameraPoseContainer data)
        {
            //if last frame restart particle emission
            if (data.cameraPoseList.Count - 1 == updateViewerData.frame)
            {
                var emitter = obj.GetComponent<ParticleSystem>();
                emitter.Clear();
            }

            //data at current frame
            var m_ref = data.cameraPoseList[updateViewerData.frame];
            var pos = new Vector3((float)m_ref.pos.x, (float)m_ref.pos.y - 0.5f, (float)m_ref.pos.z);
            var rot = new Vector3((float)m_ref.rot.x, (float)m_ref.rot.y, (float)m_ref.rot.z);

            obj.transform.position = pos;
            obj.transform.rotation = Quaternion.Euler(rot);

            yield return new WaitForEndOfFrame();

            StartCoroutine(UpdateData(obj, data));
        }
    }
}
