using UnityEngine;
using System.Collections.Generic;
using ArRetarget;

public class SessionOriginPosition : MonoBehaviour
{
    public GameObject ArSessionOrigin;
    public GameObject ArSession;
    private List<PoseData> arSessionPose = new List<PoseData>();
    private List<PoseData> arOriginPose = new List<PoseData>();
    JsonSerializer jsonSerializer;
    bool recording;
    int frame = 0;

    public void Init()
    {
        jsonSerializer = this.gameObject.GetComponent<JsonSerializer>();
        ArSession = GameObject.Find("Trackables");
        ArSessionOrigin = GameObject.FindGameObjectWithTag("arSessionOrigin");
        recording = true;
    }

    public void Stop()
    {
        recording = false;
        SessionPose m_data = new SessionPose();

        m_data.originPose = arOriginPose;
        m_data.sessionPose = arSessionPose;

        var json = JsonUtility.ToJson(m_data);
        SerializeJson(json);
    }

    private void SerializeJson(string json)
    {
        Debug.Log("trying to serialize json");
        //file contents
        string contents = json;

        //file name
        string prefix = "SO";
        string time = FileManagementHelper.GetDateTime();
        string filename = $"{time}_{prefix}.json";
        var persistentPath = Application.persistentDataPath;

        //write data
        jsonSerializer.WriteDataToDisk(data: contents, persistentPath: persistentPath, filename: filename);
    }

    private void Update()
    {
        if (!recording)
            return;

        frame++;
        PoseData originPose = GetPoseData(ArSessionOrigin, frame);
        PoseData sessionPose = GetPoseData(ArSession, frame);

        arSessionPose.Add(sessionPose);
        arOriginPose.Add(originPose);
    }

    public static PoseData GetPoseData(GameObject obj, int frame)
    {
        var pos = new Vector()
        {
            x = obj.transform.position.x,
            y = obj.transform.position.y,
            z = obj.transform.position.z
        };

        var rot = new Vector()
        {
            x = obj.transform.eulerAngles.x,
            y = obj.transform.eulerAngles.y,
            z = obj.transform.eulerAngles.z,
        };

        var cameraPose = new PoseData()
        {
            pos = pos,
            rot = rot,

            frame = frame
        };

        return cameraPose;
    }
}

public class SessionPose
{
    public List<PoseData> sessionPose;
    public List<PoseData> originPose;
}