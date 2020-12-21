using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ArRetarget
{
    public class CameraPoseImporter : MonoBehaviour, IInitViewer<CameraPoseContainer>, IUpdate<GameObject, CameraPoseContainer>
    {
        public UpdateViewerDataHandler viewHandler;

        //generating the necessary obj for viewing
        public IEnumerator InitViewer(CameraPoseContainer data)
        {
            //generating the obj
            GameObject obj = GenerateGismos();
            obj.transform.parent = this.gameObject.transform;

            //setting the endframe of the animation
            viewHandler.SetFrameEnd(data.cameraPoseList.Count);

            yield return new WaitForEndOfFrame();
            StartCoroutine(UpdateData(obj, data));
            obj.SetActive(true);
        }

        private GameObject GenerateGismos()
        {
            var obj = Instantiate(Resources.Load("TrailGizmos", typeof(GameObject)) as GameObject);
            obj.SetActive(false);
            /*
            //runtime generation doesnt work on ios
            //holds the axis
            GameObject obj = new GameObject("parent");
            obj.transform.position = Vector3.zero;
            obj.transform.rotation = Quaternion.Euler(Vector3.zero);

            GameObject axisX = GameObject.CreatePrimitive(PrimitiveType.Cube);
            axisX.name = "axisX";
            axisX.transform.position = new Vector3(0.5f, 0f, 0f);
            axisX.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -1.0f));

            GameObject axisY = GameObject.CreatePrimitive(PrimitiveType.Cube);
            axisY.name = "axisY";
            axisY.transform.position = new Vector3(0.5f, 0.5f, 0.5f);
            axisY.transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));

            GameObject axisZ = GameObject.CreatePrimitive(PrimitiveType.Cube);
            axisZ.name = "axisZ";
            axisZ.transform.position = new Vector3(0.0f, 0.0f, 0.5f);
            axisZ.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));

            List<GameObject> axisList = new List<GameObject>()
            {
                axisX,
                axisY,
                axisZ
            };

            //adding material and parent
            foreach (GameObject axis in axisList)
            {
                axis.GetComponent<MeshRenderer>().material = Resources.Load("SimpleEmission", typeof(Material)) as Material;
                axis.transform.localScale = new Vector3(0.025f, 0.025f, 1);
                axis.transform.parent = obj.transform;
            }
            */
            return obj;
        }

        //updating the pose data based on the jsonViewerHandler
        public IEnumerator UpdateData(GameObject obj, CameraPoseContainer data)
        {
            //if last frame restart particle emission
            if (data.cameraPoseList.Count - 1 == viewHandler.frame)
            {
                var emitter = obj.GetComponent<ParticleSystem>();
                emitter.Clear();
            }

            //data at current frame
            var m_ref = data.cameraPoseList[viewHandler.frame];
            var pos = new Vector3((float)m_ref.pos.x, (float)m_ref.pos.y - 0.5f, (float)m_ref.pos.z);
            var rot = new Vector3((float)m_ref.rot.x, (float)m_ref.rot.y, (float)m_ref.rot.z);

            obj.transform.position = pos;
            obj.transform.rotation = Quaternion.Euler(rot);

            yield return new WaitForEndOfFrame();

            StartCoroutine(UpdateData(obj, data));
        }
    }
}
