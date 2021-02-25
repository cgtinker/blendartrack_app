using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ArRetarget
{
    public class CameraLookAtData : MonoBehaviour
    {
        public Transform viewerCamera;
        Transform target;
        JsonDataImporter jsonDataImporter;

        IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            jsonDataImporter = this.gameObject.GetComponent<JsonDataImporter>();

            if (jsonDataImporter.poseData)
                StartCoroutine(CameraLookAt());
        }

        //camera looks at imported data on import, to avoid the impression there is possibly no data
        IEnumerator CameraLookAt()
        {
            yield return new WaitForEndOfFrame();
            try
            {
                target = jsonDataImporter.importParent.transform.GetChild(0).transform;
                viewerCamera.LookAt(new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z));
                Vector3 cameraRot = new Vector3(0, viewerCamera.rotation.eulerAngles.y, viewerCamera.rotation.eulerAngles.z);
                viewerCamera.eulerAngles = cameraRot;
            }
            catch
            {
                Debug.LogWarning("Viewer couldn't be opend, probaly contains corrupted data");
            }

        }
    }
}
