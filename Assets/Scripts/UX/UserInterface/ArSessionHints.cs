using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace ArRetarget
{
    public class ArSessionHints : MonoBehaviour
    {
        #region enums
        public enum TrackingType
        {
            PlaneTracking,
            FaceTracking,
            none
        }

        public TrackingType type;

        private enum PlaneTrackingState
        {
            NewSession,
            SearchingFeatures,
            ToDark,
            DetectedPlane,
            PlacedObject,
            Recording,
        };

        private PlaneTrackingState planeState;
        private PlaneTrackingState PlaneStateCallback
        {
            get { return planeState; }
            set
            {
                planeState = value;
                DisplaySessionHints();
            }
        }

        private enum FaceTrackingState
        {
            Features,
            DetectedFace,
            Placement,
            Recording,
        }

        private FaceTrackingState faceState;
        private FaceTrackingState FaceStateCallback
        {
            get { return faceState; }
            set
            {
                faceState = value;
                DisplaySessionHints();
            }
        }
        #endregion

        public event Action<string> TrackingStateChanged;

        ARCameraManager m_CameraManager;
        ARPlaneManager m_PlaneManager;
        ReferenceCreator m_ReferenceCreator;

        public Dictionary<string, string> NotificationMessages = new Dictionary<string, string>()
        {
            { "plane", "Unable to find a surface. Try moving to the side or repositioning your phone." },
            { "placement", "Double Tap a location to place a Reference Object."},
            { "remove", "Tap long on a Reference Object to remove it." },
            { "features", "Try turning on more lights and start moving around."},
            { "motion", "Try moving your phone more slowly."},
            { "light", "Try turning on more lights."},
            { "toLong", " Try moving around, turning on more lights, and making sure your phone is pointed at a sufficiently textured surface." }
        };

        float minimumBrightness = 0.38f;

        private void Start()
        {
            switch (type)
            {
                case TrackingType.PlaneTracking:
                    InitPlaneTrackingReferences();
                    break;
                case TrackingType.FaceTracking:
                    InitFaceTrackingReferences();
                    break;
                case TrackingType.none:
                    DisableSessionHints();
                    break;
            }
        }

        public void DisplaySessionHints()
        {
            switch (type)
            {
                case TrackingType.PlaneTracking:
                    switch (planeState)
                    {
                        case PlaneTrackingState.SearchingFeatures:
                            Debug.Log("Searching Features");
                            //TrackingStateChanged("Searching for Features");
                            break;
                        case PlaneTrackingState.DetectedPlane:
                            Debug.Log("Detected a plane!");
                            break;
                        case PlaneTrackingState.PlacedObject:
                            Debug.Log("Placed an Object!");
                            break;
                        case PlaneTrackingState.Recording:
                            break;
                    }
                    break;

                case TrackingType.FaceTracking:
                    switch (faceState)
                    {
                        case FaceTrackingState.Features:
                            break;
                        case FaceTrackingState.DetectedFace:
                            break;
                        case FaceTrackingState.Placement:
                            break;
                        case FaceTrackingState.Recording:
                            break;
                    }
                    break;

                case TrackingType.none:

                    break;
            }
        }

        private void InitPlaneTrackingReferences()
        {
            planeState = PlaneTrackingState.NewSession;

            m_CameraManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ARCameraManager>();
            m_PlaneManager = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<ARPlaneManager>();
            m_ReferenceCreator = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<ReferenceCreator>();

            if (m_CameraManager != null & enabled)
            {
                m_CameraManager.frameReceived += FrameChanged;
                m_CameraManager.lightEstimationMode = UnityEngine.XR.ARSubsystems.LightEstimationMode.AmbientIntensity;
            }
            if (m_ReferenceCreator != null & enabled)
                m_ReferenceCreator.CreatedMarker += PlacedObject;
        }

        private void InitFaceTrackingReferences()
        {
            m_CameraManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ARCameraManager>();
            if (m_CameraManager != null & enabled)
                m_CameraManager.frameReceived += FrameChanged;
        }

        private void DisableSessionHints()
        {
            m_CameraManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ARCameraManager>();
            if (m_CameraManager != null & enabled)
            {
                m_CameraManager.frameReceived += FrameChanged;
                m_CameraManager.lightEstimationMode = UnityEngine.XR.ARSubsystems.LightEstimationMode.Disabled;
            }
        }

        //unsubscribe from events
        void OnDisable()
        {
            if (m_CameraManager != null)
                m_CameraManager.frameReceived -= FrameChanged;
            if (m_ReferenceCreator != null)
                m_ReferenceCreator.CreatedMarker -= PlacedObject;
        }

        //on received frame from ar camera
        int ticker = 0;
        private void FrameChanged(ARCameraFrameEventArgs args)
        {
            //check lightning conditions every half second
            ticker++;
            {
                if (ticker == 15)
                {
                    if (args.lightEstimation.averageBrightness.HasValue)
                    {
                        if (args.lightEstimation.averageBrightness.Value < minimumBrightness)
                        {
                            if (planeState != PlaneTrackingState.ToDark)
                                PlaneStateCallback = PlaneTrackingState.ToDark;
                        }
                    }
                    ticker = 0;
                }
            }


            switch (type)
            {
                case TrackingType.PlaneTracking:

                    switch (planeState)
                    {
                        case PlaneTrackingState.NewSession:
                            //message to search for features
                            if (PlanesFound())
                            {
                                //message to place object?
                                if (planeState != PlaneTrackingState.DetectedPlane)
                                    PlaneStateCallback = PlaneTrackingState.DetectedPlane;
                            }
                            else
                            {
                                if (planeState != PlaneTrackingState.SearchingFeatures)
                                    PlaneStateCallback = PlaneTrackingState.SearchingFeatures;
                            }
                            break;
                        case PlaneTrackingState.SearchingFeatures:
                            //searching for features
                            if (PlanesFound())
                            {
                                //message to place object?
                                if (planeState != PlaneTrackingState.DetectedPlane)
                                    PlaneStateCallback = PlaneTrackingState.DetectedPlane;
                            }
                            break;
                    }
                    break;
            }
        }

        bool PlanesFound()
        {
            if (m_PlaneManager == null)
                return false;

            return m_PlaneManager.trackables.count > 0;
        }

        void PlacedObject()
        {
            //message to remove // recording
            if (planeState != PlaneTrackingState.PlacedObject)
                PlaneStateCallback = PlaneTrackingState.PlacedObject;
        }
    }
}