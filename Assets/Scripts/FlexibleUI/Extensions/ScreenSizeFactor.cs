using UnityEngine;
using System.Collections;

public static class ScreenSizeFactor
{
	public static (float, float, bool) GetFactor()
	{
		bool portrait_mode;

		int height = Screen.height;
		int width = Screen.width;

		float factor = (float)1920 / 100f;
		float m_width = (float)1080 / 100f;

		if (height > width)
		{
			portrait_mode = true;
		}

		else
		{
			portrait_mode = false;
		}

		return (factor, m_width, portrait_mode);
	}
}
