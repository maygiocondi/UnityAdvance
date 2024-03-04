using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class _CameraController : MonoBehaviour
{
    public float panSpeed = 2f;
    public float smoothSpeed = 3f;

    private Vector2 panLimit = new Vector2(10f, 10f);

    private Vector3 targetPos;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
    }

    private void OnEnable()
    {
        LeanTouch.OnGesture += HandleFingerUpdate;

        targetPos = transform.position;
    }

    private void OnDisable()
    {
        LeanTouch.OnGesture -= HandleFingerUpdate;
    }

    private void HandleFingerUpdate(List<LeanFinger> fingers)
    {
        Vector2 delta = fingers[0].ScreenDelta;

        targetPos += new Vector3(-delta.x * panSpeed * Time.deltaTime, -delta.y * panSpeed * Time.deltaTime, 0);

        var claim = new Vector3(0, 0, transform.position.z);
        claim.x = Mathf.Clamp(targetPos.x, -panLimit.x, panLimit.x);
        claim.y = Mathf.Clamp(targetPos.y, -panLimit.y, panLimit.y);

        targetPos = claim;

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
