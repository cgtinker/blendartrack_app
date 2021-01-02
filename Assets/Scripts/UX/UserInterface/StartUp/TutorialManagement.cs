using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ArRetarget
{
    public class TutorialManagement : MonoBehaviour
    {
        enum SlidePosition
        {
            First,
            Default,
            Last
        }

        SlidePosition slidePosition;

        int currentSlide;
        public List<TutorialData> tutorialContents = new List<TutorialData>();

        [Header("Displayed")]
        public TextMeshProUGUI titel;
        public TextMeshProUGUI message;
        public TextMeshProUGUI slideCounter;

        public Image Icon;
        [Header("Button Text")]
        public TextMeshProUGUI customButtonText;
        public TextMeshProUGUI nextSlideText;
        public TextMeshProUGUI previousSlideText;

        [Header("Buttons")]
        public Button customButton;
        public Button nextButton;
        public Button previousButton;

        const string br = "<br>";

        private void Start()
        {
            var data = GetSlide(0, 0);
            SetTutorialContents(data);
        }

        public void SetTutorialContents(TutorialData data)
        {
            currentSlide = data.Index;
            SetSlidePosition(data.Index);

            switch (slidePosition)
            {
                case SlidePosition.First:
                    customButtonText.text = "get started";
                    SetCustomButtonVisibilty(true);
                    break;
                case SlidePosition.Default:
                    slideCounter.text = $"{data.Index} / {tutorialContents.Count - 2}";
                    SetCustomButtonVisibilty(false);
                    break;
                case SlidePosition.Last:
                    customButtonText.text = "start retargeting";
                    SetCustomButtonVisibilty(true);
                    break;
            }

            titel.text = data.Titel;
            message.text = br + data.Message;
            Icon.sprite = data.Icon;
        }

        public void SetCustomButtonVisibilty(bool active)
        {
            //custom button
            customButtonText.enabled = active;
            customButton.enabled = active;

            //next / previous button
            nextButton.enabled = !active;
            previousButton.enabled = !active;
            nextSlideText.enabled = !active;
            previousSlideText.enabled = !active;

            //slide counter
            slideCounter.enabled = !active;
        }

        TutorialData GetSlide(int curSlide, int offset)
        {
            for (int i = 0; i < tutorialContents.Count; i++)
            {
                if (tutorialContents[i].Index == curSlide + offset)
                {
                    return tutorialContents[i];
                }
            }

            Debug.LogError("No Tutorial-Data found");
            return new TutorialData();
        }

        void SetSlidePosition(int slideIndex)
        {
            if (slideIndex != tutorialContents.Count - 1 && slideIndex != 0)
            {
                slidePosition = SlidePosition.Default;
            }

            else if (slideIndex == 0)
            {
                slidePosition = SlidePosition.First;
            }

            else if (slideIndex == tutorialContents.Count - 1)
            {
                slidePosition = SlidePosition.Last;
            }
        }

        #region button events
        public void OnNextSlide()
        {
            var data = GetSlide(currentSlide, 1);
            SetTutorialContents(data);
        }

        public void OnPreviousSlide()
        {
            var data = GetSlide(currentSlide, -1);
            SetTutorialContents(data);
        }

        public void OnCustomButton()
        {
            switch (slidePosition)
            {
                case SlidePosition.First:
                    OnNextSlide();
                    break;
                case SlidePosition.Last:
                    OnCloseTutorial();
                    break;
            }
        }

        public void OnCloseTutorial()
        {
            PlayerPrefs.SetInt("tutorial", -1);

            //enable auto rotation
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.orientation = ScreenOrientation.AutoRotation;

            //loading scene
            int scene = PlayerPrefs.GetInt("scene", 1);
            GameObject.FindGameObjectWithTag("manager").GetComponent<AdditiveSceneManager>().SwitchScene(scene);

            this.gameObject.SetActive(false);
        }
        #endregion
    }

    [System.Serializable]
    public class TutorialData
    {
        public string Titel;
        public string Message;
        public Sprite Icon;
        public int Index;
    }
}