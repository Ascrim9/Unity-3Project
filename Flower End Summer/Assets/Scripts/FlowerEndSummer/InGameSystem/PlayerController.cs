using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FlowerEndSummer
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] float rotationSpeed = 500f;

        [Header("Ground Check Settings")] [SerializeField]
        float groundCheckRadius = 0.2f;

        [SerializeField] Vector3 groundCheckOffset;
        [SerializeField] LayerMask groundLayer;
        
        
        bool isGrounded;
        private bool isControl = true;
        
        
        
        float ySpeed;
        Quaternion targetRotation;
        PlayerCamera cameraController;
        Animator animator;
        CharacterController characterController;

        private static readonly int MoveY = Animator.StringToHash("MoveY");
        private static readonly int MoveX = Animator.StringToHash("MoveX");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            characterController = GetComponent<CharacterController>();

            cameraController = PlayerCamera.Instance;
        }


        private void Update()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

            var moveInput = (new Vector3(h, 0, v)).normalized;

            var moveDir = cameraController.PlanarRotation * moveInput;

            if (isControl is true)
            {
                if (characterController.isGrounded)
                {
                    ySpeed = -0.5f;
                }
                else
                {
                    ySpeed += Physics.gravity.y * Time.deltaTime;
                }

                var velocity = moveDir * moveSpeed;
                velocity.y = ySpeed;

                characterController.Move(velocity * Time.deltaTime);

                if (moveAmount > 0)
                {
                    targetRotation = Quaternion.LookRotation(moveDir);
                }

                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                    rotationSpeed * Time.deltaTime);

                animator.SetFloat(MoveX, moveAmount, 0.2f, Time.deltaTime);
            }
            

        }

        public void SetControl(bool isControl)
        {
            this.isControl = isControl;
            characterController.enabled = isControl;

            if (isControl is false)
            {
                // animator.SetFloat(MoveY, 0.0f);
                animator.SetFloat(MoveX, 0.0f);
                targetRotation = transform.rotation;
            }
        }
        
    }
}