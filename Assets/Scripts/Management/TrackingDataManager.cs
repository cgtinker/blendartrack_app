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

    //the tracking references always contain some of the following interfaces
    public void TrackingReference(GameObject obj)
    {
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

    public void SerializeJson()
    {
        string tmp = json.GetJsonString();
        string tlt = prefix.GetJsonPrefix();
        jsonSerializer.SerializeData(data: tmp, persistentPath: persistentPath, prefix: tlt);
    }
}