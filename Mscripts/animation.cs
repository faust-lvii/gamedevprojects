using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinIglesias
{
    public class AnimationStateChanger : MonoBehaviour
    {
        private Animator animator;
        private Vector3 lastPosition;
        private bool isGrounded;
        private Rigidbody rb;

        // Animation parameter names
        private readonly string IS_WALKING = "IsWalking";
        private readonly string IS_RUNNING = "IsRunning";
        private readonly string IS_JUMPING = "IsJumping";
        private readonly string MOVEMENT_SPEED = "MovementSpeed";

        [Header("Movement Settings")]
        [SerializeField] private float walkSpeedThreshold = 0.1f;
        [SerializeField] private float runSpeedThreshold = 4f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckDistance = 0.2f;

        void Start()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            lastPosition = transform.position;

            // Make sure these parameters exist in your Animator Controller
            Debug.Log("Remember to set up these parameters in your Animator Controller:");
            Debug.Log("- " + IS_WALKING + " (bool)");
            Debug.Log("- " + IS_RUNNING + " (bool)");
            Debug.Log("- " + IS_JUMPING + " (bool)");
            Debug.Log("- " + MOVEMENT_SPEED + " (float)");
        }

        void Update()
        {
            if (animator == null) return;

            // Calculate movement
            Vector3 currentPosition = transform.position;
            Vector3 movement = currentPosition - lastPosition;
            float speed = movement.magnitude / Time.deltaTime;

            // Ground check
            isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

            // Update animation parameters
            UpdateMovementAnimation(speed);
            UpdateJumpingAnimation();

            lastPosition = currentPosition;
        }

        private void UpdateMovementAnimation(float speed)
        {
            // Update movement speed parameter
            animator.SetFloat(MOVEMENT_SPEED, speed);

            // Set walking and running states
            bool isWalking = speed > walkSpeedThreshold && speed <= runSpeedThreshold;
            bool isRunning = speed > runSpeedThreshold;

            animator.SetBool(IS_WALKING, isWalking);
            animator.SetBool(IS_RUNNING, isRunning);
        }

        private void UpdateJumpingAnimation()
        {
            // Check if character is jumping
            bool isJumping = !isGrounded;
            animator.SetBool(IS_JUMPING, isJumping);
        }

        // Bu fonksiyonu inspector'dan test etmek için kullanabilirsiniz
        public void TriggerCustomAnimation(string triggerName)
        {
            if (animator != null)
            {
                animator.SetTrigger(triggerName);
            }
        }

        // Debug için görsel yardımcılar
        private void OnDrawGizmos()
        {
            // Ground check ray'ini görselleştir
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
        }
    }
}
