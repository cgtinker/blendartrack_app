using UnityEngine;
using NatSuite.Recorders;
using NatSuite.Recorders.Clocks;
using NatSuite.Recorders.Inputs;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(IntrinsicsCaptureManager))]
public class CameraRecorder : MonoBehaviour
{
    private CameraInput cameraInput;
    private MP4Recorder recorder;

    private IntrinsicsCaptureManager intrinsicsCaptureManager;
    private PoseDataTracker poseDataTracker;

    private FixedIntervalClock clock;
    private ARCameraManager arCameraManager;

    private void Start()
    {
        intrinsicsCaptureManager = this.gameObject.GetComponent<IntrinsicsCaptureManager>();
        poseDataTracker = this.gameObject.GetComponent<PoseDataTracker>();
        //sessionOrigin = this.gameObject.GetComponent<SessionOriginPosition>();
    }

    public void RecordVideo()
    {
        arCameraManager = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponentInChildren<ARCameraManager>();

        // init the recorder and start capturing intrinsics data
        InitRecorder(480, 640, 30, 8, 3);
        intrinsicsCaptureManager.OnStartRecording();
        poseDataTracker.Init();
        //sessionOrigin.Init();
    }

    private void InitRecorder(int width, int height, int fps, int bitrate, int keyframeInterval)
    {
        var br = bitrate * 1000000;
        //recorder settings
        recorder = new MP4Recorder(width: width, height: height, frameRate: fps, sampleRate: 0, channelCount: 0, bitrate: br, keyframeInterval: keyframeInterval);

        //var clock = new RaltimeClock();
        clock = new FixedIntervalClock(fps, false);
        OnReceivedFrameUpdate();
        // And use a `CameraInput` to record the main game camera
        cameraInput = new CameraInput(recorder, clock, Camera.main);
    }

    private void OnReceivedFrameUpdate()
    {
        arCameraManager.frameReceived += CommitVideoFrame;
    }

    private void OnDisponseFrameUpdate()
    {
        arCameraManager.frameReceived -= CommitVideoFrame;
    }

    public void CommitVideoFrame(ARCameraFrameEventArgs frame)
    {
        //interval += (double)frame.timestampNs;
        //clock.interval = interval;
        clock.Tick();
    }

    public async void StopRecording()
    {
        OnDisponseFrameUpdate();
        // stop caputing intrinsic data
        intrinsicsCaptureManager.OnStopRecording();
        poseDataTracker.Stop();
        //sessionOrigin.Stop();
        // Stop commiting frames to the recorder
        cameraInput.Dispose();
        // Finish writing
        var path = await recorder.FinishWriting();

        NativeGallery.SaveVideoToGallery(existingMediaPath: path, album: "Retargeter", filename: "NewVideo");
    }
}
