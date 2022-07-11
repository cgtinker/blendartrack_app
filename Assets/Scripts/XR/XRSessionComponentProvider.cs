using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


namespace ArRetarget
{
	[RequireComponent(typeof(ARSessionOrigin))]
	public class XRSessionComponentProvider : Singleton<XRSessionComponentProvider>
	{
		[Header("Plane Tracking")]
		[SerializeField]
		private GameObject arPlanePrefab = null;
		[SerializeField]
		private GameObject arPointCloudPrefab = null;

		[Space]
		[SerializeField]
		private GameObject arReferencePrefab = null;
		[SerializeField]
		private Vector2 arReferenceCreatingTabTimings = new Vector2(0.3f, 0.8f);

		[Header("Face Tracking")]
		[SerializeField]
		private GameObject arFaceMesh = null;

		[Header("ArSession Components")]
		[SerializeField]
		private ARSession arSession;
		[SerializeField]
		private GameObject arDisplayCameraObject;
		[SerializeField]
		private ARCameraManager arCameraManager;
		[SerializeField]
		private ARCameraBackground arCameraBackground;

		public IEnumerator OnEnableFaceDetection()
		{
			Debug.Log("Enabling Face Detection");
			SetCameraFacingDirection(CameraFacingDirection.User);

			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			arSession.Reset();

			yield return new WaitForEndOfFrame();
			EnableARDisplayCamera(true);

			// Adding components to ar session
			ARFaceManager arFaceManager = this.gameObject.AddComponent<ARFaceManager>();
			ARInputManager arInputManager = this.gameObject.AddComponent<ARInputManager>();

			arFaceManager.facePrefab = arFaceMesh;
			arFaceManager.requestedMaximumFaceCount = 1;
		}

		public IEnumerator OnEnablePlaneDetection()
		{
			Debug.Log("Enabling Plane Detection");

			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			arSession.Reset();

			SetCameraFacingDirection(CameraFacingDirection.World);

			yield return new WaitForEndOfFrame();
			EnableARDisplayCamera(true);

			// set camera facing direction
			// adding components to ar session
			ARPlaneManager arPlaneManager = this.gameObject.AddComponent<ARPlaneManager>();
			ARPointCloudManager arPointCloudManager = this.gameObject.AddComponent<ARPointCloudManager>();
			ARInputManager arInputManager = this.gameObject.AddComponent<ARInputManager>();
			ARRaycastManager arRayCastManager = this.gameObject.AddComponent<ARRaycastManager>();
			ReferenceCreator arReferenceCreator = this.gameObject.AddComponent<ReferenceCreator>();

			// set references to components
			arPointCloudManager.pointCloudPrefab = arPointCloudPrefab;
			arPlaneManager.planePrefab = arPlanePrefab;

			arReferenceCreator.arRaycastManager = arRayCastManager;
			arReferenceCreator.anchorPrefab = arReferencePrefab;
			arReferenceCreator.MaxDubbleTapTime = arReferenceCreatingTabTimings[0];
			arReferenceCreator.LongTouchTime = arReferenceCreatingTabTimings[1];
		}

		public IEnumerator OnDisableFaceDetection()
		{
			Debug.Log("Disabling Face Detection");
			// disable the display camera before changing camera facing direction
			EnableARDisplayCamera(false);
			yield return new WaitForEndOfFrame();

			// camera facing direction gets changed on loading camera tracking
			if (StateMachine.Instance.AppState != StateMachine.State.CameraTracking)
			{
				SetCameraFacingDirection(CameraFacingDirection.None);
			}

			// removing tracking related components
			GameObject[] faces = GameObject.FindGameObjectsWithTag(TagManager.ARFace);
			DestroyGameObjects(faces);
			Destroy(this.gameObject.GetComponent<ARFaceManager>());
			Destroy(this.gameObject.GetComponent<ARInputManager>());

			yield return new WaitForEndOfFrame();
			arSession.Reset();
		}

		public IEnumerator OnDisablePlaneDetection()
		{
			Debug.Log("Disabling Plane Detection");

			EnableARDisplayCamera(false);
			yield return new WaitForEndOfFrame();

			// Keep camera state as it gets switched on intializing camera tracker
			if (StateMachine.Instance.AppState != StateMachine.State.FaceTracking)
			{
				SetCameraFacingDirection(CameraFacingDirection.None);
			}

			// remove trackables
			GameObject[] planes = GameObject.FindGameObjectsWithTag(TagManager.ARPlane);
			DestroyGameObjects(planes);
			GameObject[] clouds = GameObject.FindGameObjectsWithTag(TagManager.ARPointCloud);
			DestroyGameObjects(clouds);

			// remove session related components
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

		private void EnableARDisplayCamera(bool enable)
		{
			if (arDisplayCameraObject.activeSelf != enable)
			{
				arDisplayCameraObject.SetActive(enable);
			}

			if (arCameraBackground.enabled != enable)
			{
				arCameraBackground.enabled = enable;
			}

			Debug.Log($"{arCameraBackground.isActiveAndEnabled} - camera background active and enabled");
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