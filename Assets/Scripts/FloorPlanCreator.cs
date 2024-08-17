using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class NodeData
{
    public Vector2Int position;
    public List<Vector2Int> connections = new List<Vector2Int>();
}

public class GridManager : MonoBehaviour
{
    public GameObject nodePrefab;   // Assign your node prefab in Unity
    public GameObject connectionPrefab; // Assign your connection prefab in Unity

    private Dictionary<Vector2Int, GameObject> nodes = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "grid_data.csv");
        LoadGridFromCsv(filePath);
    }

    void LoadGridFromCsv(string filePath)
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            Dictionary<Vector2Int, NodeData> gridData = new Dictionary<Vector2Int, NodeData>();

            // Skip the header line
            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');
                Vector2Int nodePosition = StringToVector2Int(values[0]);
                NodeData nodeData = new NodeData
                {
                    position = nodePosition
                };

                if (values.Length > 1 && !string.IsNullOrWhiteSpace(values[1]))
                {
                    string[] connections = values[1].Split(')');
                    foreach (string conn in connections)
                    {
                        if (!string.IsNullOrWhiteSpace(conn))
                        {
                            nodeData.connections.Add(StringToVector2Int(conn + ")"));
                        }
                    }
                }

                gridData[nodePosition] = nodeData;
            }

            // Instantiate nodes and connections
            foreach (var entry in gridData)
            {
                Vector2Int nodePosition = entry.Key;
                NodeData nodeData = entry.Value;

                GameObject node = Instantiate(nodePrefab, new Vector3(nodePosition.x, 0, nodePosition.y), Quaternion.identity);
                nodes[nodePosition] = node;

                foreach (var connection in nodeData.connections)
                {
                    if (nodes.ContainsKey(connection))
                    {
                        GameObject conn = Instantiate(connectionPrefab);
                        LineRenderer lineRenderer = conn.GetComponent<LineRenderer>();
                        lineRenderer.SetPositions(new Vector3[] { new Vector3(nodePosition.x, 0, nodePosition.y), new Vector3(connection.x, 0, connection.y) });
                    }
                }
            }
        }
    }

    Vector2Int StringToVector2Int(string s)
    {
        s = s.Trim(new char[] { '(', ')' });
        string[] parts = s.Split(',');
        return new Vector2Int(int.Parse(parts[0]), int.Parse(parts[1]));
    }
}
