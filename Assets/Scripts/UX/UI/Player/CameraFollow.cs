using UnityEngine;

namespace UX.UI.Player
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Target")] [SerializeField] private Transform target;

        [Header("Settings")] [SerializeField] private Vector3 offset = new(0, 10, -10);

        [SerializeField] private bool lookAtTarget = true;
        [SerializeField] private float smoothTime = 0.1f;
        private Vector3 lastTargetPosition;

        private Vector3 velocity = Vector3.zero;

        private void Start()
        {
            if (target != null)
            {
                lastTargetPosition = target.position;
                transform.position = target.position + offset;

                if (lookAtTarget) transform.LookAt(target);
            }
        }

        private void LateUpdate()
        {
            if (target == null)
                return;

            var targetPos = target.position;
            var desiredPosition = targetPos + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

            if (lookAtTarget)
                if (targetPos != lastTargetPosition)
                {
                    transform.LookAt(target);
                    lastTargetPosition = targetPos;
                }
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            if (newTarget != null) lastTargetPosition = newTarget.position;
        }
    }
}