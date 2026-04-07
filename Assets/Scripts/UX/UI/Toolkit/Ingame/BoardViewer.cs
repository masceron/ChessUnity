using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal.Pending;
using Game.Action.Quiets;
using Game.Action.Relics;
using Game.Action.Skills;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using PrimeTween;
using UnityEngine;
using UX.UI.Toolkit.Common;
using Action = Game.Action.Action;

namespace UX.UI.Toolkit.Ingame
{
    public enum ControlState
    {
        Idle,
        PieceSelected,
        TargetingMove,
        TargetingAttack,
        TargetingSkill,
        TargetingRelic,
        TargetingPending,
    }

    public class BoardViewer : MonoBehaviour, ITargetingContext
    {
        private UniTaskCompletionSource<int> _pendingClickTcs;
        private Func<int, bool> _pendingClickValidator;
        
        [NonSerialized] private Transform _centerOfView;
        [NonSerialized] private InputManager _inputManager;
        [NonSerialized] private ControlState _currentState;
        [NonSerialized] private PieceLogic _selectingPiece;
        [NonSerialized] private List<Action> _availableMoves;
        
        [SerializeField] private UDictionary<InGameMenuType, GameObject> menuTemplates;
        [SerializeField] private Transform uiParent;

        private void Awake()
        {
            _inputManager = gameObject.GetComponent<InputManager>();
            _availableMoves = new List<Action>();
            _centerOfView = GameObject.Find("CameraTarget").GetComponent<Transform>();
        }

        private void OnEnable()
        {
            _currentState = ControlState.Idle;
            _selectingPiece = null;

            _inputManager.OnTileLeftClicked += TileClick;
            _inputManager.OnRightClicked += CancelClick;
            _inputManager.OnTileHovered += Hover;
        }

        private void OnDisable()
        {
            _inputManager.OnTileLeftClicked -= TileClick;
        }

        private async void ExecuteAction(Action action)
        {
            try
            {
                if (action is PendingAction pendingAction) 
                {
                    action = await pendingAction.WaitForCompletion(this);
                    
                    if (action == null) 
                    {
                        Idle();
                        return;
                    }
                }
            
                ActionManager.DoManualAction(action);
                Idle();
            }
            catch (Exception)
            {
                Idle();
            }
        }

        private void Idle()
        {
            _currentState = ControlState.Idle;
            _selectingPiece = null;
            _availableMoves.Clear();
            TileManager.Ins.UnmarkAll();
        }

        private void PanTo(int position)
        {
            var direction = _centerOfView.position;
            direction.x = BoardUtils.RankOf(position);
            direction.z = BoardUtils.FileOf(position);

            Tween.Position(_centerOfView, direction, 0.3f);
        }

        private void Select(PieceLogic piece)
        {
            _currentState = ControlState.PieceSelected;
            _selectingPiece = piece;
            TileManager.Ins.Select(piece.Pos);
            PanTo(piece.Pos);
        }

        private void TileClick(int position)
        {
            Action move;
            switch (_currentState)
            {
                case ControlState.Idle:
                {
                    var pieceClicked = BoardUtils.PieceOn(position);
                    if (pieceClicked != null)
                    {
                        Select(pieceClicked);
                        if (pieceClicked.Color == BoardUtils.OurSide())
                        {
                            pieceClicked.MoveList(_availableMoves);
                        }
                    }

                    break;
                }
                case ControlState.PieceSelected:
                {
                    var pieceClicked = BoardUtils.PieceOn(position);
                    if (pieceClicked != null)
                    {
                        if (pieceClicked != _selectingPiece)
                        {
                            Select(pieceClicked);
                            if (pieceClicked.Color == BoardUtils.OurSide())
                            {
                                _availableMoves.Clear();
                                pieceClicked.MoveList(_availableMoves);
                            }
                        }
                        else
                        {
                            Idle();
                        }
                    }
                    break;
                }
                case ControlState.TargetingMove:
                    move = _availableMoves.Find(a => a is IQuiets && a.GetTargetPos() == position);
                    if (move != null)
                    {
                        Idle();
                        ExecuteAction(move);
                    }

                    break;
                case ControlState.TargetingAttack:
                    move = _availableMoves.Find(a => a is ICaptures && a.GetTargetPos() == position);
                    if (move != null)
                    {
                        Idle();
                        ExecuteAction(move);
                    }

                    break;
                case ControlState.TargetingSkill:
                    move = _availableMoves.Find(a => a is ISkills && a.GetTargetPos() == position);
                    if (move != null)
                    {
                        Idle();
                        ExecuteAction(move);
                    }

                    break;
                case ControlState.TargetingRelic:
                    move = _availableMoves.Find(a => a is IRelicAction && a.GetTargetPos() == position);
                    if (move != null)
                    {
                        Idle();
                        ExecuteAction(move);
                    }

                    break;
                case ControlState.TargetingPending:
                    if (_pendingClickTcs != null)
                    {
                        if (_pendingClickValidator == null || _pendingClickValidator(position))
                        {
                            var tcs = _pendingClickTcs;
                            _pendingClickTcs = null;
                            tcs.TrySetResult(position);
                        }
                    }
                    break;
            }
        }

        private void CancelClick()
        {
            if (_currentState == ControlState.TargetingPending && _pendingClickTcs != null)
            {
                _pendingClickTcs.TrySetCanceled();
                _pendingClickTcs = null;
            }
            Idle();
        }

        private void Hover(int position)
        {
            
        }

        public UniTask<int> NextSelection(Func<int, bool> validator)
        {
            _currentState = ControlState.TargetingPending;
            _pendingClickValidator = validator;
            _pendingClickTcs = new UniTaskCompletionSource<int>();
        
            return _pendingClickTcs.Task;
        }
        
        public void Highlighter(IEnumerable<int> positions)
        {
            
        }

        public void ClearHighlights()
        {
            throw new NotImplementedException();
        }
        
        public async UniTask<TResult> OpenMenu<TResult, TPayload>(InGameMenuType menuType, TPayload payload)
        {
            if (!menuTemplates.TryGetValue(menuType, out var prefab))
            {
                throw new Exception($"No UI registered for MenuType: {menuType}");
            }
            
            var uiInstance = Instantiate(prefab, uiParent);
            
            var awaitableUI = uiInstance.GetComponent<IAwaitableUI<TResult, TPayload>>();

            try
            {
                var result = await awaitableUI.WaitForSelection(payload);
                return result;
            }
            finally
            {
                if (uiInstance) Destroy(uiInstance);
            }
        }
    }
}