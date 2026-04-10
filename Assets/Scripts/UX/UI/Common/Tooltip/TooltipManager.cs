using Game.Common;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace UX.UI.Common.Tooltip
{
    public class TooltipManager : Singleton<TooltipManager>
    {
        private UIDocument _mainDocument;
        
        [Header("Setup")]
        [SerializeField] private VisualTreeAsset tooltipAsset;
        [SerializeField] private float showDelay = 0.4f;
        [SerializeField] private float lockDuration = 1.5f;

        private VisualElement _tooltipRoot;
        private VisualElement _tooltipStyleRoot;
        private Label _firstHeader;
        private Label _secondHeader;
        private Label _content;
        private RadialProgress _radialProgress;
        
        private enum TooltipState { Hidden, Delaying, Filling, Locked }
        private TooltipState _currentState = TooltipState.Hidden;
        
        private VisualElement _activeTrigger;
        private IVisualElementScheduledItem _gracePeriodTask;
        private float _timer;

        protected new void Awake()
        {
            _mainDocument = GetComponent<UIDocument>();
            if (!_mainDocument || !tooltipAsset) return;
            
            _tooltipRoot = tooltipAsset.Instantiate();
            _tooltipRoot.style.position = Position.Absolute;
            _tooltipRoot.style.display = DisplayStyle.None;
            _tooltipRoot.pickingMode = PickingMode.Position;

            _tooltipStyleRoot = _tooltipRoot.Q<VisualElement>("TooltipRoot");
            _mainDocument.rootVisualElement.Add(_tooltipRoot);
            
            _firstHeader = _tooltipRoot.Q<Label>("ToolTipFirstHeader");
            _secondHeader = _tooltipRoot.Q<Label>("TooltipSecondHeader");
            _content = _tooltipRoot.Q<Label>("TooltipContent");
            _radialProgress = _tooltipRoot.Q<RadialProgress>("TooltipLockRadial");
            
            _tooltipRoot.RegisterCallback<PointerEnterEvent>(OnTooltipHovered);
            _tooltipRoot.RegisterCallback<PointerLeaveEvent>(OnTooltipUnhovered);
        }

        private void Update()
        {
            if (_currentState == TooltipState.Hidden || _currentState == TooltipState.Locked) return;

            _timer += Time.deltaTime;

            if (_currentState == TooltipState.Delaying)
            {
                if (_timer >= showDelay)
                {
                    _currentState = TooltipState.Filling;
                    _timer = 0f;
                    ShowTooltipUI();
                }
            }
            else if (_currentState == TooltipState.Filling)
            {
                _radialProgress.Progress = 1f - (_timer / lockDuration);

                if (_timer >= lockDuration)
                {
                    LockTooltip();
                }
            }
        }

        public void HandlePointerEnter(VisualElement trigger, string hLeft, string hRight, string cnt)
        {
            _activeTrigger = trigger;
            _gracePeriodTask?.Pause();
            
            _firstHeader.text = hLeft;
            _secondHeader.text = hRight;
            _content.text = cnt;

            if (_currentState == TooltipState.Locked)
            {
                _currentState = TooltipState.Filling;
                _timer = 0f;
                ShowTooltipUI();
            }
            else
            {
                _currentState = TooltipState.Delaying;
                _timer = 0f;
                _tooltipRoot.style.display = DisplayStyle.None;
            }
        }

        public void HandlePointerLeave(VisualElement trigger)
        {
            if (_activeTrigger != trigger) return;

            switch (_currentState)
            {
                case TooltipState.Delaying or TooltipState.Filling:
                    ForceHide();
                    break;
                case TooltipState.Locked:
                    _gracePeriodTask = _tooltipRoot.schedule.Execute(CheckGracePeriod).StartingIn(200);
                    break;
            }
        }

        private void CheckGracePeriod()
        {
            var mousePos = Mouse.current.position.ReadValue();
            var screenPos = new Vector2(mousePos.x, Screen.height - mousePos.y);
            var panelPos = RuntimePanelUtils.ScreenToPanel(_tooltipRoot.panel, screenPos);
            
            if (!_tooltipRoot.worldBound.Contains(panelPos))
            {
                ForceHide();
            }
        }

        private void ShowTooltipUI()
        {
            _radialProgress.style.display = DisplayStyle.Flex;
            _radialProgress.Progress = 1f;
            _tooltipStyleRoot.RemoveFromClassList("tooltip-root-lock");
            
            _tooltipRoot.style.display = DisplayStyle.Flex;
            _tooltipRoot.style.visibility = Visibility.Hidden;
            
            _tooltipRoot.RegisterCallback<GeometryChangedEvent>(OnTooltipLayoutResolved);
        }

        private void OnTooltipLayoutResolved(GeometryChangedEvent evt)
        {
            _tooltipRoot.UnregisterCallback<GeometryChangedEvent>(OnTooltipLayoutResolved);
            
            if (_currentState == TooltipState.Hidden) return; 
            
            UpdateTooltipPosition(Mouse.current.position.ReadValue());
            
            _tooltipRoot.style.visibility = Visibility.Visible;
        }

        private void LockTooltip()
        {
            _currentState = TooltipState.Locked;
            _radialProgress.style.display = DisplayStyle.None;
            _tooltipStyleRoot.AddToClassList("tooltip-root-lock");
        }

        private void ForceHide()
        {
            _activeTrigger = null;
            _gracePeriodTask?.Pause();
            _currentState = TooltipState.Hidden;
            _tooltipRoot.style.display = DisplayStyle.None;
            _tooltipStyleRoot.RemoveFromClassList("tooltip-root-lock");
        }

        private void OnTooltipHovered(PointerEnterEvent evt)
        {
            _gracePeriodTask?.Pause();
        }

        private void OnTooltipUnhovered(PointerLeaveEvent evt)
        {
            ForceHide();
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