using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoSingleton<UIController>
{
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private BuildSheet buildSheet;

    public void OpenBuildSheetPopup(_FieldSlot slot)
    {
        buildSheet.gameObject.SetActive(true);
        buildSheet.SetSlot(slot);

        Camera cam = Camera.main;
        Vector3 screenPos = cam.WorldToScreenPoint(slot.transform.position);

        Vector2 canvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, cam, out canvasPos);
        buildSheet.GetComponent<RectTransform>().anchoredPosition = canvasPos;
    }
}
