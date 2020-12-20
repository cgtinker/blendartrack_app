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
        public GameObject Timer;
        TimeCounter counter;

        private bool recording;

        // Start is called before the first frame update
        void Start()
        {
            counter = Timer.GetComponent<TimeCounter>();
            recording = false;
        }

        //play micro interaction and handel tracking
        public void OnToggleRecording()
        {
            recording = !recording;

            if (recording)
            {
                //rec button pressed
                animator.Play("rec");
                m_image.sprite = breakImg;

                Timer.SetActive(true);
                counter.StartTimer();

                inputHandler.StartTracking();
            }

            else
            {
                //break button pressed
                animator.Play("stop");
                m_image.sprite = recordImg;

                Timer.SetActive(false);
                counter.StopTimer();

                inputHandler.StopTrackingAndSerializeData();
            }
        }
    }
}