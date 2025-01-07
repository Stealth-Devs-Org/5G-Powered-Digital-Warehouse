// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class WebSocketServerInput : MonoBehaviour
// {
//     // Singleton instance
//     public static WebSocketServerInput Instance;

//     // WebSocket client reference
//     private WebSocketClient webSocketClient;

//     // WebSocket server IP address
//     public string IPAddress = "localhost"; // Default value

//     // WebSocket URL
//     public string WebSocketUrl { get; private set; } = "";

//     private void Awake()
//     {
//         // Implement singleton pattern
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject); // Persist across scene reloads
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     private void Start()
//     {
//         // Assign the WebSocket URL if it's already set
//         AssignWebSocketClient();
//     }

//     private void AssignWebSocketClient()
//     {
//         // Find the WebSocketClient component in the scene
//         webSocketClient = FindAnyObjectByType<WebSocketClient>();

//         if (webSocketClient != null)
//         {
//             // Disable the WebSocketClient script until the URL is updated
//             webSocketClient.enabled = false;

//             // Assign the URL to the WebSocket client if available
//             if (!string.IsNullOrEmpty(WebSocketUrl))
//             {
//                 webSocketClient._url = WebSocketUrl;
//                 webSocketClient.enabled = true; // Enable WebSocketClient after URL update
//                 Debug.Log("WebSocket URL assigned and WebSocketClient enabled: " + WebSocketUrl);
//             }
//             else
//             {
//                 Debug.LogWarning("WebSocket URL is empty. WebSocketClient remains disabled.");
//             }
//         }
//         else
//         {
//             Debug.LogWarning("WebSocketClient not found in the scene.");
//         }
//     }

//     public void ReadStringinput(string message)
//     {
//         // Update the IP Address and WebSocket URL
//         IPAddress = message.Trim(); // Ensure no trailing spaces
//         WebSocketUrl = "ws://" + IPAddress + ":8765/agv";

//         Debug.Log("WebSocket URL updated to: " + WebSocketUrl);

//         // Restart the scene
//         RestartScene();
//     }

//     private void RestartScene()
//     {
//         // Get the current active scene
//         Scene currentScene = SceneManager.GetActiveScene();

//         // Reload the current scene
//         SceneManager.LoadScene(currentScene.name);
//     }

//     private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//     {
//         // Reassign WebSocket client and update its status after the scene reloads
//         AssignWebSocketClient();
//     }

//     private void OnEnable()
//     {
//         SceneManager.sceneLoaded += OnSceneLoaded;
//     }

//     private void OnDisable()
//     {
//         SceneManager.sceneLoaded -= OnSceneLoaded;
//     }
// }
