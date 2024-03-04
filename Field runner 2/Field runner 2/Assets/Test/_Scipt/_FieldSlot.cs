using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class _FieldSlot : MonoBehaviour, IPointerClickHandler
{
    private Vector2Int pos;
    private List<Vector2Int> neighbors;

    [SerializeField] private bool isOccupied;
    [SerializeField] private SpriteRenderer towerSprite;


    public List<Vector2Int> Neighbors => neighbors;
    public Vector2Int Pos => pos;
    public bool IsOccupied => isOccupied;

    public void SetPos(int x, int y)
    {
        this.pos = new Vector2Int(x, y);
        transform.position = (Vector2)pos;
    }

    public void SetNeighbors(List<Vector2Int> neighbors)
    {
        this.neighbors = neighbors;
    }

    public void SetOccupied(bool isOccupied)
    {
        this.isOccupied = isOccupied;
    }

    public void BuildTower(Sprite towerSprite)
    {
        isOccupied = true;
        this.towerSprite.sprite = towerSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIController.Instance.OpenBuildSheetPopup(this);
    }
}
