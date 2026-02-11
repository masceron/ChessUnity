using UnityEngine;

namespace Game.Player
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;

        [Header("Settings")]
        [SerializeField] private Vector3 offset = new Vector3(0, 10, -10); 
        [SerializeField] private bool lookAtTarget = true;

        private Vector3 velocity = Vector3.zero;
        [SerializeField] private float smoothTime = 0.1f;
        private Vector3 lastTargetPosition; // Track target movement
        private Transform targetTransform; // Cache target transform 

        private void LateUpdate()
        {
            if (target == null)
                return;

            // Cache target position to avoid multiple property accesses (reduces allocations)
            var targetPos = target.position;
            var desiredPosition = targetPos + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

            if (lookAtTarget)
            {
                // Only call LookAt if target has moved to avoid unnecessary calculations
                if (targetPos != lastTargetPosition)
                {
                    transform.LookAt(target);
                    lastTargetPosition = targetPos;
                }
            }
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            if (newTarget != null)
            {
                lastTargetPosition = newTarget.position;
            }
        }
    }
}
