using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGVMovement : MonoBehaviour
{
    public float speedofAGV =5.0f;
    public float rotationSpeed = 0.5f;




    // Start is called before the first frame update
    void Start()
    {
        

        
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.Translate(Vector3.forward * Time.deltaTime * speedofAGV);  
        gameObject.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);      
    }
}
