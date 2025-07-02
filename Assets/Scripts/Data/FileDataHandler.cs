using System;
using System.IO;
using UnityEngine;

public static class FileDataHandler
{
    private static string dataDirPath;

    public static void SetPath(string _dataDirPath)
    {
        dataDirPath = _dataDirPath;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public static string Load(string fileName)
    {
        string fullPath = Path.Combine(dataDirPath, fileName);

        if (File.Exists(fullPath))
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error occured when trying to get data from file: " + fullPath + " " + e);
                Application.Quit();
            }
        }

        return null;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public static void Save(string fileName, string data)
    {
        string fullPath = Path.Combine(dataDirPath, fileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(data); 
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error occured when trying to save data to file: " + fullPath + " " + e);
            Application.Quit();
        }
    }
}