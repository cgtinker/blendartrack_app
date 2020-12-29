using UnityEngine;
using System.Collections;

public static class ScreenSizeFactor
{

    public static (float, float, bool) GetFactor()
    {
        float factor = 1;
        float m_width = 1;
        bool portrait_mode = false;

        int height = Screen.height;
        int width = Screen.width;
        float dpi = Screen.dpi;

#if UNITY_ANDROID
        float x_dpi = DisplayMetricsAndroid.XDPI;
        float y_dpi = DisplayMetricsAndroid.YDPI;
        Debug.Log($"x_dpi: {x_dpi}, y_dpi: {y_dpi} :: dpi: {dpi} :: height: {height}, width: {width}");
        if (height > width)
        {
            if ((int)y_dpi != 0)
            {
                factor = (float)height / y_dpi;
                m_width = (float)width / y_dpi;
                portrait_mode = true;
            }

            else
            {
                if (dpi != 0)
                {
                    factor = (float)height / dpi;
                    m_width = (float)width / dpi;
                    portrait_mode = true;
                }

                else
                {
                    factor = (float)height / 72f;
                    m_width = (float)width / 72f;
                    portrait_mode = true;
                }
            }
        }

        else
        {
            if (x_dpi != 0)
            {
                factor = (float)width / x_dpi;
                m_width = (float)height / x_dpi;
                portrait_mode = false;
            }

            else
            {
                if (dpi != 0)
                {
                    factor = (float)width / dpi;
                    factor = (float)width / dpi;
                    portrait_mode = false;
                }

                else
                {
                    factor = (float)width / 72f;
                    factor = (float)width / 72f;
                    portrait_mode = false;
                }
            }
        }
#endif

#if UNITY_IOS
        if (height > width)
            {
                if (dpi != 0)
                {
                    factor = (float)height / 100f / dpi / 72;
                    m_width = (float)width / 100f / dpi / 72;
                    portrait_mode = true;

                }

                else
                {
                    factor = (float)height / 100f;
                    m_width = (float)width / 100f;
                    portrait_mode = true;
                }

            }

            else
            {
                if (dpi != 0)
                {
                    factor = (float)width / 100f / dpi / 72;
                    m_width = (float)height / 100f / dpi / 72;
                    portrait_mode = true;
                }

                else
                {
                    factor = (float)width / 100f;
                    m_width = (float)height / 100f;
                    portrait_mode = false;
                }
            }
#endif

#if UNITY_EDITOR
        if (height > width)
        {
            factor = (float)height / 100f;
            m_width = (float)width / 100f;
            portrait_mode = true;
        }

        else
        {
            factor = (float)width / 100f;
            m_width = (float)height / 100f;
            portrait_mode = false;
        }
#endif
        Debug.Log($"factor: {factor}, mW {m_width}, portait {portrait_mode}");
        return (factor, m_width, portrait_mode);
    }
}
