using UnityEngine;

public class CaptureScreenshot : MonoBehaviour
{
	private static int imgCount = 0;
	private string path;

	public int sizeFactor = 1;
	public string imgTitel = "BlendArTrack";
	public KeyCode keyCode;

	private void Start()
	{
		imgCount = 0;
		path = Application.persistentDataPath;
	}

	private void Update()
	{
		if (Input.GetKeyDown(keyCode))
		{
			OnCaptureScreenshot();
		}
	}

	public void OnCaptureScreenshot()
	{
		imgCount++;
		string filename = $"{path}/{imgTitel}_{imgCount}.png";

		ScreenCapture.CaptureScreenshot(filename, sizeFactor);
		Debug.Log($"saved {imgTitel}_{imgCount} at persistent path");
	}
}
