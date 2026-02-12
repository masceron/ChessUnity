using UX.UI;
using UnityEngine;
using Game.Player;

namespace Game.Statue
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class StatueInteraction : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private LayerMask statueLayer;
        [SerializeField] private float maxInteractionDistance = 100f;

        private Camera mainCamera;
        private Statue currentStatue;
        private PlayerController player;

        private void Awake()
        {
            mainCamera = Camera.main;
            player = FindAnyObjectByType<PlayerController>();
        }

        private void Update()
        {
            HandleStatueClick();
        }

        private void HandleStatueClick()
        {
            if (UnityEngine.InputSystem.Mouse.current == null || mainCamera == null) return;

            if (UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
            {
                var mousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
                var ray = mainCamera.ScreenPointToRay(mousePos);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, maxInteractionDistance, statueLayer))
                {
                    var statue = hit.collider.GetComponent<Statue>();
                    if (statue != null && player != null && Vector3.Distance(statue.transform.position, player.transform.position) <= 1.5f)
                    {
                        OnStatueClicked(statue);
                    }
                }
            }
        }

        private void OnStatueClicked(Statue statue)
        {
            currentStatue = statue;
            ShowBattlePopup();
        }

        private void ShowBattlePopup()
        {
            UIManager.Ins.Load(CanvasID.StatueBattlePopup);
            
            var popup = FindFirstObjectByType<UX.UI.Popup.StatueBattlePopup>();
            if (popup != null)
            {
                popup.Initialize(currentStatue);
            }
        }
    }
}
