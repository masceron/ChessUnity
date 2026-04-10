using Game.Common;
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
        
        private BoardViewer _boardViewer;
        private VisualElement _effectBar;
        private VisualElement _enemyEffectBar;

        private void Awake()
        {
            var uiDocument = GetComponent<UIDocument>();
            _boardViewer = GetComponent<BoardViewer>();
            _effectBar = uiDocument.rootVisualElement.Q<VisualElement>("EffectBar");
            _enemyEffectBar = uiDocument.rootVisualElement.Q<VisualElement>("EnemyEffectBar");
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
            _enemyEffectBar.Clear();
            if (pieceLogic == null) return;

            var ourSide = BoardUtils.OurSide() == pieceLogic.Color;

            foreach (var effect in pieceLogic.Effects)
            {
                var newIcon = effectIcon.Instantiate();
                SetupIcon(newIcon, effect, ourSide);
                (ourSide ? _effectBar : _enemyEffectBar).Add(newIcon);
            }
        }

        private void SetupIcon(VisualElement instance, Effect effect, bool ourSide)
        {
            var root = instance.Q<VisualElement>("EffectRoot");
            var data = AssetManager.Ins.EffectData[effect.EffectName];
            var icon = data.icon;
            root.style.backgroundImage = icon ? icon : missing;
            root.AddTooltip(Localizer.GetText("effect_name", data.key, null),
                effect.Duration > 0 ? $"{effect.Duration.ToString()} turn(s) left" : "∞",
                effect.Description());
            
            if (ourSide) return;
            
            root.style.width = 50;
            root.style.height = 50;
        }
    }
}