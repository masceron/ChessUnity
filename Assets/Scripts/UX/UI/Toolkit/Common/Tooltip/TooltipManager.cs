using Game.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace UX.UI.Toolkit.Common.Tooltip
{
    public class TooltipManager : Singleton<TooltipManager>
    {
        private UIDocument _mainDocument;
        
        [Header("Setup")]
        [SerializeField] private VisualTreeAsset tooltipAsset;

        private VisualElement _tooltipRoot;
        private Label _headerLeft;
        private Label _headerRight;
        private Label _content;
        
        private bool _isShowing;

        protected new void Awake()
        {
            _mainDocument = GetComponent<UIDocument>();
            if (!_mainDocument || !tooltipAsset)
            {
                return;
            }
            
            _tooltipRoot = tooltipAsset.Instantiate();
            _tooltipRoot.style.position = Position.Absolute;
            _tooltipRoot.style.display = DisplayStyle.None;
            _mainDocument.rootVisualElement.Add(_tooltipRoot);
            
            _headerLeft = _tooltipRoot.Q<Label>("HeaderLeft");
            _headerRight = _tooltipRoot.Q<Label>("HeaderRight");
            _content = _tooltipRoot.Q<Label>("Content");
        }

        private void Update()
        {
            if (!_isShowing) return;
            
            var mousePos = Mouse.current.position.ReadValue();
            UpdateTooltipPosition(mousePos);
        }

        public void Show(string hLeft, string hRight, string cnt)
        {
            _headerLeft.text = hLeft;
            _headerRight.text = hRight;
            _content.text = cnt;
            _isShowing = true;
            
            UpdateTooltipPosition(Mouse.current.position.ReadValue());

            _tooltipRoot.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            _tooltipRoot.style.display = DisplayStyle.None;
            _isShowing = false;
        }

        private void UpdateTooltipPosition(Vector2 mousePos)
        {
            if (_tooltipRoot.panel == null) return;
            
            var screenPos = new Vector2(mousePos.x, Screen.height - mousePos.y);
            
            var panelPos = RuntimePanelUtils.ScreenToPanel(_tooltipRoot.panel, screenPos);
            
            var width = float.IsNaN(_tooltipRoot.layout.width) ? 0 : _tooltipRoot.layout.width;
            var height = float.IsNaN(_tooltipRoot.layout.height) ? 0 : _tooltipRoot.layout.height;
            
            var panelWidth = _tooltipRoot.panel.visualTree.layout.width;
            var panelHeight = _tooltipRoot.panel.visualTree.layout.height;
            
            const float offset = 10f; 
    
            if (panelPos.x + width + offset > panelWidth) panelPos.x -= (width + offset);
            else panelPos.x += offset;

            if (panelPos.y + height + offset > panelHeight) panelPos.y -= (height + offset);
            else panelPos.y += offset;
            
            _tooltipRoot.style.left = panelPos.x;
            _tooltipRoot.style.top = panelPos.y;
        }
    }
}