using ArRetarget;
using UnityEngine;

public class TrackingDataManager : MonoBehaviour
{
    JsonSerializer jsonSerializer;

    private string persistentPath;
    private bool _recording;
    private int frame = 0;

    private IGet<int> getter;
    private IJson json;
    private IInit init;
    private IStop stop;
    private IPrefix prefix;

    #region initialize tracking session
    void Start()
    {
        //prepare for tracking and serializing
        persistentPath = Application.persistentDataPath;
        _recording = false;
        jsonSerializer = this.gameObject.GetComponent<JsonSerializer>();
        Debug.Log("Session started");
    }

    //resetting as not all tracking models include all tracker interfaces
    public void ResetTrackerInterfaces()
    {
        getter = null;
        json = null;
        init = null;
        stop = null;
        prefix = null;
    }

    //the tracking references always contain some of the following interfaces
    public void TrackingReference(GameObject obj)
    {
        ResetTrackerInterfaces();

        //iinit -> setup || ijson -> serialze
        if (obj.GetComponent<IInit>() != null && obj.GetComponent<IJson>() != null)
        {
            init = obj.GetComponent<IInit>();
            json = obj.GetComponent<IJson>();
            prefix = obj.GetComponent<IPrefix>();
        }

        //iget -> pulling data
        if (obj.GetComponent<IGet<int>>() != null)
        {
            getter = obj.GetComponent<IGet<int>>();
        }

        //istop -> stop pushing data
        if (obj.GetComponent<IStop>() != null)
        {
            stop = obj.GetComponent<IStop>();
        }

        Debug.Log("Receiving Tracker Type Reference");
    }
    #endregion

    #region capturing
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
    #endregion

    public string SerializeJson()
    {
        //file contents
        string contents = json.GetJsonString();

        //file name
        string prefix = this.prefix.GetJsonPrefix();
        string time = FileManagementHelper.GetDateTime();
        string filename = $"{time}_{prefix}.json";

        //write data
        jsonSerializer.WriteDataToDisk(data: contents, persistentPath: persistentPath, filename: filename);

        return filename;
    }
}