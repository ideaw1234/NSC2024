using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Photon.Pun.Demo.PunBasics
{
    public class PlayerCamera : MonoBehaviour
    {
        private const float SMOROTH_TIME = 0.3f;
        public bool LockX, LockY, LockZ;
        public float offsetZ = -7f;
        public bool useSmoothing = true;
        private Transform camerathisTransform; //Camera
        private Vector3 velocity;
        void Awake()
        {
            camerathisTransform = Camera.main.transform;
            velocity = new Vector3(0.5f, 0.5f, 0.5f);
        }
        void Update()
        {
            var newPos = Vector3.zero;
            if (useSmoothing)
            {
                newPos.x = Mathf.SmoothDamp(camerathisTransform.position.x, this.transform.position.x, ref velocity.x, SMOROTH_TIME);
                newPos.y = Mathf.SmoothDamp(camerathisTransform.position.y, this.transform.position.y, ref velocity.y, SMOROTH_TIME);
                newPos.z = Mathf.SmoothDamp(camerathisTransform.position.z, this.transform.position.z + offsetZ, ref velocity.z, SMOROTH_TIME);
            }
            else
            {
                newPos.x = this.transform.position.x;
                newPos.y = this.transform.position.y;
                newPos.z = this.transform.position.z + offsetZ;
            }

            if (LockX)
            {
                newPos.x = camerathisTransform.transform.position.x;
            }
            if (LockY)
            {
                newPos.y = camerathisTransform.transform.position.y;
            }
            if (LockZ)
            {
                newPos.z = camerathisTransform.transform.position.z;
            }
            camerathisTransform.position = Vector3.Slerp(transform.position, newPos, Time.time);
        }
    }
}

