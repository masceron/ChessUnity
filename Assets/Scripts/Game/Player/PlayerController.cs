using UnityEngine;
using UnityEngine.AI;

namespace Game.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float stoppingDistance = 0.2f;

        private NavMeshAgent navMeshAgent;
        private bool isMoving;
        private Camera mainCamera; // Cache camera reference

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            mainCamera = Camera.main; // Cache once instead of calling every frame
        }

        private void Start()
        {
            isMoving = false;
            
            // Configure NavMeshAgent
            if (navMeshAgent != null)
            {
                navMeshAgent.stoppingDistance = stoppingDistance;
            }
        }

        private void Update()
        {
            HandleInput();
            UpdateMovementState();
        }

        public event System.Action<Vector3> OnMoveTargetSet; 

        public enum InputMouseButton { Left, Right }

        [Header("Input Settings")]
        [SerializeField] private InputMouseButton moveButton = InputMouseButton.Right; // Default to Right

        private void HandleInput()
        {
            if (UnityEngine.InputSystem.Mouse.current == null || mainCamera == null) return;

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
                var ray = mainCamera.ScreenPointToRay(mousePos);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    SetDestination(hit.point);
                    OnMoveTargetSet?.Invoke(hit.point);
                }
            }
        }

        private void SetDestination(Vector3 destination)
        {
            if (navMeshAgent != null && navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.SetDestination(destination);
                isMoving = true;
            }
        }

        private void UpdateMovementState()
        {
            if (navMeshAgent != null && isMoving)
            {
                // Check if agent has reached destination
                if (!navMeshAgent.pathPending)
                {
                    if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                    {
                        if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                        {
                            isMoving = false;
                        }
                    }
                }
            }
        }

        public bool IsMoving => isMoving;
        
        public Vector3 GetDestination()
        {
            return navMeshAgent != null ? navMeshAgent.destination : transform.position;
        }
    }
}
