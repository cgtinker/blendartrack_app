using UnityEngine;
using ArRetarget;

public class DataManager : MonoBehaviour
{
    JsonSerializer jsonSerializer;

    private CameraPoseHandler cameraPoseHandler;
    private FaceMeshHandler faceMeshHandler;

    private string attachmentPath;
    private bool _recording;
    private int frame = 0;

    void Start()
    {
        attachmentPath = Application.persistentDataPath;
        _recording = false;
        jsonSerializer = this.gameObject.GetComponent<JsonSerializer>();
        Debug.Log("Session started");
    }

    public void AssignDataType()
    {

        switch (DeviceManager.Instance.Ability)
        {
            case DeviceManager.TrackingType.ArCore_CameraPose:
                cameraPoseHandler = GameObject.FindGameObjectWithTag("retarget").GetComponent<CameraPoseHandler>();
                break;

            case DeviceManager.TrackingType.ArCore_FaceMesh:
                faceMeshHandler = GameObject.FindGameObjectWithTag("retarget").GetComponent<FaceMeshHandler>();
                break;

            case DeviceManager.TrackingType.ArKit_CameraPose:
                break;

            case DeviceManager.TrackingType.ArKit_BlendShapes:
                break;
        }
    }

    public void ToggleRecording()
    {
        if (!_recording)
            InitRetargeting();

        _recording = !_recording;

        Debug.Log($"Recording: {_recording}");
    }

    private void InitRetargeting()
    {
        switch (DeviceManager.Instance.Ability)
        {
            case DeviceManager.TrackingType.ArCore_CameraPose:
                cameraPoseHandler.Init();
                break;
            case DeviceManager.TrackingType.ArCore_FaceMesh:
                faceMeshHandler.Init();
                break;
            case DeviceManager.TrackingType.ArKit_CameraPose:
                break;
            case DeviceManager.TrackingType.ArKit_BlendShapes:
                break;
        }

        Debug.Log("Init Retargeting");
    }

    private void Update()
    {
        if (_recording)
        {
            frame++;

            switch (DeviceManager.Instance.Ability)
            {
                case DeviceManager.TrackingType.ArCore_CameraPose:
                    cameraPoseHandler.GetCameraPoseData(frame);
                    break;

                case DeviceManager.TrackingType.ArCore_FaceMesh:
                    faceMeshHandler.ProcessMeshVerts(frame);
                    break;

                case DeviceManager.TrackingType.ArKit_CameraPose:
                    break;
                case DeviceManager.TrackingType.ArKit_BlendShapes:
                    break;
            }
        }
    }

    public void SerializeJson()
    {
        //DeleteFile.FileAtMediaPath(attachmentPath + "/*.json");
        Debug.Log("Serializing json");

        //TODO: Interface for serialization
        switch (DeviceManager.Instance.Ability)
        {
            case DeviceManager.TrackingType.ArCore_CameraPose:
                PoseDataContainer cpd = new PoseDataContainer()
                {
                    poseList = cameraPoseHandler.cameraPoseList
                };

                jsonSerializer.SerializeCameraPoseData(cpd, attachmentPath);
                break;

            case DeviceManager.TrackingType.ArCore_FaceMesh:
                MeshDataContainer mvd = new MeshDataContainer()
                {
                    meshDataList = faceMeshHandler.meshDataList
                };

                jsonSerializer.SerializeMeshData(mvd, attachmentPath);
                break;

            case DeviceManager.TrackingType.ArKit_CameraPose:
                break;
            case DeviceManager.TrackingType.ArKit_BlendShapes:
                break;
        }
    }
}