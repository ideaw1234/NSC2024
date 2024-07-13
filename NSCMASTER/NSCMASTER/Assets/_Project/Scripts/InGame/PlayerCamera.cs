//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//namespace Photon.Pun.Demo.PunBasics
//{
//    public class PlayerCamera : MonoBehaviour
//    {
//        private const float SMOROTH_TIME = 0.3f;
//        public bool LockX, LockY, LockZ;
//        public float offsetZ = -7f;
//        public bool useSmoothing = true;
//        private Transform camerathisTransform; //Camera
//        private Vector3 velocity;
//        void Awake()
//        {
//            camerathisTransform = Camera.main.transform;
//            velocity = new Vector3(0.5f, 0.5f, 0.5f);
//        }
//        void Update()
//        {
//            var newPos = Vector3.zero;
//            if (useSmoothing)
//            {
//                newPos.x = Mathf.SmoothDamp(camerathisTransform.position.x, this.transform.position.x, ref velocity.x, SMOROTH_TIME);
//                newPos.y = Mathf.SmoothDamp(camerathisTransform.position.y, this.transform.position.y, ref velocity.y, SMOROTH_TIME);
//                newPos.z = Mathf.SmoothDamp(camerathisTransform.position.z, this.transform.position.z + offsetZ, ref velocity.z, SMOROTH_TIME);
//            }
//            else
//            {
//                newPos.x = this.transform.position.x;
//                newPos.y = this.transform.position.y;
//                newPos.z = this.transform.position.z + offsetZ;
//            }

//            if (LockX)
//            {
//                newPos.x = camerathisTransform.transform.position.x;
//            }
//            if (LockY)
//            {
//                newPos.y = camerathisTransform.transform.position.y;
//            }
//            if (LockZ)
//            {
//                newPos.z = camerathisTransform.transform.position.z;
//            }
//            camerathisTransform.position = Vector3.Slerp(transform.position, newPos, Time.time);
//        }
//    }
//}
using UnityEngine;
using System.Collections;
using Photon.Pun;

namespace Photon.Pun.Demo.PunBasics
{
    public class PlayerCamera : MonoBehaviourPunCallbacks
    {
        public float offsetZ = -7f;
        public float offsetY = 5f; // New: Vertical offset below the player
        public float smoothTime = 0.3f;

        private Transform cameraTransform;
        private Vector3 currentVelocity;
        private Transform currentTarget;

        void Awake()
        {
            cameraTransform = Camera.main.transform;
            currentVelocity = Vector3.zero;
        }

        void Start()
        {
            if (PlayerManager.LocalPlayerInstance != null)
            {
                SetTarget(PlayerManager.LocalPlayerInstance.transform);
            }
            else
            {
                Debug.LogWarning("PlayerCamera: Local player instance not found!");
            }
        }

        void LateUpdate()
        {
            if (currentTarget == null)
                return;

            Vector3 targetPosition = currentTarget.position + new Vector3(0, offsetY, offsetZ);
            Vector3 newPos = Vector3.SmoothDamp(cameraTransform.position, targetPosition, ref currentVelocity, smoothTime);

            cameraTransform.position = newPos;

            // Make the camera look at a point slightly above the player
            Vector3 lookAtPosition = currentTarget.position + Vector3.up * 1f;
            cameraTransform.LookAt(lookAtPosition);
        }

        public void SetTarget(Transform newTarget)
        {
            if (newTarget == currentTarget)
                return;

            StopAllCoroutines();
            StartCoroutine(SmoothTargetTransition(newTarget));
        }

        private IEnumerator SmoothTargetTransition(Transform newTarget)
        {
            float transitionTime = 1f;
            float elapsedTime = 0f;
            Vector3 startPosition = cameraTransform.position;
            Quaternion startRotation = cameraTransform.rotation;

            while (elapsedTime < transitionTime)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / transitionTime;

                Vector3 targetPosition = newTarget.position + new Vector3(0, offsetY, offsetZ);
                cameraTransform.position = Vector3.Lerp(startPosition, targetPosition, t);

                Vector3 lookAtPosition = newTarget.position + Vector3.up * 1f;
                Quaternion targetRotation = Quaternion.LookRotation(lookAtPosition - cameraTransform.position);
                cameraTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

                yield return null;
            }

            currentTarget = newTarget;
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            if (newPlayer.IsLocal && PlayerManager.LocalPlayerInstance != null)
            {
                SetTarget(PlayerManager.LocalPlayerInstance.transform);
            }
        }
    }
}
