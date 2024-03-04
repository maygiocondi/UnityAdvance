using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuildButton : MonoBehaviour
{
    [SerializeField] private Image image;
    private _FieldSlot slot;
    
    public void SetData(_FieldSlot slot)
    {
        this.slot = slot;
    }

    public void BuildTower()
    {
        EventRegister.Instance.buildTowerAction.Invoke("");
        slot.BuildTower(image.sprite);
    }
}
