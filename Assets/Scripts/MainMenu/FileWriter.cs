using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FileWriter : MonoBehaviour
{
    InputIPScript inputIPScript;
    private string fileName = "IP_Addressdata.txt";

    public bool isFileWritten = false;

    void Start()
    {
        inputIPScript = FindFirstObjectByType<InputIPScript>();

    }
    void Update()
    {
        
        // Path to the executable folder
        string filePath = Path.Combine(Application.dataPath, "../" + fileName);

        if(inputIPScript.isIPSet) 
        {   

            string dataToWrite = inputIPScript.IPAddress;
            File.WriteAllText(filePath, dataToWrite);
            Debug.Log("Data written to file: " + filePath);
            isFileWritten = true;
            inputIPScript.isIPSet = false;
            
            

        }
        


    }
}



