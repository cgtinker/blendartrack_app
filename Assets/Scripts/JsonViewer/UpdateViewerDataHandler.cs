using UnityEngine;
using System.Collections;

namespace ArRetarget
{
    public class UpdateViewerDataHandler : MonoBehaviour
    {
        public int frame = 0;

        public bool pause = false;
        private int frameEnd = 125;

        private void Start()
        {
            Application.targetFrameRate = 30;
            StartCoroutine(UpdateData());
        }

        public IEnumerator UpdateData()
        {
            if (frame < frameEnd && !pause)
            {
                frame++;
            }

            else if (frame == frameEnd && !pause)
            {
                frame = 0;
            }

            yield return new WaitForEndOfFrame();
            StartCoroutine(UpdateData());
        }

        public void Reset()
        {
            frame = frameEnd;
        }

        public void Pause()
        {
            pause = !pause;
        }

        public void SetFrameEnd(int frames)
        {
            if (frames > 0)
            {
                frameEnd = frames;
            }

            else
            {
                Debug.LogError("Tried to set a negative frame end value: " + frames);
            }
        }
    }
}
