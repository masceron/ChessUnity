using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private LayerMask groundLayer;

        private Vector3 targetPosition;
        private bool isMoving;
        [SerializeField] private float minMoveDistance = 0.2f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float gravityMultiplier = 2.0f;
        private Vector3 verticalVelocity;

        private CharacterController characterController;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            targetPosition = transform.position;
            isMoving = false;
        }

        private void Update()
        {
            HandleInput();
            Move();
        }

        public event System.Action<Vector3> OnMoveTargetSet; 

        public enum InputMouseButton { Left, Right }

        [Header("Input Settings")]
        [SerializeField] private InputMouseButton moveButton = InputMouseButton.Right; // Default to Right

        private void HandleInput()
        {
            if (UnityEngine.InputSystem.Mouse.current == null) return;

            var wasPressed = false;
            if (moveButton == InputMouseButton.Left)
            {
                wasPressed = UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame;
            }
            else
            {
                wasPressed = UnityEngine.InputSystem.Mouse.current.rightButton.wasPressedThisFrame;
            }

            if (wasPressed)
            {
                var mousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
                var ray = Camera.main.ScreenPointToRay(mousePos);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    targetPosition = hit.point;
                    isMoving = true;
                    
                    OnMoveTargetSet?.Invoke(hit.point);
                }
            }
        }

        private void Move()
        {
            var currentPos = transform.position;
            var targetPos = targetPosition;
            var currentPosFlat = new Vector3(currentPos.x, 0, currentPos.z);
            var targetPosFlat = new Vector3(targetPos.x, 0, targetPos.z);
            
            var distance = Vector3.Distance(currentPosFlat, targetPosFlat);
            var horizontalMove = Vector3.zero;

            if (isMoving && distance > minMoveDistance)
            {
                var direction = (targetPosFlat - currentPosFlat).normalized;
                horizontalMove = direction * moveSpeed;

                if (direction != Vector3.zero)
                {
                    var lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                isMoving = false;
            }

            if (characterController.isGrounded)
            {
                verticalVelocity.y = -2f; 
            }
            else
            {
                verticalVelocity.y += gravity * gravityMultiplier * Time.deltaTime;
            }

            var finalMove = (horizontalMove + verticalVelocity) * Time.deltaTime;
            characterController.Move(finalMove);
        }
    }
}
