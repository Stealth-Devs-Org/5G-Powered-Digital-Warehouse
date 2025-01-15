// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI; // For interacting with UI elements

// public class ButtonSceneTransform : MonoBehaviour
// {
//     public string sceneToLoad; 
//     public Button buttonToLoadScene; 
//     FileWriter fileWriter;

//     private void Start()
//     {
//         fileWriter = FindAnyObjectByType<FileWriter>();
//         if (buttonToLoadScene != null)
//         {
//             buttonToLoadScene.interactable = false;

//             // Change the button's color to a faded version
//             ColorBlock colorBlock = buttonToLoadScene.colors;
//             colorBlock.disabledColor = new Color(0.4f, 0.4f, 0.4f, 0.5f); // White with 50% transparency
//             buttonToLoadScene.colors = colorBlock;
//         }
//     }

//     private void Update()
//     {
//         Debug.Log(fileWriter.isFileWritten);
//         if (fileWriter.isFileWritten) 
//         {
//             buttonToLoadScene.interactable = true;
//             // Change the button's color to a faded version
//             ColorBlock colorBlock = buttonToLoadScene.colors;
//             colorBlock.disabledColor = new Color(1f, 1f, 1f, 1f); // White with 50% transparency
//             buttonToLoadScene.colors = colorBlock;
//         }

//         if (Input.GetKeyDown(KeyCode.Return) && buttonToLoadScene.interactable)
//         {
//             LoadScene();
//         }
//     }

//     public void LoadScene()
//     {
//         if (!string.IsNullOrEmpty(sceneToLoad))
//         {
//             SceneManager.LoadScene(sceneToLoad);
//         }
//         else
//         {
//             Debug.LogError("Scene name is empty or null. Please specify a valid scene name.");
//         }
//     }
// }













// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI; // For interacting with UI elements



// public class ButtonSceneTransform : MonoBehaviour
// {
//     public string sceneToLoad;
//     public Button buttonToLoadScene;
//     FileWriter fileWriter;

//     private void Start()
//     {
//         fileWriter = FindAnyObjectByType<FileWriter>();
//         if (buttonToLoadScene != null)
//         {
//             buttonToLoadScene.interactable = false;

//             buttonToLoadScene.colors =rgb(116, 116, 116);
//         }
//     }

//     private void Update()
//     {
//         Debug.Log(fileWriter.isFileWritten);
//         if (fileWriter.isFileWritten)
//         {
//             buttonToLoadScene.interactable = true;


//             buttonToLoadScene.colors =rgb(255, 255, 255);
//         }

//         if (Input.GetKeyDown(KeyCode.Return) && buttonToLoadScene.interactable)
//         {
//             LoadScene();
//         }
//     }

//     public void LoadScene()
//     {
//         if (!string.IsNullOrEmpty(sceneToLoad))
//         {
//             SceneManager.LoadScene(sceneToLoad);
//         }
//         else
//         {
//             Debug.LogError("Scene name is empty or null. Please specify a valid scene name.");
//         }
//     }
// }






using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonSceneTransform : MonoBehaviour
{
    public string sceneToLoad;
    public Button buttonToLoadScene;
    private Image buttonImage; // Reference to the button's image component
    FileWriter fileWriter;

    private void Start()
    {
        fileWriter = FindAnyObjectByType<FileWriter>();
        if (buttonToLoadScene != null)
        {
            // Get the Image component of the button
            buttonImage = buttonToLoadScene.GetComponent<Image>();
            
            buttonToLoadScene.interactable = false;

            if (buttonImage != null)
            {
                // Set the initial disabled color (e.g., gray)
                buttonImage.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
    }

    private void Update()
    {
        Debug.Log(fileWriter.isFileWritten);
        if (fileWriter.isFileWritten)
        {
            buttonToLoadScene.interactable = true;

            if (buttonImage != null)
            {
                // Change the color to white when the file is written
                buttonImage.color = Color.white;
            }
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
