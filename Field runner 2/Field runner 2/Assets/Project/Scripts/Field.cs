using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Field : MonoBehaviour
{
    private List<GameObject> temps = new List<GameObject>();
 

    [SerializeField] private GameObject tempPrefab;
    [SerializeField] private Vector2Int start;
    [SerializeField] private Vector2Int end;
    [SerializeField] FieldSlot slotPrefabs;

    [SerializeField] Vector2Int fieldSize;
    [SerializeField] Vector2Int origin;

    private Dictionary<Vector2Int, FieldSlot> fieldSlots;
    private Dictionary<Vector2Int, FieldSlot> emptySlots;

    private void Awake()
    {
        CreateField();
    }

    private void CreateField()
    {
        fieldSlots = new Dictionary<Vector2Int, FieldSlot>();

        for (int i = origin.x; i <= fieldSize.x + origin.x; i++)
        {
            for (int j = origin.y; j <= fieldSize.y + origin.y; j++)
            {
                var slot = Instantiate(slotPrefabs);
                slot.SetPos(i, j);
                fieldSlots.Add(new Vector2Int(i, j), slot);
            }
        }

        foreach (var kvp in fieldSlots)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>();

            var up = kvp.Key + Vector2Int.up;
            if (fieldSlots.ContainsKey(up)) neighbors.Add(up);
            var right = kvp.Key + Vector2Int.right;
            if (fieldSlots.ContainsKey(right)) neighbors.Add(right);
            var down = kvp.Key + Vector2Int.down;
            if (fieldSlots.ContainsKey(down)) neighbors.Add(down);
            var left = kvp.Key + Vector2Int.left;
            if (fieldSlots.ContainsKey(left)) neighbors.Add(left);

            kvp.Value.SetNeighbors(neighbors);
        }


    }

    [ContextMenu("Find path")]
    private void FindPath()
    {
        var paths = BFSearch(start, end);


        for (int i = 0; i < temps.Count; i++)
        {
            Destroy(temps[i]);
        }

        temps.Clear();

        if (paths != null)
        {
            foreach (var node in paths)
            {
                var obj = Instantiate(tempPrefab);
                obj.transform.position = (Vector2)node;
                temps.Add(obj);
            }
        }
    }

    private List<Vector2Int> BFSearch(Vector2Int start, Vector2Int end)
    {
        if (!fieldSlots.ContainsKey(start) || !fieldSlots.ContainsKey(end)) throw new System.Exception("No slot in field");
        List<Vector2Int> visited = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFroms = new Dictionary<Vector2Int, Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current == end)
            {
                return ReconstructedPath(start, end, cameFroms);
            }

            foreach (Vector2Int neighbor in fieldSlots[current].Neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                    cameFroms[neighbor] = current;
                }
            }
        }

        return null;
    }


    private List<Vector2Int> ReconstructedPath(Vector2Int start, Vector2Int end, Dictionary<Vector2Int, Vector2Int> cameFroms)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = end;

        while (current != start)
        {
            path.Add(current);
            current = cameFroms[current];
        }

        path.Add(start);
        path.Reverse();

        Debug.Log(path);

        return path;
    }

    private void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 22;
        style.fontStyle = FontStyle.Bold;

        if (fieldSlots != null)
        {
            foreach (var kvp in fieldSlots)
            {
                Handles.Label((Vector2)kvp.Key, $"{kvp.Key}");
            }
        }
    }
}
