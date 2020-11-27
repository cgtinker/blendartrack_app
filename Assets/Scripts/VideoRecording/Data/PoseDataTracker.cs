using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SpatialTracking;
using ArRetarget;

public class PoseDataTracker : MonoBehaviour
{
    public TrackedPoseDriver poseDiver;
    public List<PoseData> cameraPoseList;
    private JsonSerializer jsonSerializer;
    private int frame;
    public GameObject obj;

    public void Init()
    {
        jsonSerializer = this.gameObject.GetComponent<JsonSerializer>();
        frame = 0;
        poseDiver = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponentInChildren<TrackedPoseDriver>();
        OnEnableTracking();
        obj = GameObject.FindGameObjectWithTag("MainCamera");

    }

    public void Stop()
    {
        OnDisableTracking();
        CameraPoseContainer container = new CameraPoseContainer();
        container.cameraPoseList = cameraPoseList;
        var json = JsonUtility.ToJson(container);
        SerializeJson(json);
    }

    private void SerializeJson(string json)
    {
        Debug.Log("trying to serialize json");
        //file contents
        string contents = json;

        //file name
        string prefix = "PD";
        string time = FileManagementHelper.GetDateTime();
        string filename = $"{time}_{prefix}.json";
        var persistentPath = Application.persistentDataPath;

        //write data
        jsonSerializer.WriteDataToDisk(data: contents, persistentPath: persistentPath, filename: filename);
    }


    protected virtual void OnEnableTracking()
    {
        Application.onBeforeRender += OnBeforeRender;
    }

    protected virtual void OnDisableTracking()
    {
        Application.onBeforeRender -= OnBeforeRender;
    }

    protected virtual void OnBeforeRender()
    {
        {
            OnPreformUpdate();
        }
    }

    protected virtual void OnPreformUpdate()
    {
        frame++;
        PoseData cur_pose = GetPoseData(obj, frame);
        cameraPoseList.Add(cur_pose);
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
