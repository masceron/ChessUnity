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

        private void OnEnable()
        {
            if (playerController != null)
            {
                playerController.OnMoveTargetSet += ShowEffect;
            }
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
            if (currentEffectInstance != null && currentEffectInstance.activeSelf && playerController != null)
            {
                var distance = Vector3.Distance(playerController.transform.position, currentEffectInstance.transform.position);
                
                if (distance < hideDistance)
                {
                    currentEffectInstance.SetActive(false);
                }
            }
        }

        private void ShowEffect(Vector3 position)
        {
            if (clickEffectPrefab == null) return;

            var spawnPos = position + Vector3.up * 0.1f;

            if (currentEffectInstance == null)
            {
                currentEffectInstance = Instantiate(clickEffectPrefab, spawnPos, Quaternion.Euler(90, 0, 0));
            }
            else
            {
                currentEffectInstance.transform.position = spawnPos;
                currentEffectInstance.SetActive(false); // Reset animation/particles
            }

            currentEffectInstance.SetActive(true);
        }
    }
}
