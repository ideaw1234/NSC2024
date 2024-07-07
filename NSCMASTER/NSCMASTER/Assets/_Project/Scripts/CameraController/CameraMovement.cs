using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private const float SMOROTH_TIME = 0.3f;
    public bool LockX, LockY, LockZ;
    public float offsetZ = -7f;
    public bool useSmoothing = true;
    public Transform target; //Player
    private Transform thisTransform; //Camera
    private Vector3 velocity;
    void Awake()
    {
        thisTransform = transform;
        velocity = new Vector3(0.5f, 0.5f, 0.5f);
    }
    private void Update()
    {
        var newPos = Vector3.zero;
        if (useSmoothing)
        {
            newPos.x = Mathf.SmoothDamp(thisTransform.position.x, target.position.x, ref velocity.x, SMOROTH_TIME);
            newPos.y = Mathf.SmoothDamp(thisTransform.position.y, target.position.y, ref velocity.y, SMOROTH_TIME);
            newPos.z = Mathf.SmoothDamp(thisTransform.position.z, target.position.z + offsetZ, ref velocity.z, SMOROTH_TIME);
        }
        else
        {
            newPos.x = target.position.x;
            newPos.y = target.position.y;
            newPos.z = target.position.z + offsetZ;
        }

        if (LockX)
        {
            newPos.x = thisTransform.position.x;
        }
        if (LockY)
        {
            newPos.y = thisTransform.position.y;
        }
        if (LockZ)
        {
            newPos.z = thisTransform.position.z;
        }
        transform.position = Vector3.Slerp(thisTransform.position, newPos, Time.time);
    }
}
