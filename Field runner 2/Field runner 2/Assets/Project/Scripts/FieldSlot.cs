using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSlot : MonoBehaviour
{
    public Vector2Int pos;
    public Vector2Int Pos => pos;


    private List<Vector2Int> neighbors;
    public List<Vector2Int> Neighbors => neighbors;

    public void SetPos(int x, int y)
    {
        this.pos = new Vector2Int(x, y);
        transform.position = (Vector2)pos;
    }

    public void SetNeighbors(List<Vector2Int> neighbors)
    {
        this.neighbors = neighbors;
    }

}
