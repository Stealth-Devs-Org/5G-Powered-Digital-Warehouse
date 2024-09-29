using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGVMovement : MonoBehaviour
{
    AGVController AGVController;
    
    public float speedofAGV = 5.0f;
    public float rotationSpeed = 0.5f;
    WebSocketDataReceive webSocketDataReceive;
    

    

    




    // Start is called before the first frame update
    void Start()
    {
        AGVController = GetComponent<AGVController>();
        webSocketDataReceive = GetComponent<WebSocketDataReceive>();
    }

    // Update is called once per frame
    void Update()
    {
        
        for (int i = 0; i < webSocketDataReceive.numberOfAGV; i++)
        {
            if (AGVController.AGVPresent[i]==true)
            {

                //MoveAGV();
                
            
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
