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

        private void LateUpdate()
        {
            if (target == null)
                return;

            var desiredPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

            if (lookAtTarget)
            {
                transform.LookAt(target);
            }
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
}
