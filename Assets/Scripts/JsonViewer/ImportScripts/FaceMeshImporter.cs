using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArRetarget
{
    public class FaceMeshImporter : MonoBehaviour, IInitViewer<MeshDataContainer>, IUpdate<List<GameObject>, MeshDataContainer>
    {
        public UpdateViewerDataHandler viewHandler;
        int meshDataListIndex = 0;

        public IEnumerator InitViewer(MeshDataContainer data)
        {
            //generating points if received the point amout
            if (GetPointAmount(data) > 0 && meshDataListIndex < data.meshDataList.Count)
            {
                //setting frame end
                viewHandler.SetFrameEnd(data.meshDataList.Count);
                //generating points
                List<GameObject> meshPoints = GeneratePoints(GetPointAmount(data));

                yield return new WaitForEndOfFrame();
                //start updating
                StartCoroutine(UpdateData(meshPoints, data));
            }

            //restarting if there arent points at a certain frame
            else if (GetPointAmount(data) == 0 && meshDataListIndex > data.meshDataList.Count)
            {
                StartCoroutine(InitViewer(data));
            }

            else
            {
                Debug.LogError("Mesh Data Container doesnt have stored data");
            }

            yield return new WaitForEndOfFrame();
        }

        public IEnumerator UpdateData(List<GameObject> obj, MeshDataContainer data)
        {
            for (int i = 0; i < data.meshDataList[viewHandler.frame].pos.Count; i++)
            //for (int i = 0; i < data.meshDataList[viewHandler.frame].pos.Length; i++)

            {
                var pos = data.meshDataList[viewHandler.frame].pos[i];
                obj[i].transform.position = new Vector3((float)pos.x * 10, (float)pos.y * 10, (float)pos.z * 10);
            }

            yield return new WaitForEndOfFrame();

            StartCoroutine(UpdateData(obj, data));
        }

        #region point generation
        //amount of points to generate
        private int GetPointAmount(MeshDataContainer data)
        {
            if (data.meshDataList[meshDataListIndex].pos.Count > 0)
            //if (data.meshDataList[meshDataListIndex].pos.Length > 0)
            {
                //return data.meshDataList[meshDataListIndex].pos.Length;
                return data.meshDataList[meshDataListIndex].pos.Count;
            }

            else
            {
                meshDataListIndex++;
                return 0;
            }
        }

        //point styleing
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
        #endregion
    }
}
