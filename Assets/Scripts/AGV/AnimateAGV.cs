using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class AnimateAGV : MonoBehaviour
{
    WebSocketClient webSocketClient;

    AGV1Controller aGV1Controller;
    AGV2Controller aGV2Controller;
    AGV3Controller aGV3Controller;
    AGV4Controller aGV4Controller;

    
    private Animator animator;

    public GameObject LoadPrefab;

    public int currentStatus = 0;

    private class AGVMessage
    {
        public string agv_id;
        public int[] location;
        public List<int[]> segment;
        public int status;
        public string timestamp;
    }

    void Start()
    {
        aGV1Controller = FindObjectOfType<AGV1Controller>();
        aGV2Controller = FindObjectOfType<AGV2Controller>();
        aGV3Controller = FindObjectOfType<AGV3Controller>();
        aGV4Controller = FindObjectOfType<AGV4Controller>();

        animator = GetComponent<Animator>();

    }

    void Update()
    {
        // if name is AGV1, use agv1Message, otherwise use agv2Message
        if (gameObject.name == "AGV1")
            {
                currentStatus= aGV1Controller.currentStatus;

            }

        else if (gameObject.name == "AGV2")
            {
                currentStatus= aGV2Controller.currentStatus;
            }

        else if (gameObject.name == "AGV3")
            {
                currentStatus= aGV3Controller.currentStatus;
            }

        else if (gameObject.name == "AGV4")
            {
                currentStatus= aGV4Controller.currentStatus;
            }

       


            UpdateAnimation();

        }
    

    private void UpdateAnimation()
    {
        if (currentStatus == 3)
        {
            animator.SetBool("idle", false);
            animator.SetBool("loading", true);
            
            Transform rigMast = transform.Find("Rig_root/Lift_mast_2/Rig_mast_2"); 
            GameObject load = Instantiate(LoadPrefab, rigMast.position, rigMast.rotation);
            load.transform.SetParent(rigMast, true);
        }

        else if (currentStatus == 4)
        {
            animator.SetBool("idle", false);
            animator.SetBool("loading", true);
            // remove the only load only  in chuile of rigmast
                // Find the Rig_mast_2 object in the hierarchy
            Transform rigMast = transform.Find("Rig_root/Lift_mast_2/Rig_mast_2");
            foreach (Transform child in rigMast)
            {
                
                if (child.CompareTag("Load")) 
                {
                    Destroy(child.gameObject); 
                    break;
                }
            }

        }


        else
        {
            animator.SetBool("idle", true);
            animator.SetBool("loading", false);
        }
    }
}


