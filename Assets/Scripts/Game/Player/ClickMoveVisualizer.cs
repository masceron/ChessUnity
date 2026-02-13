using UnityEngine;

namespace Game.Player
{
    public class ClickMoveVisualizer : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private GameObject clickEffectPrefab;
        [SerializeField] private float hideDistance = 0.05f;

        private GameObject currentEffectInstance;
        private Transform playerTransform; // Cache player transform
        private Transform effectTransform; // Cache effect transform
        private float hideDistanceSqr; // Store squared distance to avoid sqrt
        private Vector3 offsetUp; // Cache Vector3.up * 0.1f to avoid allocation
        private Vector3 lastDestination; // Cache last destination to avoid repeated GetDestination calls

        private void OnEnable()
        {
            if (playerController != null)
            {
                playerController.OnMoveTargetSet += ShowEffect;
                playerTransform = playerController.transform; // Cache on enable
            }
            
            hideDistanceSqr = 1f + hideDistance * hideDistance; // Pre-calculate squared distance
            offsetUp = Vector3.up * 0.1f; // Cache offset to avoid allocation each frame
        }

        private void OnDisable()
        {
            if (playerController != null)
            {
                playerController.OnMoveTargetSet -= ShowEffect;
            }
        }

        private void Update()
        {
            if (effectTransform != null && currentEffectInstance.activeSelf && playerTransform != null)
            {
                // Update position to follow the destination (in case path changes)
                lastDestination = playerController.GetDestination();
                if (lastDestination != Vector3.zero)
                {
                    effectTransform.position = lastDestination + offsetUp; // Use cached offset - no allocation
                }
                
                // Use sqrMagnitude to avoid expensive sqrt calculation
                var distanceSqr = (playerTransform.position - effectTransform.position).sqrMagnitude;
                
                if (distanceSqr < hideDistanceSqr)
                {
                    currentEffectInstance.SetActive(false);
                }
            }
        }

        private void ShowEffect(Vector3 position)
        {
            if (clickEffectPrefab == null) return;

            var spawnPos = position + offsetUp; // Use cached offset - no allocation

            if (currentEffectInstance == null)
            {
                currentEffectInstance = Instantiate(clickEffectPrefab, spawnPos, Quaternion.Euler(90, 0, 0));
                effectTransform = currentEffectInstance.transform; // Cache transform on creation
            }
            else
            {
                effectTransform.position = spawnPos;
                currentEffectInstance.SetActive(false); // Reset animation/particles
            }

            currentEffectInstance.SetActive(true);
        }
    }
}
