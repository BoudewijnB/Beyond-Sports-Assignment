using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime;
    private Vector3 offset;
    private Vector3 currentVelocity = Vector3.zero;

    private void Awake()
    {
        // Set camera offset
        offset = transform.position - target.position;
    }

    // LateUpdate for Camera in case objects that it follow move in update
    private void LateUpdate()
    {
        // Set target position and follow the target with smoothing
        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref currentVelocity, smoothTime);
    }
}
