using UnityEngine;
using ArRetarget;
using NatSuite.Recorders;
using NatSuite.Recorders.Clocks;
using NatSuite.Recorders.Inputs;
using UnityEngine.XR.ARFoundation;

public class CameraRecorder : MonoBehaviour
{
    private CameraInput cameraInput;
    private MP4Recorder recorder;

    private FixedIntervalClock clock;
    private ARCameraManager arCameraManager;

    public void RecordVideo()
    {
        arCameraManager = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponentInChildren<ARCameraManager>();

        // init the recorder and start capturing intrinsics data
        InitRecorder(480, 640, 30, 8, 3);
    }

    private void InitRecorder(int width, int height, int fps, int bitrate, int keyframeInterval)
    {
        var br = bitrate * 1000000;
        //recorder settings
        recorder = new MP4Recorder(width: width, height: height, frameRate: fps, sampleRate: 0, channelCount: 0, bitrate: br, keyframeInterval: keyframeInterval);

        //clock to match ar frame timings
        clock = new FixedIntervalClock(fps, false);
        OnReceivedFrameUpdate();
        // recording main camera with nat corder
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

    //commiting a frame when a new ar frame is received
    public void CommitVideoFrame(ARCameraFrameEventArgs frame)
    {
        clock.Tick();
    }

    public async void StopRecording()
    {
        OnDisponseFrameUpdate();
        // Stop commiting frames to the recorder
        cameraInput.Dispose();
        // Finish writing
        var path = await recorder.FinishWriting();

        //file name
        string time = FileManagement.GetDateTime();
        string filename = $"{time}_REC";

        NativeGallery.SaveVideoToGallery(existingMediaPath: path, album: "Retargeter", filename: filename);
    }
}
