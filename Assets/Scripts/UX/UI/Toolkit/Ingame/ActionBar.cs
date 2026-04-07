using System;
using System.Linq;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Action.Relics;
using Game.Action.Skills;
using Game.Common;
using UnityEngine;
using UnityEngine.UIElements;
using ZLinq;
using Action = Game.Action.Action;

namespace UX.UI.Toolkit.Ingame
{
    public class ActionBar : MonoBehaviour
    {
        private BoardViewer _boardViewer;
        private Button _quiets;
        private Button _captures;
        private Button _skills;
        private Button _relics;
        private Button _skip;

        private void Awake()
        {
            _boardViewer = gameObject.GetComponent<BoardViewer>();

            var root = GetComponent<UIDocument>().rootVisualElement;
            _quiets = root.Q<Button>("Move");
            _quiets.clicked += ClickMove;

            _captures = root.Q<Button>("Capture");
            _captures.clicked += ClickCapture;

            _skills = root.Q<Button>("Skill");
            _skills.clicked += ClickSkill;

            _relics = root.Q<Button>("Relic");
            _relics.clicked += ClickRelic;

            _skip = root.Q<Button>("Skip");
            _skip.clicked += ClickSkip;

            var input = GetComponent<InputManager>();
            input.OnMarkQuiets += ClickMove;
            input.OnMarkCaptures += ClickCapture;
            input.OnMarkSkills += ClickSkill;
            input.OnMarkRelics += ClickRelic;
            _boardViewer.OnStateChanged += UpdateUIState;

            input.OnMarkSkip += ClickSkip;
        }

        private void ClickSkip()
        {
            if (!_skip.enabledSelf) return;
                _boardViewer.ExecuteAction(new SkipTurn());
        }

        private void UpdateUIState(ControlState newState)
        {
            const string activeClass = "active-function";

            _quiets.RemoveFromClassList(activeClass);
            _captures.RemoveFromClassList(activeClass);
            _skills.RemoveFromClassList(activeClass);
            _relics.RemoveFromClassList(activeClass);

            switch (newState)
            {
                case ControlState.TargetingMove:
                    _quiets.AddToClassList(activeClass);
                    break;
                case ControlState.TargetingAttack:
                    _captures.AddToClassList(activeClass);
                    break;
                case ControlState.TargetingSkill:
                    _skills.AddToClassList(activeClass);
                    break;
                case ControlState.TargetingRelic:
                    _relics.AddToClassList(activeClass);
                    break;
                case ControlState.PieceSelected:
                    _quiets.SetEnabled(_boardViewer.AllMoves.Count(a => a is IQuiets) != 0);
                    _captures.SetEnabled(_boardViewer.AllMoves.Count(a => a is ICaptures) != 0);
                    _skills.SetEnabled(_boardViewer.AllMoves.Count(a => a is ISkills) != 0);
                    break;
                case ControlState.Idle:
                    _quiets.SetEnabled(false);
                    _captures.SetEnabled(false);
                    _skills.SetEnabled(false);
                    _relics.SetEnabled(BoardUtils.OurSide() == BoardUtils.SideToMove());
                    _skip.SetEnabled(BoardUtils.OurSide() == BoardUtils.SideToMove());
                    break;
                case ControlState.TargetingPending:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }

        private void ClickMove()
        {
            if (!_quiets.enabledSelf) return;
            switch (_boardViewer.CurrentState)
            {
                case ControlState.TargetingMove:
                    _boardViewer.ClearHighlights();
                    _boardViewer.CurrentState = ControlState.PieceSelected;
                    break;
                case ControlState.PieceSelected:
                case ControlState.TargetingAttack:
                case ControlState.TargetingRelic:
                case ControlState.TargetingSkill:
                    if (_boardViewer.SelectingPiece != null &&
                        _boardViewer.SelectingPiece.Color == BoardUtils.SideToMove())
                        //&& BoardUtils.OurSide() == BoardUtils.SideToMove())
                    {
                        _boardViewer.CurrentState = ControlState.TargetingMove;
                        _boardViewer.SelectingPiece.MoveList(_boardViewer.AllMoves);
                        _boardViewer.ClearHighlights();
                        _boardViewer.CurrentAvailableMoves.Clear();
                        foreach (var move in Enumerable.OfType<IQuiets>(_boardViewer.AllMoves))
                        {
                            _boardViewer.CurrentAvailableMoves.Add((Action)move);
                        }

                        _boardViewer.Highlighter(
                            _boardViewer.CurrentAvailableMoves.Select(a => a.GetTargetPos()).ToList());
                    }

                    break;
                case ControlState.Idle:
                case ControlState.TargetingPending:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ClickCapture()
        {
            if (!_captures.enabledSelf) return;
            switch (_boardViewer.CurrentState)
            {
                case ControlState.TargetingAttack:
                    _boardViewer.ClearHighlights();
                    _boardViewer.CurrentState = ControlState.PieceSelected;
                    break;
                case ControlState.PieceSelected:
                case ControlState.TargetingMove:
                case ControlState.TargetingRelic:
                case ControlState.TargetingSkill:
                    if (_boardViewer.SelectingPiece != null &&
                        _boardViewer.SelectingPiece.Color == BoardUtils.SideToMove())
                        //&& BoardUtils.OurSide() == BoardUtils.SideToMove())
                    {
                        _boardViewer.CurrentState = ControlState.TargetingAttack;
                        _boardViewer.SelectingPiece.MoveList(_boardViewer.AllMoves);
                        _boardViewer.CurrentAvailableMoves.Clear();
                        _boardViewer.ClearHighlights();
                        foreach (var move in Enumerable.OfType<ICaptures>(_boardViewer.AllMoves))
                        {
                            _boardViewer.CurrentAvailableMoves.Add((Action)move);
                        }

                        _boardViewer.Highlighter(
                            _boardViewer.CurrentAvailableMoves.Select(a => a.GetTargetPos()).ToList());
                    }

                    break;
                case ControlState.Idle:
                case ControlState.TargetingPending:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ClickSkill()
        {
            if (!_skills.enabledSelf) return;
            switch (_boardViewer.CurrentState)
            {
                case ControlState.TargetingSkill:
                    _boardViewer.ClearHighlights();
                    _boardViewer.CurrentState = ControlState.PieceSelected;
                    break;
                case ControlState.PieceSelected:
                case ControlState.TargetingMove:
                case ControlState.TargetingRelic:
                case ControlState.TargetingAttack:
                    if (_boardViewer.SelectingPiece != null &&
                        _boardViewer.SelectingPiece.Color == BoardUtils.SideToMove())
                        //&& BoardUtils.OurSide() == BoardUtils.SideToMove())
                    {
                        _boardViewer.CurrentState = ControlState.TargetingSkill;
                        _boardViewer.SelectingPiece.MoveList(_boardViewer.AllMoves);
                        _boardViewer.CurrentAvailableMoves.Clear();
                        _boardViewer.ClearHighlights();
                        foreach (var move in Enumerable.OfType<ISkills>(_boardViewer.AllMoves))
                        {
                            _boardViewer.CurrentAvailableMoves.Add((Action)move);
                        }

                        _boardViewer.Highlighter(
                            _boardViewer.CurrentAvailableMoves.Select(a => a.GetTargetPos()).ToList());
                    }

                    break;
                case ControlState.Idle:
                case ControlState.TargetingPending:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ClickRelic()
        {
            if (!_relics.enabledSelf) return;
            switch (_boardViewer.CurrentState)
            {
                case ControlState.TargetingRelic:
                    _boardViewer.ClearHighlights();
                    _boardViewer.CurrentState = ControlState.PieceSelected;
                    break;
                case ControlState.PieceSelected:
                case ControlState.TargetingMove:
                case ControlState.TargetingSkill:
                case ControlState.TargetingAttack:
                    if (_boardViewer.SelectingPiece != null &&
                        _boardViewer.SelectingPiece.Color == BoardUtils.SideToMove())
                        //&& BoardUtils.OurSide() == BoardUtils.SideToMove())
                    {
                        _boardViewer.CurrentState = ControlState.TargetingRelic;
                        _boardViewer.SelectingPiece.MoveList(_boardViewer.AllMoves);
                        _boardViewer.CurrentAvailableMoves.Clear();
                        _boardViewer.ClearHighlights();
                        foreach (var move in Enumerable.OfType<IRelicAction>(_boardViewer.AllMoves))
                        {
                            _boardViewer.CurrentAvailableMoves.Add((Action)move);
                        }

                        _boardViewer.Highlighter(
                            _boardViewer.CurrentAvailableMoves.Select(a => a.GetTargetPos()).ToList());
                    }

                    break;
                case ControlState.Idle:
                case ControlState.TargetingPending:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}