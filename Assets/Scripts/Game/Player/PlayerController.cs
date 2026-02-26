using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Game.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerController : MonoBehaviour
    {
        public enum InputMouseButton
        {
            Left,
            Right
        }

        [Header("Movement Settings")] [SerializeField]
        private LayerMask groundLayer;

        [SerializeField] private float stoppingDistance = 0.2f;

        [Header("Input Settings")] [SerializeField]
        private InputMouseButton moveButton = InputMouseButton.Right; // Default to Right

        private Camera mainCamera; // Cache camera reference

        private NavMeshAgent navMeshAgent;

        public bool IsMoving { get; private set; }

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            mainCamera = Camera.main; // Cache once instead of calling every frame
        }

        private void Start()
        {
            IsMoving = false;

            // Configure NavMeshAgent
            if (navMeshAgent != null) navMeshAgent.stoppingDistance = stoppingDistance;
        }

        private void Update()
        {
            HandleInput();
            UpdateMovementState();
        }

        public event Action<Vector3> OnMoveTargetSet;

        private void HandleInput()
        {
            if (Mouse.current == null || mainCamera == null) return;

            var wasPressed = false;
            if (moveButton == InputMouseButton.Left)
                wasPressed = Mouse.current.leftButton.wasPressedThisFrame;
            else
                wasPressed = Mouse.current.rightButton.wasPressedThisFrame;

            if (wasPressed)
            {
                var mousePos = Mouse.current.position.ReadValue();
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
                IsMoving = true;
            }
        }

        private void UpdateMovementState()
        {
            if (navMeshAgent != null && IsMoving)
                // Check if agent has reached destination
                if (!navMeshAgent.pathPending)
                    if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                        if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                            IsMoving = false;
        }

        public Vector3 GetDestination()
        {
            return navMeshAgent != null ? navMeshAgent.destination : transform.position;
        }
    }
}