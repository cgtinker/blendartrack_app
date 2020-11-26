using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArRetarget;

[RequireComponent(typeof(CameraIntrinsicsHandler))]
public class IntrinsicsCaptureManager : MonoBehaviour
{
    private CameraIntrinsicsHandler cameraIntrinsicsHandler;
    private JsonSerializer jsonSerializer;

    public bool recording = false;

    // Start is called before the first frame update
    void Start()
    {
        cameraIntrinsicsHandler = this.gameObject.GetComponent<CameraIntrinsicsHandler>();
        jsonSerializer = this.gameObject.GetComponent<JsonSerializer>();
    }

    public void OnStartRecording()
    {
        recording = true;
    }

    public void OnStopRecording()
    {
        recording = false;
        Debug.Log("writing camera intrinsics json");
        var container = cameraIntrinsicsHandler.GetCameraIntrinsicsContainer();

        var json = JsonUtility.ToJson(container);
        SerializeJson(json);
    }

    private void SerializeJson(string json)
    {
        Debug.Log("trying to serialize json");
        //file contents
        string contents = json;

        //file name
        string prefix = "CI";
        string time = FileManagementHelper.GetDateTime();
        string filename = $"{time}_{prefix}.json";
        var persistentPath = Application.persistentDataPath;

        cameraIntrinsicsHandler.ClearCache();

        //write data
        jsonSerializer.WriteDataToDisk(data: contents, persistentPath: persistentPath, filename: filename);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!recording)
            return;

        cameraIntrinsicsHandler.CacheCameraRelatedSettings();
    }
}
