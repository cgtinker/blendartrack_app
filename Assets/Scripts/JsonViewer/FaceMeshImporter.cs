using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArRetarget
{
    public class FaceMeshImporter : MonoBehaviour
    {
        public UpdateViewerDataHandler viewHandler;
        int meshDataListIndex = 0;

        public IEnumerator InitJsonViewer(MeshDataContainer data)
        {
            //generating points if received the point amout
            if (GetPointAmount(data) > 0 && meshDataListIndex < data.meshDataList.Count)
            {
                List<GameObject> meshPoints = GeneratePoints(GetPointAmount(data));
                StartCoroutine(UpdatePose(meshPoints, data));
            }

            //restarting if there arent points at a certain frame
            else if (GetPointAmount(data) == 0 && meshDataListIndex > data.meshDataList.Count)
            {
                StartCoroutine(InitJsonViewer(data));
            }

            else
            {
                Debug.LogError("Mesh Data Container doesnt have stored data");
            }

            yield return new WaitForEndOfFrame();
        }

        private IEnumerator UpdatePose(List<GameObject> meshPoints, MeshDataContainer data)
        {
            for (int i = 0; i < data.meshDataList[viewHandler.frame].pos.Count; i++)
            {
                var pos = data.meshDataList[viewHandler.frame].pos[i];
                meshPoints[i].transform.position = new Vector3(pos.x * 10, pos.y * 10, pos.z * 10);
            }

            yield return new WaitForEndOfFrame();

            StartCoroutine(UpdatePose(meshPoints, data));
        }

        private int GetPointAmount(MeshDataContainer data)
        {
            if (data.meshDataList[meshDataListIndex].pos.Count > 0)
            {
                return data.meshDataList[meshDataListIndex].pos.Count;
            }

            else
            {
                meshDataListIndex++;
                return 0;
            }
        }

        private List<GameObject> GeneratePoints(int meshPoints)
        {
            List<GameObject> meshPointList = new List<GameObject>();

            for (int i = 0; i < meshPoints; i++)
            {
                GameObject meshPoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
                meshPoint.transform.position = new Vector3(0, 0, 0);
                meshPoint.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

                var rend = meshPoint.GetComponent<MeshRenderer>();
                Material emission = Resources.Load("SimpleEmission", typeof(Material)) as Material;
                rend.material = new Material(emission);

                meshPoint.transform.SetParent(this.gameObject.transform);


                meshPointList.Add(meshPoint);
            }

            return meshPointList;
        }
    }
}
