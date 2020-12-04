using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArRetarget
{
    public class RecordButton : MonoBehaviour
    {
        [Header("Sprites")]
        public Sprite recordImg;
        public Sprite breakImg;

        [Header("References")]
        public Image m_image;
        public Animator animator;
        public InputHandler inputHandler;

        private bool recording;

        // Start is called before the first frame update
        void Start()
        {
            recording = false;
        }

        //play micro interaction and handel tracking
        public void OnToggleRecording()
        {
            recording = !recording;

            if (!recording)
            {
                //break button pressed
                animator.Play("stop");
                m_image.sprite = recordImg;

                //inputHandler.StartTracking();
            }

            else
            {
                //rec button pressed
                animator.Play("rec");
                m_image.sprite = breakImg;

                //inputHandler.StopTrackingAndSerializeData();
            }
        }
    }
}