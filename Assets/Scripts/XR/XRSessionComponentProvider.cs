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

		private ARSession arSession;
		private Camera arDisplayCamera;
		private ARCameraManager arCameraManager;

		private void Start()
		{
			arSession = GameObject.FindGameObjectWithTag("arSession").GetComponent<ARSession>();
			arDisplayCamera = GameObject.FindGameObjectWithTag("ARDisplayCamera").GetComponent<Camera>();
			arCameraManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ARCameraManager>();
		}


		public IEnumerator OnEnableFaceDetection()
		{
			RequestCameraFacingDirection(CameraFacingDirection.User);
			yield return new WaitForEndOfFrame();
			arSession.Reset();
			yield return new WaitForEndOfFrame();

			// Adding components to ar session
			ARFaceManager arFaceManager = this.gameObject.AddComponent<ARFaceManager>();
			ARInputManager arInputManager = this.gameObject.AddComponent<ARInputManager>();

			arFaceManager.facePrefab = arFaceMesh;
			arFaceManager.requestedMaximumFaceCount = 1;
		}

		public IEnumerator OnDisableFaceDetection()
		{
			RequestCameraFacingDirection(CameraFacingDirection.None);
			Debug.Log("Dsiabled Face Detection");
			GameObject[] faces = GameObject.FindGameObjectsWithTag("face");
			DestroyGameObjects(faces);
			Destroy(this.gameObject.GetComponent<ARFaceManager>());
			Destroy(this.gameObject.GetComponent<ARInputManager>());

			yield return new WaitForEndOfFrame();
			arSession.Reset();
		}

		public IEnumerator OnEnablePlaneDetection()
		{
			RequestCameraFacingDirection(CameraFacingDirection.World);
			Debug.Log("Enabled Plane Detection");
			yield return new WaitForEndOfFrame();
			arSession.Reset();
			yield return new WaitForEndOfFrame();
			// set camera facing direction
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

		public IEnumerator OnDisablePlaneDetection()
		{
			RequestCameraFacingDirection(CameraFacingDirection.None);
			print("Try Destory plane componenrs");
			GameObject[] planes = GameObject.FindGameObjectsWithTag("retarget");
			DestroyGameObjects(planes);
			GameObject[] clouds = GameObject.FindGameObjectsWithTag("pointCloud");
			DestroyGameObjects(clouds);

			Destroy(this.gameObject.GetComponent<ARPlaneManager>());
			Destroy(this.gameObject.GetComponent<ARPointCloudManager>());
			Destroy(this.gameObject.GetComponent<ARInputManager>());
			Destroy(this.gameObject.GetComponent<ARRaycastManager>());
			Destroy(this.gameObject.GetComponent<ReferenceCreator>());

			yield return new WaitForEndOfFrame();
			arSession.Reset();
		}

		private void DestroyGameObjects(GameObject[] objs)
		{
			foreach (GameObject obj in objs)
			{
				Destroy(obj);
			}
		}

		private void RequestCameraFacingDirection(CameraFacingDirection direction)
		{
			switch (direction)
			{
				case CameraFacingDirection.None:
				arDisplayCamera.enabled = false;
				break;
				case CameraFacingDirection.World:
				arDisplayCamera.enabled = true;
				break;
				case CameraFacingDirection.User:
				arDisplayCamera.enabled = true;
				break;
				default:
				arDisplayCamera.enabled = false;
				break;
			}

			SetCameraFacingDirection(direction);
		}

		private void SetCameraFacingDirection(CameraFacingDirection direction)
		{
			if (arCameraManager.currentFacingDirection != direction)
			{
				arCameraManager.requestedFacingDirection = direction;
			}
		}
	}
}