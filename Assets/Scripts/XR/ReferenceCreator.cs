using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using System;

namespace ArRetarget
{
    public class ReferenceCreator : MonoBehaviour
    {
        public ARRaycastManager arRaycastManager;

        [Header("Placed Prefab")]
        public GameObject MarkerPrefab;
        public Vector3 MarkerScale = new Vector3(0.1f, 0.1f, 0.1f);

        [Header("Double Tapping")]
        int TapCount;
        public float MaxDubbleTapTime = 0.3f;
        public float LongTouchTime = 0.8f;
        float NewTime;

        [HideInInspector]
        public List<GameObject> Markers = new List<GameObject>();
        private List<ARRaycastHit> arRaycastHits = new List<ARRaycastHit>();

        #region raycast methods
        private void DeleteDetectedMarker(Touch touch)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.tag == "anchor")
                {
                    DeleteMarker(hit.collider.gameObject);
                }
            }
        }

        private void PlaceMarkerOnPlane(Touch touch)
        {
            if (arRaycastManager.Raycast(touch.position, arRaycastHits))
            {
                var pose = arRaycastHits[0].pose;
                CreateMarker(pose.position);
            }
        }
        #endregion

        #region detect input
        public void Update()
        {
            if (Input.touchCount == 1)
            {
                DetectLongTab();
                DetectDoubleTab();
            }
        }


        private float timer = 0.0f;
        //for touch timing
        bool newTouch = false;
        private void DetectLongTab()
        {
            if (newTouch)
            {
                timer += Time.deltaTime;

                //delete marker when long touch time has been reached
                if (timer > LongTouchTime)
                {
                    if (Input.touches[0].phase == TouchPhase.Stationary)
                    {
                        DeleteDetectedMarker(Input.GetTouch(0));
                    }

                    //reset when done
                    newTouch = false;
                    timer = 0.0f;
                    TapCount = 0;
                }

                else
                {
                    //reset if the touch ended
                    if (Input.touches[0].phase == TouchPhase.Ended)
                    {
                        newTouch = false;
                        timer = 0.0f;
                    }
                }

            }

            else
            {
                timer = 0.0f;
                newTouch = true;
            }
        }

        private void DetectDoubleTab()
        {
            Touch touch = Input.GetTouch(0);

            //resetting the tab count
            if (Time.time > NewTime)
            {
                TapCount = 0;
            }

            //add first touch to tabcount
            if (touch.phase == TouchPhase.Began)
            {
                TapCount += 1;
            }

            //set new time in which the tab event has to take place
            if (TapCount == 1)
            {
                NewTime = Time.time + MaxDubbleTapTime;
            }

            //double tap to place anchor
            else if (TapCount == 2 && Time.time <= NewTime)
            {
                PlaceMarkerOnPlane(touch);
                TapCount = 0;
            }
        }
        #endregion

        #region create & delete
        public event Action CreatedMarker;

        private void CreateMarker(Vector3 position)
        {
            var marker = Instantiate(MarkerPrefab, position, Quaternion.identity);
            marker.transform.localScale = MarkerScale;
            Markers.Add(marker);
            CreatedMarker();
        }

        private void DeleteMarker(GameObject marker)
        {
            Markers.Remove(marker);
            Destroy(marker);
        }
        #endregion
    }
}

