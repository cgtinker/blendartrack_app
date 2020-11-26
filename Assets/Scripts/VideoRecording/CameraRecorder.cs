using UnityEngine;
using NatSuite.Recorders;
using NatSuite.Recorders.Clocks;
using NatSuite.Recorders.Inputs;

[RequireComponent(typeof(IntrinsicsCaptureManager))]
public class CameraRecorder : MonoBehaviour
{
    private CameraInput cameraInput;
    private MP4Recorder recorder;
    private IntrinsicsCaptureManager intrinsicsCaptureManager;

    private void Start()
    {
        intrinsicsCaptureManager = this.gameObject.GetComponent<IntrinsicsCaptureManager>();
    }

    public void RecordVideo()
    {
        // init the recorder and start capturing intrinsics data
        InitRecorder(480, 640, 30, 8, 3);
        intrinsicsCaptureManager.OnStartRecording();
    }

    private void InitRecorder(int width, int height, int fps, int bitrate, int keyframeInterval)
    {
        var br = bitrate * 1000000;
        //recorder settings
        recorder = new MP4Recorder(width: width, height: height, frameRate: fps, sampleRate: 0, channelCount: 0, bitrate: br, keyframeInterval: keyframeInterval);

        var clock = new RealtimeClock();
        // And use a `CameraInput` to record the main game camera
        cameraInput = new CameraInput(recorder, clock, Camera.main);
    }

    public async void StopRecording()
    {
        // stop caputing intrinsic data
        intrinsicsCaptureManager.OnStopRecording();
        // Stop commiting frames to the recorder
        cameraInput.Dispose();
        // Finish writing
        var path = await recorder.FinishWriting();

        NativeGallery.SaveVideoToGallery(existingMediaPath: path, album: "Retargeter", filename: "NewVideo");
    }
}
