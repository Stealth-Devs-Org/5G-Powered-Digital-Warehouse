using System.Collections;
using System.Collections.Generic;
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

    private GameObject currentLoad;

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
        if (gameObject.name == "AGV1")
        {
            currentStatus = aGV1Controller.currentStatus;
        }
        else if (gameObject.name == "AGV2")
        {
            currentStatus = aGV2Controller.currentStatus;
        }
        else if (gameObject.name == "AGV3")
        {
            currentStatus = aGV3Controller.currentStatus;
        }
        else if (gameObject.name == "AGV4")
        {
            currentStatus = aGV4Controller.currentStatus;
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (currentStatus == 2) 
        {
            animator.SetBool("idle", false);
            animator.SetBool("loading", true);

            if (currentLoad == null)
            {
                Transform rigMast = transform.Find("Rig_root/Lift_mast_2/Rig_mast_2");
                if (rigMast != null)
                {
                    currentLoad = Instantiate(LoadPrefab, rigMast.position, rigMast.rotation);
                    currentLoad.transform.SetParent(rigMast, true);
                    currentLoad.tag = "Load"; 
                }
            }
        }
        else if (currentStatus == 3) 
        {
            Debug.Log("Unloading");
            animator.SetBool("idle", false);
            animator.SetBool("loading", true);

            if (currentLoad != null)
            {
                
                StartCoroutine(DestroyLoadAfterDelay(1.0f)); 
            }
        }
        else
        {
            animator.SetBool("idle", true);
            animator.SetBool("loading", false);
        }
    }

    private IEnumerator DestroyLoadAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); 

        
        Destroy(currentLoad);
        currentLoad = null; 
    }
}
