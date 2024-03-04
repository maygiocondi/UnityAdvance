using UnityEngine;
using Lean.Touch;
using System.Collections.Generic;
using TMPro;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 0.5f;
    public float smoothSpeed = 5f;
    public Vector2 panLimit = new Vector2(10f, 10f);

    private Vector3 targetPos;

    void OnEnable()
    {
        LeanTouch.OnGesture += HandleFingerUpdate;

        targetPos = transform.position;
    }

    void OnDisable()
    {
        LeanTouch.OnGesture -= HandleFingerUpdate;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
    }

    void HandleFingerUpdate(List<LeanFinger> fingers)
    {
        if (fingers.Count > 0)
        {
            Vector2 delta = fingers[0].ScreenDelta;

            targetPos += new Vector3(-delta.x * panSpeed * Time.deltaTime, -delta.y * panSpeed * Time.deltaTime, 0);
            var clamped = new Vector3(0, 0, transform.position.z);
            clamped.x = Mathf.Clamp(targetPos.x, -panLimit.x, panLimit.x);
            clamped.y = Mathf.Clamp(targetPos.y, -panLimit.y, panLimit.y);
            targetPos = clamped;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(new Vector2(-panLimit.x, -panLimit.y), new Vector2(panLimit.x, -panLimit.y));
        Gizmos.DrawLine(new Vector3(-panLimit.x, panLimit.y), new Vector3(panLimit.x, panLimit.y));

        Gizmos.DrawLine(new Vector3(-panLimit.x, -panLimit.y), new Vector3(-panLimit.x, panLimit.y));
        Gizmos.DrawLine(new Vector3(panLimit.x, -panLimit.y), new Vector3(panLimit.x, panLimit.y));
    }
}