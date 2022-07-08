using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


namespace ArRetarget
{
	[RequireComponent(typeof(ARSessionOrigin))]
	public class XRSessionComponentProvider : MonoBehaviour
	{
		[SerializeField]
		private GameObject arPlanePrefab = null;
		[SerializeField]
		private GameObject arPointCloudPrefab = null;
		[Space]
		[SerializeField]
		private GameObject arReferencePrefab = null;
		[SerializeField]
		private Vector2 arReferenceCreatingTabTimings = new Vector2(0.3f, 0.8f);
		[Space]
		[SerializeField]
		private GameObject arFaceMesh = null;

		public IEnumerator OnEnableFaceDetection()
		{
			Debug.Log("Enabled Face Detection");
			// Adding components to ar session
			yield return new WaitForEndOfFrame();
			// set camera facing direction
			ARCameraManager arCameraManager = this.transform.GetChild(0).gameObject.GetComponent<ARCameraManager>();
			if (arCameraManager.currentFacingDirection != CameraFacingDirection.User)
			{
				arCameraManager.requestedFacingDirection = CameraFacingDirection.User;
			}

			ARFaceManager arFaceManager = this.gameObject.AddComponent<ARFaceManager>();
			ARInputManager arInputManager = this.gameObject.AddComponent<ARInputManager>();

			arFaceManager.facePrefab = arFaceMesh;
			arFaceManager.requestedMaximumFaceCount = 1;
		}

		public void OnDisableFaceDetection()
		{
			Debug.Log("Dsiabled Face Detection");

			Destroy(this.gameObject.GetComponent<ARFaceManager>());
			Destroy(this.gameObject.GetComponent<ARInputManager>());

		}

		public IEnumerator OnEnablePlaneDetection()
		{
			Debug.Log("Enabled Plane Detection");
			yield return new WaitForEndOfFrame();
			// set camera facing direction
			ARCameraManager arCameraManager = this.transform.GetChild(0).gameObject.GetComponent<ARCameraManager>();
			if (arCameraManager.currentFacingDirection != CameraFacingDirection.World)
			{
				arCameraManager.requestedFacingDirection = CameraFacingDirection.World;
			}

			// Adding components to ar session
			ARPlaneManager arPlaneManager = this.gameObject.AddComponent<ARPlaneManager>();
			ARPointCloudManager arPointCloudManager = this.gameObject.AddComponent<ARPointCloudManager>();
			ARInputManager arInputManager = this.gameObject.AddComponent<ARInputManager>();
			ARRaycastManager arRayCastManager = this.gameObject.AddComponent<ARRaycastManager>();
			ReferenceCreator arReferenceCreator = this.gameObject.AddComponent<ReferenceCreator>();

			arPointCloudManager.pointCloudPrefab = arPointCloudPrefab;
			arPlaneManager.planePrefab = arPointCloudPrefab;

			arReferenceCreator.arRaycastManager = arRayCastManager;
			arReferenceCreator.anchorPrefab = arReferencePrefab;
			arReferenceCreator.MaxDubbleTapTime = arReferenceCreatingTabTimings[0];
			arReferenceCreator.LongTouchTime = arReferenceCreatingTabTimings[1];
		}

		public void OnDisablePlaneDetection()
		{
			print("Try Destory plane componenrs");
			Destroy(this.gameObject.GetComponent<ARPlaneManager>());
			Destroy(this.gameObject.GetComponent<ARPointCloudManager>());
			Destroy(this.gameObject.GetComponent<ARInputManager>());
			Destroy(this.gameObject.GetComponent<ARRaycastManager>());
			Destroy(this.gameObject.GetComponent<ReferenceCreator>());
		}
	}
}