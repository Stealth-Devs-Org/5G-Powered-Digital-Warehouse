using TMPro;
using UnityEngine;

public class AutoLabelGrandChildren : MonoBehaviour
{
    public GameObject labelPrefab;  // Drag your GameObject prefab here
    public Transform grandParent;   // The grandparent object containing all spheres

    void Start()
    {
        // Iterate through all direct children of the grandparent
        foreach (Transform parent in grandParent)
        {
            // Iterate through all children of each child (grandchildren of the grandparent)
            foreach (Transform sphere in parent)
            {
                Vector3 position = sphere.position;
                Collider sphereCollider = sphere.GetComponent<Collider>();
                sphereCollider.isTrigger = true;

                // Instantiate the label at the sphere's position with a small offset
                GameObject labelObject = Instantiate(labelPrefab, position + new Vector3(0, 0.5f, 0), Quaternion.identity);

                // Get the TextMeshPro component from the instantiated label object
                TextMeshPro label = labelObject.GetComponent<TextMeshPro>();

                if (label != null)
                {
                    // Set the label text to the sphere's coordinates
                    label.text = $"({position.x}, {position.y}, {position.z})";

                    // Optionally, make the label a child of the sphere to keep it aligned
                    label.transform.SetParent(sphere);

                    // Adjust the label orientation to always face the camera (optional)
                    label.transform.LookAt(Camera.main.transform);
                    label.transform.Rotate(0, 180, 0);
                }
                else
                {
                    Debug.LogError("The prefab does not have a TextMeshPro component attached!");
                }
            }
        }
    }
}
