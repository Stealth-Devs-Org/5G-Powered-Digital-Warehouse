using System.IO;
using UnityEngine;

public class FileReader : MonoBehaviour
{
    private string fileName = "IP_Addressdata.txt";

    string filePath;


    void Start()
    {   
        filePath = Path.Combine(Application.dataPath, "../" + fileName);
        
    }

    void Update()
    {
        // Path to the executable folder
        

        // Check if the file exists before reading
        if (File.Exists(filePath))
        {
            // Read the string from the file
            string dataRead = File.ReadAllText(filePath);
            Debug.Log("Data read from file: " + dataRead);
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
        }
    }
}
