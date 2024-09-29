using System.Collections;
using UnityEngine;
using TMPro;
using System.Numerics;

public class GetLocationOfSphere : MonoBehaviour
{
    public Transform grandParent;
    public Vector2Int spherelocation = new Vector2Int(0,0);
    public Vector2Int location;

    void Update()
    {
        //location = ReturnLocationCordinate(spherelocation);
        //Debug.Log("Location: " + location);
    }

    public Vector2Int ReturnLocationCordinate(Vector2Int spherelocation) //Return real location of shpere by giving coordinate (0,-1)
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

                    Vector2Int locationVector = ConvertStringToVector2Int(textMesh.text);

                    if (locationVector == spherelocation)
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



    Vector2Int ConvertStringToVector2Int(string input)
    {
        
        input = input.Trim(new char[] { '(', ')' });
        string[] values = input.Split(',');

      
        int x = int.Parse(values[0].Trim());
        int y = int.Parse(values[1].Trim());

       
        return new Vector2Int(x, y);
    }
}
