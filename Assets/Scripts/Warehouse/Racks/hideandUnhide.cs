using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public GameObject targetObject; // Assign this in Inspector

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (targetObject != null)
            {
                bool isActive = targetObject.activeSelf;
                targetObject.SetActive(!isActive);
            }
        }
    }
}
