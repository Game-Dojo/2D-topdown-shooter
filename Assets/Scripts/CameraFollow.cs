using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float targetSpeed = 5.0f;
    
    void LateUpdate()
    {
        if (!target) return;
        Vector3 targetPos = target.position;
        targetPos.z = transform.position.z;
        
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * targetSpeed);
    }
}
