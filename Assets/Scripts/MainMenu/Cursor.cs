using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        // Unlock the cursor and make it visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
