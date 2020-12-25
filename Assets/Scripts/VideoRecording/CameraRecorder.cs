using UnityEngine;
using ArRetarget;
using NatSuite.Recorders;
using NatSuite.Recorders.Clocks;
using NatSuite.Recorders.Inputs;
using UnityEngine.XR.ARFoundation;
using System.Collections;

public class CameraRecorder : MonoBehaviour, IInit<string, string>, IStop
{
    private CameraInput cameraInput;
    private MP4Recorder recorder;

    private FixedIntervalClock clock;
    //private ARCameraManager arCameraManager;

    private bool recording;
    private string mediaDesitinationPath;
    string title;
    public void Init(string path, string title)
    {
        mediaDesitinationPath = path;
        this.title = title;
        //referencing the ar camera
        var sessionOrigin = GameObject.FindGameObjectWithTag("arSessionOrigin");
        //arCameraManager = sessionOrigin.GetComponentInChildren<ARCameraManager>();
        XRCameraConfigHandler config = sessionOrigin.GetComponentInChildren<XRCameraConfigHandler>();

        //assigning running config
        int? fps = config.activeXRCameraConfig.framerate;
        //int width = config.activeXRCameraConfig.height;
        //int height = config.activeXRCameraConfig.width;

        (int width, int height) = GetScreenDimensions(config);

        //init the recorder
        if (fps == 0 || fps == null)
            InitRecorder(width, height, 30, 8, 3);

        else
            InitRecorder(width, height, (int)fps, 8, 3);
    }

    private (int, int) GetScreenDimensions(XRCameraConfigHandler config)
    {
        if (Screen.orientation == ScreenOrientation.Landscape || Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            int height = config.activeXRCameraConfig.height;
            int width = config.activeXRCameraConfig.width;

            return (width, height);
        }

        else if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            int width = config.activeXRCameraConfig.height;
            int height = config.activeXRCameraConfig.width;

            return (width, height);
        }

        else
        {
            int width = config.activeXRCameraConfig.height;
            int height = config.activeXRCameraConfig.width;

            return (height, width);
        }
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
        Camera cam = GameObject.FindGameObjectWithTag("recorder").GetComponent<Camera>();
        // recording main camera with nat corder
        cameraInput = new CameraInput(recorder, clock, cam);
    }

    public async void StopTracking()
    {
        //OnDisponseFrameUpdate();
        // Stop commiting frames to the recorder
        recording = false;
        cameraInput.Dispose();

        // Finish writing
        var path = await recorder.FinishWriting();

        //file name
        string filename = $"{title}_REC.mov";
        Debug.Log("dest: " + mediaDesitinationPath);

        if (PlayerPrefs.GetInt("vidzip", -1) == 1)
            FileManagement.CopyFile(sourceFile: path, destFile: $"{mediaDesitinationPath}{filename}");
        if (PlayerPrefs.GetInt("vidgal", -1) == 1)
            NativeGallery.SaveVideoToGallery(existingMediaPath: path, album: "Retargeter", filename: filename);
    }
}
