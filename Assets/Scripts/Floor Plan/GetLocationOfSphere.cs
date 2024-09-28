using System.Collections;
using UnityEngine;
using TMPro;

public class GetLocationOfSphere : MonoBehaviour
{
    public Transform grandParent;
    public string spherelocation = "0,0";
    public Vector2Int location;

    void Update()
    {
        location = ReturnLocationCordinate(spherelocation);
        Debug.Log("Location: " + location);
    }

    Vector2Int ReturnLocationCordinate(string spherelocation)
    {
        foreach (Transform parent in grandParent)
        {
            foreach (Transform sphere in parent)
            {
                TMP_Text textMesh = sphere.GetComponentInChildren<TMP_Text>();
        
                if (textMesh != null)
                {
                    int x = Mathf.RoundToInt(sphere.position.x);
                    int z = Mathf.RoundToInt(sphere.position.z);

                    

                    if (textMesh.text == spherelocation)
                    {

                        location = new Vector2Int(x, z);
                        //Debug.Log("Sphere coordinate: " + textMesh.text + " Location: " + location);
                        return location;
                    }
                }
            }
        }
        return Vector2Int.zero;
    }
}
