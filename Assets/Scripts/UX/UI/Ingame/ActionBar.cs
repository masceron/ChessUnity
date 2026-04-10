using System;
using System.Linq;
using Game.Action;
using Game.Action.Captures;
using Game.Action.Quiets;
using Game.Action.Skills;
using Game.Common;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Relics.Commons;
using UnityEngine;
using UnityEngine.UIElements;
using UX.UI.Common;
using UX.UI.Common.Tooltip;
using ZLinq;
using Action = Game.Action.Action;

namespace UX.UI.Ingame
{
    public class ActionBar : MonoBehaviour
    {
        private BoardViewer _boardViewer;
        private Button _quiets;
        private Button _captures;
        private Button _skills;
        private Button _relic;
        private Button _skip;
        private VisualElement _icon;

        private RadialProgress _skillCooldown;
        private RadialProgress _relicCooldown;

        private Label _skillCooldownTime;
        private Label _relicCooldownTime;

        private TooltipManipulator _tooltipManipulator;
        private RelicLogic _relicLogic;

        private void Awake()
        {
            _boardViewer = gameObject.GetComponent<BoardViewer>();
            _tooltipManipulator = null;

            var root = GetComponent<UIDocument>().rootVisualElement;
            _quiets = root.Q<Button>("Move");
            _captures = root.Q<Button>("Capture");
            _skills = root.Q<Button>("Skill");
            _relic = root.Q<Button>("Relic");
            _skip = root.Q<Button>("Skip");
            _icon = root.Q<VisualElement>("AllyPieceIcon");
            _skillCooldown = root.Q<RadialProgress>("SkillCooldownRadial");
            _relicCooldown = root.Q<RadialProgress>("RelicCooldownRadial");
            _skillCooldownTime = root.Q<Label>("SkillCooldown");
            _relicCooldownTime = root.Q<Label>("RelicCooldown");
            
            MatchManager.Ins.OnInitComplete += state =>
            {
                SetupRelic(state.Relics);
                state.PropertyChanged += (_, args) =>
                {
                    if (args.PropertyName == nameof(GameState.Relics))
                        HandleRelicChanges();
                };
            };
        }

        private void Start()
        {
            UpdateUIState(ControlState.Idle);
        }

        private void OnEnable()
        {
            _quiets.clicked += ClickMove;
            _captures.clicked += ClickCapture;
            _skills.clicked += ClickSkill;
            _relic.clicked += ClickRelic;
            _skip.clicked += ClickSkip;
            var input = GetComponent<InputManager>();
            input.OnMarkQuiets += ClickMove;
            input.OnMarkCaptures += ClickCapture;
            input.OnMarkSkills += ClickSkill;
            input.OnMarkRelics += ClickRelic;
            _boardViewer.OnStateChanged += UpdateUIState;
            _boardViewer.OnSelectingPieceChanged += SelectingPieceChangedHandler;
            input.OnMarkSkip += ClickSkip;
        }

        private void OnDisable()
        {
            _quiets.clicked -= ClickMove;
            _captures.clicked -= ClickCapture;
            _skills.clicked -= ClickSkill;
            _relic.clicked -= ClickRelic;
            _skip.clicked -= ClickSkip;
            var input = GetComponent<InputManager>();
            input.OnMarkQuiets -= ClickMove;
            input.OnMarkCaptures -= ClickCapture;
            input.OnMarkSkills -= ClickSkill;
            input.OnMarkRelics -= ClickRelic;
            _boardViewer.OnStateChanged -= UpdateUIState;
            _boardViewer.OnSelectingPieceChanged -= SelectingPieceChangedHandler;
            input.OnMarkSkip -= ClickSkip;
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
            _relic.RemoveFromClassList(activeClass);

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
                    _relic.AddToClassList(activeClass);
                    break;
                case ControlState.PieceSelected:
                    var piece = _boardViewer.SelectingPiece;

                    //if (piece.Color != BoardUtils.OurSide()) return;
                    
                    _quiets.SetEnabled(_boardViewer.AllMoves.Count(a => a is IQuiets) != 0);
                    _captures.SetEnabled(_boardViewer.AllMoves.Count(a => a is ICaptures) != 0);
                    _skills.SetEnabled(_boardViewer.AllMoves.Count(a => a is ISkills) != 0);
                    
                    if (piece.SkillCooldown > 0)
                    {
                        var timeToCooldown = ((IPieceWithSkill)piece).TimeToCooldown;
                        _skillCooldown.Progress =
                            (float)(timeToCooldown - piece.SkillCooldown) / timeToCooldown;
                        _skillCooldownTime.text = piece.SkillCooldown.ToString();
                    }
                    break;
                case ControlState.Idle:
                    _quiets.SetEnabled(false);
                    _captures.SetEnabled(false);
                    _skills.SetEnabled(false);
                    _skip.SetEnabled(BoardUtils.OurSide() == BoardUtils.SideToMove());
                    
                    _skillCooldown.Progress = 1f;
                    _skillCooldownTime.text = "";
                    break;
                case ControlState.TargetingPending:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }

        private void SelectingPieceChangedHandler(PieceLogic pieceLogic)
        {
            _skills.RemoveTooltip(_tooltipManipulator);
            _tooltipManipulator = null;
            
            if (pieceLogic == null || pieceLogic.Color != BoardUtils.OurSide())
            {
                _icon.style.backgroundImage = null;
                return;
            }

            _icon.style.backgroundImage = AssetManager.Ins.PieceData[pieceLogic.Type].icon;
            if (pieceLogic is not IPieceWithSkill) return;
            
            var data = AssetManager.Ins.PieceData[pieceLogic.Type];
            _tooltipManipulator = _skills.AddTooltip(Localizer.GetText("piece_skill", data.key + "_skill", null),
                "",
                Localizer.GetText("piece_skill_description", data.key + "_skill_description", null));
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
            if (!_relic.enabledSelf) return;
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
                        _boardViewer.CurrentAvailableMoves.Clear();
                        _boardViewer.ClearHighlights();
                        BoardUtils.GetRelicOf(BoardUtils.SideToMove()).Activate(_boardViewer.CurrentAvailableMoves);

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
        
        private void SetupRelic((RelicLogic, RelicLogic) relics)
        {
            _relicLogic = BoardUtils.OurSide() ? relics.Item2 : relics.Item1;
            var data = AssetManager.Ins.RelicData[_relicLogic.Type];
            _relic.AddTooltip(Localizer.GetText("relic_name", data.key, null), "",
                Localizer.GetText("relic_description", $"{data.key}_description", null)); 
            HandleRelicChanges();
        }

        private void HandleRelicChanges()
        {
            _relicCooldown.Progress = 1f;
            _relicCooldownTime.text = "";
            //_relic.SetEnabled(false);

            switch (_relicLogic.CurrentCooldown)
            {
                case 0:
                    _relic.SetEnabled(true);
                    return;
                case > 0:
                    _relicCooldown.Progress = (float)(_relicLogic.TimeCooldown - _relicLogic.CurrentCooldown) /
                                                 _relicLogic.TimeCooldown;
                    _relicCooldownTime.text = _relicLogic.CurrentCooldown.ToString();
                    break;
            }
        }
    }
}