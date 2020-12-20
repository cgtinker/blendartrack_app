using UnityEngine;
using System.Collections;
using System;
using System.IO;


public static class JsonFileWriter
{
    const string quote = "\"";
    const string f = "]}";      //closing json
    const string w = ",";       //listing values

    public static string GetContainerPrefix(string containerName)
    {
        string containerPrefix = "{" + quote + containerName + quote + ":[";
        return containerPrefix;
    }

    public static void WriteDataToFile(string path, string text, string title, bool lastFrame)
    {
        if (!File.Exists(path))
        {
            //create a file to write to
            using (StreamWriter sw = File.CreateText(path))
            {
                string m_prefix = GetContainerPrefix(title);
                sw.WriteLine($"{m_prefix}{text}{w}");
            }
        }

        else
        {
            //if the file exists, append text
            using (StreamWriter sw = File.AppendText(path))
            {
                if (!lastFrame)
                    sw.WriteLine($"{text}{w}");
                else
                    sw.WriteLine($"{text}{f}");
            }
        }
    }
}
