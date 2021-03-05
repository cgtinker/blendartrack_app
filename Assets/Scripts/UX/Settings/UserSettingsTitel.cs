using UnityEngine;
using TMPro;

namespace ArRetarget
{
    public class UserSettingsTitel : MonoBehaviour
    {
        public TextMeshProUGUI displayText;

        public void Init(string displayText)
        {
            this.displayText.text = displayText;
        }
    }
}
