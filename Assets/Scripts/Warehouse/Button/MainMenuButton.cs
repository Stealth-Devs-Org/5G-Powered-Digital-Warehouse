using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public string sceneToLoad; 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Home))
        {
            LoadScene(sceneToLoad);
        }
    }
    private void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene name is empty or null. Please specify a valid scene name.");
        }
    }
}
