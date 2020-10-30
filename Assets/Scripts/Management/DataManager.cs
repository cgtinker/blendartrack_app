using UnityEngine;
using ArRetarget;

public class DataManager : MonoBehaviour
{
    JsonSerializer jsonSerializer;

    private string persistentPath;
    private bool _recording;
    private int frame = 0;

    private IGet<int> getter;
    private IJson json;
    private IInit init;
    private IStop stop;

    void Start()
    {
        persistentPath = Application.persistentDataPath;
        _recording = false;
        jsonSerializer = this.gameObject.GetComponent<JsonSerializer>();
        Debug.Log("Session started");
    }

    public void TrackingReference(GameObject obj)
    {
        if (obj.GetComponent<IInit>() != null && obj.GetComponent<IJson>() != null)
        {
            Debug.Log("Received Tracker Init & Json Reference");
            init = obj.GetComponent<IInit>();
            json = obj.GetComponent<IJson>();
        }

        if (obj.GetComponent<IGet<int>>() != null)
        {
            Debug.Log("Received Tracker Get Reference");
            getter = obj.GetComponent<IGet<int>>();
        }

        if (obj.GetComponent<IStop>() != null)
        {
            stop = obj.GetComponent<IStop>();
            Debug.Log("Received Tracker Stop Reference");
        }
    }

    public void ToggleRecording()
    {
        if (!_recording)
            InitRetargeting();

        else
            StopRetargeting();

        _recording = !_recording;

        Debug.Log($"Recording: {_recording}");
    }

    private void InitRetargeting()
    {
        Debug.Log("Init Retargeting");
        init.Init();
    }

    private void StopRetargeting()
    {
        if (stop != null)
        {
            Debug.Log("Stop Retargeting");
            stop.StopTracking();
        }
    }

    private void Update()
    {
        if (_recording && getter != null)
        {
            frame++;
            getter.GetFrameData(frame);
        }
    }

    public void SerializeJson()
    {
        string tmp = json.GetJsonString();
        jsonSerializer.SerializeData(data: tmp, persistentPath: persistentPath, prefix: "prefix");
    }
}