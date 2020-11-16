using System;
using System.IO;
using System.Collections;
using UnityEngine;

namespace ArRetarget
{
    public class JsonSerializer : MonoBehaviour
    {
        public void WriteDataToDisk(string data, string persistentPath, string filename)
        {
            Debug.Log("Serializing json data");

            var path = $"{persistentPath}/{filename}.json";
            File.WriteAllText(path: path, contents: data, encoding: System.Text.Encoding.UTF8);
        }
    }
}
