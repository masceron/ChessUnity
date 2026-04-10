using Game.Effects;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;
using UnityEngine.UIElements;
using UX.UI.Common.Tooltip;

namespace UX.UI.Ingame
{
    public class EffectBar : MonoBehaviour
    {
        [Header("Setup")] [SerializeField] private VisualTreeAsset effectIcon;

        [SerializeField] private Texture2D missing;

        private UIDocument _uiDocument;
        private BoardViewer _boardViewer;
        private VisualElement _effectBar;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _boardViewer = GetComponent<BoardViewer>();
            _effectBar = _uiDocument.rootVisualElement.Q<VisualElement>("EffectBar");
        }

        private void OnEnable()
        {
            _boardViewer.OnSelectingPieceChanged += SelectingPieceHandler;
        }

        private void OnDisable()
        {
            _boardViewer.OnSelectingPieceChanged -= SelectingPieceHandler;
        }

        private void SelectingPieceHandler(PieceLogic pieceLogic)
        {
            _effectBar.Clear();
            if (pieceLogic == null) return;

            foreach (var effect in pieceLogic.Effects)
            {
                var newIcon = effectIcon.Instantiate();
                SetupIcon(newIcon, effect);
                _effectBar.Add(newIcon);
            }
        }

        private void SetupIcon(VisualElement instance, Effect effect)
        {
            var root = instance.Q<VisualElement>("Root");
            var data = AssetManager.Ins.EffectData[effect.EffectName];
            var icon = data.icon;
            root.style.backgroundImage = icon ? icon : missing;
            root.AddTooltip(Localizer.GetText("effect_name", data.key, null),
                effect.Duration > 0 ? $"{effect.Duration.ToString()} turn(s) left" : "∞",
                effect.Description());
        }
    }
}