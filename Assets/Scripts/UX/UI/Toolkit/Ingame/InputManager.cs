using System;
using Game.Common;
using Game.Tile;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UX.UI.Toolkit.Ingame
{
    public class InputManager: MonoBehaviour
    {
        [NonSerialized] private UnityEngine.Camera _mainCamera;
        [NonSerialized] private BoardViewer _boardViewer;
        private PlayerInput _controls;
        private Tile _currentHoveredTile;
        
        public event Action<int> OnTileLeftClicked;
        public event Action OnRightClicked;
        public event Action<int> OnTileHovered;
        public event Action OnMarkQuiets;
        public event Action OnMarkCaptures;
        public event Action OnMarkSkills;
        public event Action OnMarkRelics;
        public event Action OnMarkSkip;
        public event Action OnMenuToggle;
        
        private int _tileLayerMask;

        private void Awake()
        {
            _mainCamera = UnityEngine.Camera.main;
            _controls = new PlayerInput();
            _boardViewer = GetComponent<BoardViewer>();
            _tileLayerMask = LayerMask.GetMask("Tile");

            _controls.PlayerActions.Select.performed += HandleSelect;
            _controls.PlayerActions.Cancel.performed += HandleCancel;
            _controls.PlayerActions.MarkQuiets.performed += HandleMarkQuiets;
            _controls.PlayerActions.MarkCaptures.performed += HandleMarkCaptures;
            _controls.PlayerActions.MarkSkill.performed += HandleMarkSkills;
            _controls.PlayerActions.MarkRelics.performed += HandleMarkRelics;
            _controls.PlayerActions.SkipTurn.performed += HandleSkipTurn;
            _controls.PlayerActions.Quit.performed += HandleEsc;
        }
        
        private void OnEnable()
        {
            _controls.Enable();
        }

        private void Update()
        {
            HandleHover();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }

        private void OnDestroy()
        {
            _controls.PlayerActions.Select.performed -= HandleSelect;
            _controls.PlayerActions.Cancel.performed -= HandleCancel;
        }

        private void HandleHover()
        {
            var screenPosition = Mouse.current.position.ReadValue();
            var ray = _mainCamera.ScreenPointToRay(screenPosition);
            
            if (Physics.Raycast(ray, out var hit, 100f, _tileLayerMask))
            {
                var hitTile = hit.collider.GetComponent<Tile>();

                if (!hitTile || hitTile == _currentHoveredTile) return;
                
                _currentHoveredTile = hitTile;
                var pos = BoardUtils.IndexOf(hitTile.Rank, hitTile.File);
                OnTileHovered?.Invoke(pos);
            }
            else if (_currentHoveredTile)
            {
                _currentHoveredTile = null;
                OnTileHovered?.Invoke(-1);
            }
        }

        private void HandleSelect(InputAction.CallbackContext context)
        {
            if (!_currentHoveredTile) return;
            
            var pos = BoardUtils.IndexOf(_currentHoveredTile.Rank, _currentHoveredTile.File);
            OnTileLeftClicked?.Invoke(pos);
        }

        private void HandleCancel(InputAction.CallbackContext context)
        {
            OnRightClicked?.Invoke();
        }

        private void HandleMarkQuiets(InputAction.CallbackContext context)
        {
            OnMarkQuiets?.Invoke();
        }
        
        private void HandleMarkCaptures(InputAction.CallbackContext context)
        {
            OnMarkCaptures?.Invoke();
        }
        
        private void HandleMarkSkills(InputAction.CallbackContext context)
        {
            OnMarkSkills?.Invoke();
        }
        
        private void HandleMarkRelics(InputAction.CallbackContext context)
        {
            OnMarkRelics?.Invoke();
        }

        private void HandleSkipTurn(InputAction.CallbackContext context)
        {
            OnMarkSkip?.Invoke();
        }

        private void HandleEsc(InputAction.CallbackContext context)
        {
            if (UIManager.Ins.HasActiveMenus)
            {
                UIManager.Ins.Pop();
                return;
            }
            
            if (_boardViewer.CurrentState == ControlState.TargetingPending && _boardViewer.PendingTask != null)
            {
                _boardViewer.PendingTask.TrySetCanceled();
                _boardViewer.PendingTask = null;
                _boardViewer.Idle();
                return;
            }
    
            if (_boardViewer.CurrentState != ControlState.Idle)
            {
                _boardViewer.Idle();
                return;
            }
            
            OnMenuToggle?.Invoke();
        }
    }
}