using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath;

    public FileDataHandler(string dataDirPath)
    {
        this.dataDirPath = dataDirPath;
    }

    public string Load(string fileName)
    {
        string fullPath = Path.Combine(this.dataDirPath, fileName);

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

    public void Save(string fileName, string data)
    {
        string fullPath = Path.Combine(this.dataDirPath, fileName);

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