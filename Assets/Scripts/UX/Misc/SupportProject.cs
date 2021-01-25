using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArRetarget
{
    public class SupportProject : MonoBehaviour
    {
        const string m_url = "https://www.buymeacoffee.com/cgtinker";

        public void OnSupportProject()
        {
            Application.OpenURL(m_url);
        }
    }
}
