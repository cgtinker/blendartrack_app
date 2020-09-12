using System;
using System.IO;
using System.Collections;
using UnityEngine;
using ArRetarget;

public class JsonSerializer : MonoBehaviour
{
    public void SerializeMeshData(MeshDataContainer data, string persistentPath)
    {
        var json = JsonUtility.ToJson(data);
        var path = persistentPath + Time().ToString() + "/FaceMeshData.json";
        JsonSerialization(json, path);
    }

    public void SerializeCameraPoseData(PoseDataContainer data, string persistentPath)
    {
        var json = JsonUtility.ToJson(data);
        var path = $"{persistentPath}/CameraPose{Time()}.json";
        //var path = persistentPath + Time().ToString() + "/CameraPoseData.json";
        JsonSerialization(json, path);
    }

    public DateTime Time()
    {
        DateTime time = DateTime.Now;
        return time;
    }

    private void JsonSerialization(string json, string attachmentPath)
    {
        //json in app data
        File.WriteAllText(path: attachmentPath, contents: json, encoding: System.Text.Encoding.UTF8);

        DateTime localDate = DateTime.Now;
        string mailSubject = "Ar Retarget " + localDate.ToString();
        string s = Environment.NewLine;
        string mailText = "Ar Retarget " + localDate.ToString();

        StartCoroutine(NativeShare(filePath: attachmentPath, subject: mailSubject, text: mailText));
    }

    private IEnumerator NativeShare(string filePath, string subject, string text)
    {
        yield return new WaitForEndOfFrame();
        //new NativeShare().AddFile(filePath).SetSubject(subject).SetText(text).Share();
        new NativeShare().AddFile(filePath).SetSubject(subject).SetTarget("target").SetTitle("Title").SetText(text).Share();
    }
}
