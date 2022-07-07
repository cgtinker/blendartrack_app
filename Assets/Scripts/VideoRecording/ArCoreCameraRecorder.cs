using UnityEngine;
using UnityEngine.XR.ARCore;
using Google.XR.ARCoreExtensions;
using System;
using ArRetarget;

public class ArCoreCameraRecorder : MonoBehaviour, IInit<string, string>, IStop
{
	private ARRecordingManager recordingManager;
	private ARCoreRecordingConfig recordingConfig;

	void Start()
	{
		recordingManager = this.gameObject.GetComponent<ARRecordingManager>();
	}

	public void Init(string path, string title)
	{
		Debug.Log("init video recorder");
		recordingConfig = ScriptableObject.CreateInstance<ARCoreRecordingConfig>();
		Debug.LogWarning("config tracks " + recordingConfig.Tracks);
		foreach (Track track in recordingConfig.Tracks)
		{
			Debug.Log(track);
		}
		Uri datasetUri = new System.Uri($"{path}{title}.mp4");
		// explicit conversion
		Uri absoluteUri = new System.Uri(datasetUri.AbsoluteUri);
		recordingConfig.Mp4DatasetUri = absoluteUri;

		recordingManager.StartRecording(recordingConfig);
	}

	void RecordingStatus()
	{
		Debug.Log("Current Recording Status: " + recordingManager.RecordingStatus);
	}

	public void StopTracking()
	{
		recordingManager.StopRecording();
	}
}
