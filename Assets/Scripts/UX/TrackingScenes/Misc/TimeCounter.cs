using UnityEngine;
using TMPro;

//simple timer as user feedback on press recording
public class TimeCounter : MonoBehaviour
{
	private float time;
	public TextMeshProUGUI text;
	private bool recording = false;

	private void OnEnable()
	{
		StartTimer();
	}

	private void OnDisable()
	{
		StopTimer();
	}

	private void StartTimer()
	{
		time = 0.0f;
		recording = true;
	}

	private void StopTimer()
	{
		time = 0.0f;
		recording = false;
	}

	// Update is called once per frame
	private void Update()
	{
		if (recording)
		{
			time += Time.deltaTime;

			string minutes = Mathf.Floor(time / 60).ToString("00");
			string seconds = Mathf.Floor(time % 60).ToString("00");

			string tmp = $"{minutes}:{seconds}";

			text.text = tmp;
		}
	}
}
