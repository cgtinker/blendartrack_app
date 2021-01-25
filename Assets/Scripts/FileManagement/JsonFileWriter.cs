using System.IO;


public static class JsonFileWriter
{
    const string quote = "\"";
    const string f = "]}";      //closing json list
    const string w = ",";       //listing values

    public static string GetContainerPrefix(string containerName)
    {
        string containerPrefix = "{" + quote + containerName + quote + ":[";
        return containerPrefix;
    }

    //writing data directly in a .json file
    public static void WriteDataToFile(string path, string text, string title, bool lastFrame)
    {
        if (!File.Exists(path))
        {
            //create a file to write to
            using (StreamWriter sw = File.CreateText(path))
            {
                string m_prefix = GetContainerPrefix(title);
                sw.WriteLine($"{m_prefix}");
            }
        }

        else
        {
            //if the file exists, append text
            using (StreamWriter sw = File.AppendText(path))
            {
                if (!lastFrame)
                    sw.WriteLine($"{text}{w}");

                //TODO: different data types requiere different closing types
                else
                    sw.WriteLine($"{text}");
            }
        }
    }
}
