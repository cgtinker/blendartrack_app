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
		Uri datasetUri = new System.Uri($"{path}{title}.mp4");
		// explicit conversion
		Uri absoluteUri = new System.Uri(datasetUri.AbsoluteUri);
		recordingConfig.Mp4DatasetUri = absoluteUri;

		recordingManager.StartRecording(recordingConfig);
	}

	public void StopTracking()
	{
		recordingManager.StopRecording();
	}
}
