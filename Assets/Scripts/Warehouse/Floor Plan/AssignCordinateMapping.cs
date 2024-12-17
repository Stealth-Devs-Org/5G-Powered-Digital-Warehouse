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
                GameObject labelObject = Instantiate(labelPrefab, position + new Vector3(0, 0.25f, 0), Quaternion.Euler(90, 0, 90));

                // Get the TextMeshPro component from the instantiated label object
                TextMeshPro label = labelObject.GetComponent<TextMeshPro>();

                if (label != null)
                {
                    // Calculate X and Y
                    // int X = (int)((49 - position.z) / 2);
                    // int Y = (int)((position.x + 5) / 2);
                    //Debug.Log(position);
                    int X = (int)(((position.z) + 22 )/2);   // equation is x+5/2
                    int Y = (int)(((49- position.x)+6)/2);     // equation is 49-x/2

                    // Set the label text to the sphere's coordinates
                    label.text = $"({X}, {Y})";


                    // Optionally, make the label a child of the sphere to keep it aligned
                    label.transform.SetParent(sphere);

                    // Adjust the label orientation to always face the camera (optional)
                    //label.transform.LookAt(Camera.main.transform);
                    //label.transform.Rotate(0, 180, 0);
                }
                else
                {
                    Debug.LogError("The prefab does not have a TextMeshPro component attached!");
                }
            }
        }
    }
}
