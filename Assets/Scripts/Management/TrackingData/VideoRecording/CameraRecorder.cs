using UnityEngine;
using ArRetarget;
using NatSuite.Recorders;
using NatSuite.Recorders.Clocks;
using NatSuite.Recorders.Inputs;
using UnityEngine.XR.ARFoundation;
using System.Collections;

public class CameraRecorder : MonoBehaviour, IInit, IStop
{
    private CameraInput cameraInput;
    private MP4Recorder recorder;

    private FixedIntervalClock clock;
    //private ARCameraManager arCameraManager;

    private bool recording;
    /*
    //initialize same as a recorder
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        var dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<TrackingDataManager>();
        dataManager.SetRecorderReference(this.gameObject);
    }
    */
    public void Init()
    {
        //referencing the ar camera
        var sessionOrigin = GameObject.FindGameObjectWithTag("arSessionOrigin");
        //arCameraManager = sessionOrigin.GetComponentInChildren<ARCameraManager>();
        XRCameraConfigHandler config = sessionOrigin.GetComponentInChildren<XRCameraConfigHandler>();

        //assigning running config
        int? fps = config.activeXRCameraConfig.framerate;
        int width = config.activeXRCameraConfig.height;
        int height = config.activeXRCameraConfig.width;

        //init the recorder
        if (fps == 0 || fps == null)
            InitRecorder(width, height, 30, 8, 3);

        else
            InitRecorder(width, height, (int)fps, 8, 3);
    }

    private void InitRecorder(int width, int height, int fps, int bitrate, int keyframeInterval)
    {
        var br = bitrate * 1000000;
        //recorder settings
        recorder = new MP4Recorder(width: width, height: height, frameRate: fps, sampleRate: 0, channelCount: 0, bitrate: br, keyframeInterval: keyframeInterval);

        //clock to match ar frame timings
        //clock = new FixedIntervalClock(fps, false);
        clock = new FixedIntervalClock(fps, true);

        //OnReceivedFrameUpdate();
        recording = true;
        // recording main camera with nat corder
        cameraInput = new CameraInput(recorder, clock, Camera.main);
    }
    /*
     * as its after receiving a frame, its probaly better to stick to the usual update method
    private void OnReceivedFrameUpdate()
    {
        arCameraManager.frameReceived += CommitVideoFrame;
    }

    private void OnDisponseFrameUpdate()
    {
        arCameraManager.frameReceived -= CommitVideoFrame;
    }
    
    //commiting a frame when a new ar frame is received
    public void CommitVideoFrame()//(ARCameraFrameEventArgs frame)
    {
        clock.Tick();
    }
    */
    public async void StopTracking()
    {
        //OnDisponseFrameUpdate();
        // Stop commiting frames to the recorder
        recording = false;
        cameraInput.Dispose();
        // Finish writing
        var path = await recorder.FinishWriting();

        //file name
        string time = FileManagement.GetDateTime();
        string filename = $"{time}_REC";

        NativeGallery.SaveVideoToGallery(existingMediaPath: path, album: "Retargeter", filename: filename);
    }
}
