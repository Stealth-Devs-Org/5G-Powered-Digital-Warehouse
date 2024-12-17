using UnityEngine;
using UnityEngine.SceneManagement; // Include this for scene management

public class ButtonSceneTransform : MonoBehaviour
{
    public string sceneToLoad; // The name of the scene to load when the button is pressed
    private Animator animator; // Animator component

    void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on this GameObject!");
        }
    }

    void Update()
    {
        // Check for mouse input and perform raycasting
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the object clicked is this GameObject
                if (hit.transform == transform)
                {
                    LoadScene(); // Load the scene if this button is clicked
                    if (animator != null)
                    {
                        animator.SetTrigger("OnClick"); // Trigger the animation
                    }
                }
            }
        }
    }

    public void LoadScene()
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
