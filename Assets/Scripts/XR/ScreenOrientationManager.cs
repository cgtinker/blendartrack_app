using UnityEngine;

namespace ArRetarget
{
	public class ScreenOrientationManager
	{
		public enum Orientation
		{
			Portrait,
			Auto
		}

		private static Orientation m_orientation;
		public static Orientation setOrientation
		{
			get
			{
				return m_orientation;
			}

			set
			{
				m_orientation = value;

				try
				{
					SetScreenOrientation();
				}
				catch
				{
					Debug.LogError($"Cannot set {m_orientation} screen orientation");
				}
			}
		}

		private static void SetScreenOrientation()
		{
			switch (m_orientation)
			{
				case Orientation.Portrait:
				SetAutoRotation(false);
				Screen.orientation = ScreenOrientation.Portrait;
				break;

				case Orientation.Auto:
				SetAutoRotation(true);
				Screen.orientation = ScreenOrientation.AutoRotation;
				break;
			}
		}

		private static void SetAutoRotation(bool cur)
		{
			Screen.autorotateToLandscapeLeft = cur;
			Screen.autorotateToLandscapeRight = cur;
		}
	}
}
