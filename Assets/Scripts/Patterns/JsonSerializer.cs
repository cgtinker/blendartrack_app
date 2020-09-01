using System;
using System.IO;
using System.Collections;
using UnityEngine;
using ArRetarget;

public class JsonSerializer : MonoBehaviour
{
    public void SerializeMeshData(MeshVertDataList data, string attachmentPath)
    {
        var json = JsonUtility.ToJson(data);
        JsonSerialization(json, attachmentPath);
    }

    public void SerializeCameraPoseData(CameraPoseDataList data, string attachmentPath)
    {
        var json = JsonUtility.ToJson(data);
        JsonSerialization(json, attachmentPath);
    }

    private void JsonSerialization(string json, string attachmentPath)
    {
        //json in app data
        File.WriteAllText(attachmentPath, json);

        DateTime localDate = DateTime.Now;
        string mailSubject = "Ar Retarget " + localDate.ToString();
        string s = Environment.NewLine;
        string mailText = "Ar Retarget " + localDate.ToString();

        StartCoroutine(NativeShare(filePath: attachmentPath, subject: mailSubject, text: mailText));
    }

    private IEnumerator NativeShare(string filePath, string subject, string text)
    {
        yield return new WaitForEndOfFrame();
        new NativeShare().AddFile(filePath).SetSubject(subject).SetText(text).Share();
    }
}
