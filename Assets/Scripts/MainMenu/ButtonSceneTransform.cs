using UnityEngine;
using UnityEngine.SceneManagement; // Include this for scene management

public class ButtonSceneTransform : MonoBehaviour
{
    public string sceneToLoad; // The name of the scene to load when the button is pressed

    void Start()
    {
        // Optional: You could do any setup here if needed
    }

    void Update()
    {
        // Check for button press (e.g., using the space bar or a mouse button)
        if (Input.GetKeyDown(KeyCode.Space)) // Change KeyCode.Space to your desired input
        {
            LoadScene();
        }
    }

    public void LoadScene()
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
