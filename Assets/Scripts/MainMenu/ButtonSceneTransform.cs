// using System.IO;
// using UnityEngine;
// using UnityEngine.SceneManagement; // Include this for scene management

// public class ButtonSceneTransform : MonoBehaviour
// {
//     public string sceneToLoad; 

//     InputIPScript inputIPScript;

//     // disable this button until IP is set button should not usable


//     private void Start()
//     {

//     }


//     void Update()
//     {
//         // Check for button press (e.g., using the space bar or a mouse button)
//         if (Input.GetKeyDown(KeyCode.Space)) // Change KeyCode.Space to your desired input
//         {
//             LoadScene();
//         }
//     }

//     public void LoadScene()
//     {
//         // Load the specified scene
//         SceneManager.LoadScene(sceneToLoad);
//     }
// }








// using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // For interacting with UI elements

public class ButtonSceneTransform : MonoBehaviour
{
    public string sceneToLoad; 
    public Button buttonToLoadScene; 
    FileWriter fileWriter;

    private void Start()
    {
        fileWriter = FindAnyObjectByType<FileWriter>();
        if (buttonToLoadScene != null)
        {
            buttonToLoadScene.interactable = false;

            // Change the button's color to a faded version
            ColorBlock colorBlock = buttonToLoadScene.colors;
            colorBlock.disabledColor = new Color(1f, 1f, 1f, 0.1f); // White with 50% transparency
            buttonToLoadScene.colors = colorBlock;
        }
    }

    private void Update()
    {
        Debug.Log(fileWriter.isFileWritten);
        if (fileWriter.isFileWritten) 
        {
            buttonToLoadScene.interactable = true;
            // Change the button's color to a faded version
            ColorBlock colorBlock = buttonToLoadScene.colors;
            colorBlock.disabledColor = new Color(1f, 1f, 1f, 1f); // White with 50% transparency
            buttonToLoadScene.colors = colorBlock;
        }

        if (Input.GetKeyDown(KeyCode.Return) && buttonToLoadScene.interactable)
        {
            LoadScene();
        }
    }

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Scene name is empty or null. Please specify a valid scene name.");
        }
    }
}
