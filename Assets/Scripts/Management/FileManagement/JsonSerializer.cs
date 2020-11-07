using System;
using System.IO;
using System.Collections;
using UnityEngine;

namespace ArRetarget
{
    public class JsonSerializer : MonoBehaviour
    {
        public void SerializeData(string data, string persistentPath, string prefix)
        {
            Debug.Log("Serializing json data");

            string time = FileManagementHelper.GetDateTime();
            var path = $"{persistentPath}/{time}{prefix}.json";
            JsonSerialization(data, path);
        }

        private void JsonSerialization(string json, string attachmentPath)
        {
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
            new NativeShare().AddFile(filePath).SetSubject(subject).SetText(text).Share();
        }
    }
}
