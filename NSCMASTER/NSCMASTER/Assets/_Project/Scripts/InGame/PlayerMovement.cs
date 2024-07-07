using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Photon.Pun.Demo.PunBasics
{
    public class PlayerMovement : MonoBehaviourPun
    {
        [Header("UI Elements")]
        [SerializeField] GameObject QuizPanel;
        Animator animator;
        CharacterController characterController;
        [Header("Player Movement")]
        [SerializeField]
        float rotationSpeed = 30f;
        public float speed = 6.0f;
        public float gravity = 20.0f;
        Vector3 inputVec;
        Vector3 tragetDirection;
        private float turner,locker;
        public float sensitivity = 2.0f;
        private Vector3 moveDirection = Vector3.zero;

        public bool isItemDetect = false;
        EngQuiz EngQuiz;
        #region MonoBehaviour Callbacks

        private void Start()
        {
            Time.timeScale = 1;
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();
            if (EngQuiz == null)
            {
                GameObject _TempGameController = GameObject.FindGameObjectWithTag("EngQuiz") as GameObject;
                EngQuiz = _TempGameController.GetComponent<EngQuiz>();
            }
        }

        private void Update()
        {
            //if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            //{
            //    return;
            //}
            if (!animator)
            {
                return;
            }
            float x = -(Input.GetAxisRaw("Vertical"));
            float z = Input.GetAxisRaw("Horizontal");
            inputVec = new Vector3(x, 0, z);

            animator.SetFloat("Input_X", z);
            animator.SetFloat("Input_Z", -(x));

            if (x != 0 || z != 0)
            {
                animator.SetBool("Moving", true);
                animator.SetBool("Running", true);
            }
            else
            {
                animator.SetBool("Moving", false);
                animator.SetBool("Running", false);
            }
            if(characterController.isGrounded)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDirection *= speed;
            }

            //turner = Input.GetAxis("Mouse X") * sensitivity;
            //if (turner != 0)
            //{
            //    transform.eulerAngles += new Vector3(0, turner, 0);
            //}
            characterController.Move(moveDirection * Time.deltaTime);
            UpdateMovement();

        }
        #endregion

        void UpdateMovement()
        {
            Vector3 motion = inputVec;
            motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1) ? .7f : 1;
            RotateTowardMovementDirection();
            GetCameraRelative();
        }

        void RotateTowardMovementDirection()
        {
            if (inputVec != Vector3.zero)
            {
                // สร้าง rotation ใหม่โดยไม่คำนึงถึงการเคลื่อนที่ถอยหลัง
                Vector3 lookDirection = new Vector3(tragetDirection.x, 0, tragetDirection.z).normalized;
                if (lookDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                }
            }
        }

        void GetCameraRelative()
        {
            Transform cameraTransform = Camera.main.transform;
            Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
            forward.y = 0;
            forward = forward.normalized;

            Vector3 right = new Vector3(forward.z, 0, -forward.x);
            float v = Input.GetAxisRaw("Vertical");
            float h = Input.GetAxisRaw("Horizontal");
            tragetDirection = (h * right) + (v * forward);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Item")
            {
                isItemDetect = true;
                Debug.Log("Item Detected");
                EngQuiz.ShowQuizPanel(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            isItemDetect = false;
            Debug.Log("Item Not Detected");
        }
    }

}
