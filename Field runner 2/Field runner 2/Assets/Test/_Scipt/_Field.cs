using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class _Field : MonoSingleton<_Field>
{
    //[SerializeField] private Vector2Int fieldSize;
    //[SerializeField] private Vector2Int origin;
    //[SerializeField] private Vector2Int start;
    //[SerializeField] private Vector2Int end;

    private Dictionary<Vector2Int, _FieldSlot> fieldSlots;
    private Dictionary<Vector2Int, _FieldSlot> emptySlots;

    private void Awake()
    {
        //CreateField();
        UpdateFieldSlot();
    }

    #region
    //private void CreateField()
    //{
    //    fieldSlots = new Dictionary<Vector2Int, _FieldSlot>();

    //    for (int x = origin.x; x < origin.x + fieldSize.x; x++)
    //    {
    //        for (int y = origin.y; y < origin.y + fieldSize.y; y++)
    //        {
    //            var slot = Instantiate(slotPrefab);
    //            fieldSlots.Add(new Vector2Int(x, y), slot);
    //            slot.SetPos(x, y);
    //        }
    //    }

    //    foreach (var kvp in fieldSlots)
    //    {
    //        List<Vector2Int> neighbors = new List<Vector2Int>();

    //        var up = kvp.Key + Vector2Int.up;
    //        if (fieldSlots.ContainsKey(up)) neighbors.Add(up);
    //        var right = kvp.Key + Vector2Int.right;
    //        if (fieldSlots.ContainsKey(right)) neighbors.Add(right);
    //        var down = kvp.Key + Vector2Int.down;
    //        if (fieldSlots.ContainsKey(down)) neighbors.Add(down);
    //        var left = kvp.Key + Vector2Int.left;
    //        if (fieldSlots.ContainsKey(left)) neighbors.Add(left);

    //        kvp.Value.SetNeighbors(neighbors);
    //    }

    //    UpdateSlot();
    //}
    #endregion

    //[ContextMenu("Update Field Slot")]
    //public void UpdateFieldSlot()
    //{
    //    var allFieldSlots = gameObject.GetComponentsInChildren<_FieldSlot>();
    //    fieldSlots = allFieldSlots.ToDictionary(s => new Vector2Int(Mathf.RoundToInt(s.transform.position.x), Mathf.RoundToInt(s.transform.position.y)), s => s);
    //    // Đoạn này thêm vào để xoá những slot bị trùng nhau khi tạo
    //    foreach(var slot in allFieldSlots)
    //    {
    //        slot.gameObject.name = $"Slot {Mathf.RoundToInt(slot.transform.position.x)}, {Mathf.RoundToInt(slot.transform.position.y)}";
    //    }
    //    UpdateSlot();
    //}
    
    [ContextMenu("Update Field Slot")]
    public void UpdateFieldSlot()
    {
        var allFieldSlots = gameObject.GetComponentsInChildren<_FieldSlot>();
        fieldSlots = new Dictionary<Vector2Int, _FieldSlot>();
        foreach(var slot in allFieldSlots)
        {
            var v2Pos = new Vector2Int(Mathf.RoundToInt(slot.transform.position.x), Mathf.RoundToInt(slot.transform.position.y));

            if (fieldSlots.ContainsKey(v2Pos))
            {
                DestroyImmediate(slot.gameObject);
                continue;
            }
            else
            {
                fieldSlots.Add(v2Pos, slot);
                slot.SetPos(v2Pos.x, v2Pos.y);
            }

            slot.gameObject.name = $"Slot {Mathf.RoundToInt(slot.transform.position.x)},{Mathf.RoundToInt(slot.transform.position.y)}";
        }
        UpdateSlot();
    }

    public void UpdateSlot()
    {
        emptySlots = fieldSlots.Where(s => !s.Value.IsOccupied).ToDictionary(s => s.Key, s => s.Value);


        foreach (var kvp in fieldSlots)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>();

            var up = kvp.Key + Vector2Int.up;
            if (emptySlots.ContainsKey(up)) neighbors.Add(up);
            var right = kvp.Key + Vector2Int.right;
            if (emptySlots.ContainsKey(right)) neighbors.Add(right);
            var down = kvp.Key + Vector2Int.down;
            if (emptySlots.ContainsKey(down)) neighbors.Add(down);
            var left = kvp.Key + Vector2Int.left;
            if (emptySlots.ContainsKey(left)) neighbors.Add(left);

            kvp.Value.SetNeighbors(neighbors);
        }
    }

    [ContextMenu("Find Path")]
    public void FindPath()
    {
        //var paths = BFSearch(start, end);
    }

    public List<_FieldSlot> BFSearch(Vector2Int start, Vector2Int end)
    {
        if (!emptySlots.ContainsKey(start) || !emptySlots.ContainsKey(end)) throw new System.Exception("No slot in field");
        if (emptySlots[start].IsOccupied || emptySlots[end].IsOccupied) throw new System.Exception("Slot is occupied");

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        List<Vector2Int> visited = new List<Vector2Int>();
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

            foreach (var neighbor in fieldSlots[current].Neighbors)
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

    private List<_FieldSlot> ReconstructedPath(Vector2Int start, Vector2Int end, Dictionary<Vector2Int, Vector2Int> cameFroms)
    {
        List<Vector2Int> paths = new List<Vector2Int>();

        var current = end;
        while (current != start)
        {
            paths.Add(current);
            current = cameFroms[current];
        }

        paths.Add(start);
        paths.Reverse();

        List<_FieldSlot> slotPath = new List<_FieldSlot>();
        foreach (var p in paths)
        {
            slotPath.Add(emptySlots[p]);
        }


        return slotPath;
    }


    private void OnDrawGizmos()
    {
        GUI.contentColor = Color.black;
        GUIStyle style = new GUIStyle();
        style.fontSize = 16;
        style.fontStyle = FontStyle.Bold;

        if (fieldSlots != null)
        {
            foreach (var kvp in fieldSlots)
            {
                if (!kvp.Value.IsOccupied) Handles.Label((Vector2)kvp.Key, $"{kvp.Key}");
            }
        }
    }
}
