using UnityEngine;
using UnityEngine.SceneManagement;

public class InputIPScript : MonoBehaviour
{
    public string IPAddress = "";
    public bool isIPSet = false; // for button dissable and scene transform


    private void Start()
    {

    }

    public void ReadStringinput(string message)
    {

        IPAddress = message.Trim();
        Debug.Log("IP Address is set to: " + IPAddress);
        isIPSet = true;


    }


}
