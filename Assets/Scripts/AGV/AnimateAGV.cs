using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class AnimateAGV : MonoBehaviour
{
    WebSocketClient webSocketClient;
    
    private Animator animator;

    private int currentStatus = 0;
    private int lastStatus = -1;

    private const int STATUS_IDLE = 1;
    private const int STATUS_LOADING = 2;

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
        webSocketClient = FindObjectOfType<WebSocketClient>();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator component not found on this GameObject!");
        }

        if (webSocketClient == null)
            Debug.LogError("WebSocketClient not found in the scene!");

        if (animator == null)
            Debug.LogError("Animator component not found on this GameObject!");
    }

    void Update()
    {
        if (webSocketClient != null && webSocketClient.agv2Message != null && webSocketClient.newmessageArrviedAGV2)
        {
            webSocketClient.newmessageArrviedAGV2 = false;
            AGVMessage message = JsonUtility.FromJson<AGVMessage>(webSocketClient.agv2Message);

            currentStatus = message.status;



            UpdateAnimation();

        }
    }

    private void UpdateAnimation()
    {
        if (currentStatus != lastStatus)
        {
            animator.SetBool("idle", currentStatus == STATUS_IDLE || currentStatus == 0);
            animator.SetBool("loading", currentStatus == STATUS_LOADING);
            lastStatus = currentStatus;
        }
    }
}
