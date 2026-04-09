using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Action;
using Game.Action.Internal.Pending;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using PrimeTween;
using UnityEngine;
using UnityEngine.UIElements;
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
        public UniTaskCompletionSource<int> PendingTask;
        private Func<int, bool> _pendingClickValidator;

        [NonSerialized] private Transform _centerOfView;
        [NonSerialized] private InputManager _inputManager;

        private ControlState _currentState = ControlState.Idle;

        public ControlState CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState == value) return;
                _currentState = value;
                OnStateChanged?.Invoke(_currentState);
            }
        }

        public event Action<ControlState> OnStateChanged;

        public event Action<PieceLogic> OnSelectingPieceChanged;
        private PieceLogic _selectingPiece;

        public PieceLogic SelectingPiece
        {
            get => _selectingPiece;
            private set
            {
                if (_selectingPiece == value) return;
                _selectingPiece = value;
                OnSelectingPieceChanged?.Invoke(_selectingPiece);
            }
        }

        [NonSerialized] public List<Action> AllMoves;
        [NonSerialized] public List<Action> CurrentAvailableMoves;

        private void Awake()
        {
            _inputManager = gameObject.GetComponent<InputManager>();
            AllMoves = new List<Action>();
            CurrentAvailableMoves = new List<Action>();
            _centerOfView = GameObject.Find("CameraTarget").GetComponent<Transform>();
            gameObject.GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            CurrentState = ControlState.Idle;
            SelectingPiece = null;

            _inputManager.OnTileLeftClicked += TileClick;
            _inputManager.OnRightClicked += CancelClick;
            _inputManager.OnTileHovered += Hover;
        }

        private void OnDisable()
        {
            _inputManager.OnTileLeftClicked -= TileClick;
            _inputManager.OnRightClicked -= CancelClick;
            _inputManager.OnTileHovered -= Hover;
        }

        public async void ExecuteAction(Action action)
        {
            try
            {
                if (action is PendingAction pendingAction)
                {
                    CurrentState = ControlState.TargetingPending;
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

        public void Idle()
        {
            CurrentState = ControlState.Idle;
            SelectingPiece = null;
            AllMoves.Clear();
            CurrentAvailableMoves.Clear();
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
            SelectingPiece = piece;
            TileManager.Ins.Select(piece.Pos);
            PanTo(piece.Pos);
            CurrentState = ControlState.PieceSelected;
        }

        private void TileClick(int position)
        {
            switch (CurrentState)
            {
                case ControlState.Idle:
                {
                    var pieceClicked = BoardUtils.PieceOn(position);
                    if (pieceClicked != null)
                    {
                        if (pieceClicked.Color == BoardUtils.SideToMove())
                        {
                            pieceClicked.MoveList(AllMoves);
                        }

                        Select(pieceClicked);
                    }

                    break;
                }
                case ControlState.PieceSelected:
                {
                    var pieceClicked = BoardUtils.PieceOn(position);
                    if (pieceClicked != null)
                    {
                        if (pieceClicked != SelectingPiece)
                        {
                            Idle();
                            if (pieceClicked.Color == BoardUtils.SideToMove())
                            {
                                AllMoves.Clear();
                                pieceClicked.MoveList(AllMoves);
                            }

                            Select(pieceClicked);
                        }
                        else
                        {
                            Idle();
                        }
                    }

                    break;
                }
                case ControlState.TargetingMove:
                case ControlState.TargetingAttack:
                case ControlState.TargetingSkill:
                case ControlState.TargetingRelic:
                    var move = CurrentAvailableMoves.Find(a => a.GetTargetPos() == position);
                    if (move != null)
                    {
                        ExecuteAction(move);
                    }

                    break;
                case ControlState.TargetingPending:
                    if (PendingTask != null)
                    {
                        if (_pendingClickValidator == null || _pendingClickValidator(position))
                        {
                            var tcs = PendingTask;
                            PendingTask = null;
                            tcs.TrySetResult(position);
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CancelClick()
        {
            switch (CurrentState)
            {
                case ControlState.TargetingPending when PendingTask != null:
                    PendingTask.TrySetCanceled();
                    PendingTask = null;
                    break;
                case ControlState.PieceSelected:
                    Idle();
                    break;
                case ControlState.Idle:
                    break;
                case ControlState.TargetingMove:
                case ControlState.TargetingAttack:
                case ControlState.TargetingSkill:
                case ControlState.TargetingRelic:
                default:
                    CurrentAvailableMoves.Clear();
                    ClearHighlights();
                    CurrentState = ControlState.PieceSelected;
                    break;
            }
        }

        private void Hover(int position)
        {
        }

        public UniTask<int> NextSelection(Func<int, bool> validator)
        {
            CurrentState = ControlState.TargetingPending;
            _pendingClickValidator = validator;
            PendingTask = new UniTaskCompletionSource<int>();

            return PendingTask.Task;
        }

        public void Highlighter(IEnumerable<int> positions)
        {
            foreach (var position in positions)
            {
                TileManager.Ins.MarkAsMoveable(position);
            }
        }

        public void ClearHighlights()
        {
            TileManager.Ins.UnmarkAll();
        }

        public void ClearHighlight(int position)
        {
            TileManager.Ins.UnMark(position);
        }

        public UniTask<TResult> OpenMenu<TPayload, TResult>(InGameMenuType menuType, TPayload payload)
        {
            return UIManager.Ins.OpenMenu<TPayload, TResult>(menuType, payload);
        }
    }
}