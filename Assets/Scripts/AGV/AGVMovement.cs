using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGVMovement : MonoBehaviour
{
    AGVController AGVController;
    public int numberOfAGV = 4;
    public float speedofAGV = 5.0f;
    public float rotationSpeed = 0.5f;
    private bool[] AGVPresent;

    

    




    // Start is called before the first frame update
    void Start()
    {
        AGVController = GetComponent<AGVController>();

        AGVPresent = new bool[numberOfAGV];
        for (int i = 0; i < numberOfAGV; i++)
        {
            AGVPresent[i] = false;  //initially no AGV present...
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
        for (int i = 0; i < numberOfAGV; i++)
        {
            if (AGVPresent[i])
            {
                //MoveAGV(AGVController.AGVs[i].transform.position);
            }
        }
        

        // gameObject.transform.Translate(Vector3.forward * Time.deltaTime * speedofAGV);  
        // gameObject.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);      
    }




    public void MoveAGV(Vector3 targetPosition)
    {

        Vector3 direction = targetPosition - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        float distance = direction.magnitude;

        if (angle > 1.0f)
        {
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotationSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speedofAGV * Time.deltaTime);
        }
    }
}
